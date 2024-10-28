using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        get { return _treeName; }
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
}
