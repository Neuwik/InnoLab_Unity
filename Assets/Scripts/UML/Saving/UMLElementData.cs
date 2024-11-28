using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[Serializable]
public class UMLElementData
{
    public static Dictionary<int, Type> ElementTypes = new Dictionary<int, Type>
    {
        { 1, typeof(UMLAction) },
        { 2 , typeof(UMLCondition) },
        { 3 , typeof(UMLForLoop) },
        { 4 , typeof(UMLTreeAction) },
        { 5 , typeof(UMLTreeStart) }
    };

    public static int GetUMLTypeID(Type type)
    {
        if (ElementTypes.ContainsValue(type))
            return ElementTypes.Where(t => t.Value == type).FirstOrDefault().Key;
        return -1;
    }

    public Type UMLType
    {
        get
        {
            Type t = null;
            if (ElementTypes.TryGetValue(typeID, out t))
                return t;
            return null;
        }
    }

    public long ID;
    public int typeID;
    public string name;
    public long value;
    public long nextTID;
    public long nextFID;
    public float[] position;

    public UMLElementData(AUMLElement element)
    {
        ID = element.GetInstanceID();
        typeID = GetUMLTypeID(element.GetType());
        name = element.Name;
        value = element.GetElementLongValue();
        nextTID = element.GetNextElement(true)?.GetInstanceID() ?? 0;
        nextFID = element.GetNextElement(false)?.GetInstanceID() ?? 0;
        position = new float[3];
        position[0] = element.transform.localPosition.x;
        position[1] = element.transform.localPosition.y;
        position[2] = element.transform.localPosition.z;
    }
}
