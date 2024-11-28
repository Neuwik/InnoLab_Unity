using System;


[Serializable]
public class UMLElementTypePrefab
{
    public Type type
    {
        get
        {
            return prefab.GetType();
        }
    }
    public AUMLElement prefab;
}
