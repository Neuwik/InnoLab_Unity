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
        Dictionary<long, UMLElementData> elementDcit = GetBranchData(tree.StartElement.NextElement, new Dictionary<long, UMLElementData>());
        elements = elementDcit.Values.ToArray();
    }

    private Dictionary<long, UMLElementData> GetBranchData(AUMLElement element, Dictionary<long, UMLElementData> elementDcit)
    {
        if (element == null)
        {
            return elementDcit;
        }

        UMLElementData elementData = new UMLElementData(element);

        if (elementDcit.ContainsKey(elementData.ID))
        {
            return elementDcit;
        }

        elementDcit.Add(elementData.ID, elementData);

        if (element.GetNextElement(false) != null)
        {
            elementDcit = GetBranchData(element.GetNextElement(false), elementDcit);
        }

        if (element.GetNextElement(true) != null)
        {
            elementDcit = GetBranchData(element.GetNextElement(true), elementDcit);
        }

        return elementDcit;
    }

    public bool IsValid()
    {
        return elements.Length > 0;
    }
}
