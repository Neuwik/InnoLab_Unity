using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class UMLTreeData
{
    public long ID;
    public string TreeName;
    public UMLElementData Start;

    public UMLElementData[] elements;

    public UMLTreeData(UMLTree tree)
    {
        ID = tree.ID;
        TreeName = tree.TreeName;
        Start = new UMLElementData(tree.StartElement);
        Dictionary<long, UMLElementData> elementDict = GetBranchData(tree.StartElement.NextElement, new Dictionary<long, UMLElementData>());
        elements = elementDict.Values.ToArray();
    }

    private Dictionary<long, UMLElementData> GetBranchData(AUMLElement element, Dictionary<long, UMLElementData> elementDict)
    {
        if (element == null)
        {
            return elementDict;
        }

        UMLElementData elementData = new UMLElementData(element);

        if (elementDict.ContainsKey(elementData.ID))
        {
            return elementDict;
        }

        elementDict.Add(elementData.ID, elementData);

        if (element.GetNextElement(false) != null)
        {
            elementDict = GetBranchData(element.GetNextElement(false), elementDict);
        }

        if (element.GetNextElement(true) != null)
        {
            elementDict = GetBranchData(element.GetNextElement(true), elementDict);
        }

        return elementDict;
    }

    public bool IsValid()
    {
        return elements.Length > 0;
    }
}
