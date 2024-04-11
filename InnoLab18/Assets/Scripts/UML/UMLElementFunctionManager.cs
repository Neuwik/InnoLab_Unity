using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class UMLElementFunctionManager : MonoBehaviour
{
    private static UMLElementFunctionManager _instance;
    public static UMLElementFunctionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UML Element Function Manager is NULL");
            }

            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    public void DoNothing(UMLActor actor)
    {
        actor.DoNothing();
    }

    public void DoSomething(UMLActor actor)
    {
        actor.DoSomething();
    }

    public void DoSomethingElse(UMLActor actor)
    {
        actor.DoSomethingElse();
    }

    public bool SomeCondition(UMLActor actor)
    {
        return actor != null;
    }

    public bool RandomCondition(UMLActor actor)
    {
        return Random.value > 0.5;
    }
}
