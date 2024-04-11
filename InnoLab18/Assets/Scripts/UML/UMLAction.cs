using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UMLAction : AUMLElement
{
    public UnityEvent<UMLActor> Action;

    protected override bool Execute(UMLActor actor)
    {
        Debug.Log("Some Action: " + name);
        if (Action == null)
        {
            return false;
        }

        Action.Invoke(actor);
        return true;
    }
}
