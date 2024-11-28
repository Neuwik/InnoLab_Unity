using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

    private string _treeName;
    public string TreeName
    {
        get
        {
            if ((String.IsNullOrEmpty(_treeName)))
            {
                TreeName = "Tree";
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
                return false;
            }

            newElements.Add(elementData, newElement);

            if (!newElement.ApplyConnectionData(elementData, newElements))
            {
                elementsWithNoConnectionYet.Add(newElement);
            }
        }

        if (!StartElement.ApplySimpleData(data.Start))
        {
            return false;
        }

        if (!StartElement.ApplyConnectionData(data.Start, newElements))
        {
            return false;
        }

        // Try again to connect elements
        foreach (AUMLElement element in elementsWithNoConnectionYet)
        {
            if (!element.ApplyConnectionData(newElements.FirstOrDefault(e => e.Value == element).Key, newElements))
            {
                return false;
            }
        }

        return true;
    }
}
