using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UMLTree : MonoBehaviour
{
    private long? _id;
    public long ID
    {
        get
        {
            if (_id == null)
            {
                _id = DateTime.UtcNow.ToBinary();
            }
            return _id ?? 0;
        }
        set
        {
            if (_id == null)
            {
                _id = value;
                name = UTreeName;
            }
            else
            {
                Debug.LogError("Tried to set ID");
            }
        }
    }

    [SerializeField]
    public string InitialName = "Tree";

    private string _treeName;
    public string TreeName
    {
        get
        {
            if ((String.IsNullOrEmpty(_treeName)))
            {
                TreeName = InitialName;
            }
            return _treeName;
        }
        set
        {
            if ((!String.IsNullOrEmpty(value)) && _treeName != value)
            {
                _treeName = value;
                name = UTreeName;
            }
        }
    }
    public string UTreeName
    {
        get
        {
            return $"{TreeName}_{ID}";
        }
    }


    [SerializeField]
    private AUMLElement _startElement;
    public AUMLElement StartElement
    {
        get { return _startElement; }
        private set { _startElement = value; }
    }

    public UnityEvent OnDestroyEvent;

    private void OnDestroy()
    {
        OnDestroyEvent.Invoke();
        //UMLManager.Instance.RemoveTree(this);
    }

    public bool ApplySimpleData(UMLTreeData data)
    {
        ID = data.ID;
        TreeName = data.TreeName;
        return true;
    }

    public bool ApplyElementData(UMLTreeData data)
    {
        // Create Elements and Apply the data
        Dictionary<UMLElementData, AUMLElement> newElements = new Dictionary<UMLElementData, AUMLElement>();
        List<AUMLElement> elementsWithNoConnectionYet = new List<AUMLElement>();

        foreach (UMLElementData elementData in data.elements.Reverse())
        {
            Type elementType = elementData.UMLType;

            UMLElementTypePrefab etp = UMLManager.Instance.ElementTypePrefabs.Find(e => e.type == elementType);

            if (etp == null)
            {
                continue;
            }

            AUMLElement newElement = Instantiate(etp.prefab, transform);

            // apply data to each block
            if (!newElement.ApplySimpleData(elementData))
            {
                Debug.LogWarning($"UML Tree: Could not Apply Simple Data to element {elementData.ID}");
                return false;
            }

            newElements.Add(elementData, newElement);

            if (!newElement.ApplyConnectionData(elementData, newElements))
            {
                Debug.LogWarning($"UML Tree: Could not Connection Data to element {elementData.ID} - will retry later");
                elementsWithNoConnectionYet.Add(newElement);
            }
        }

        if (!StartElement.ApplySimpleData(data.Start))
        {
            Debug.LogWarning($"UML Tree: Could not Apply Simple Data to start element");
            return false;
        }
        newElements.Add(data.Start, StartElement);

        if (!StartElement.ApplyConnectionData(data.Start, newElements))
        {
            Debug.LogWarning($"UML Tree: Could not Connection Data to start element");
            return false;
        }

        // Try again to connect elements
        foreach (AUMLElement element in elementsWithNoConnectionYet)
        {
            UMLElementData elementData = newElements.FirstOrDefault(e => e.Value == element).Key;
            if (!element.ApplyConnectionData(elementData, newElements))
            {
                Debug.LogWarning($"UML Tree: Could not Connection Data to element {elementData.ID}");
                /*
                foreach (var item in newElements)
                {
                    Debug.LogWarning("New Element: ID -> " + item.Key.ID);
                }
                */
                //throw new Exception($"UML Tree: Could not Connection Data to element {elementData.ID}");
                return false;
            }
        }

        return true;
    }
}
