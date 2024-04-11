using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class AUMLElement : MonoBehaviour
{
    [SerializeField]
    protected AUMLElement NextElement;

    public virtual bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        NextElement = NewNextAction;
        return true;
    }

    public bool Run(UMLActor actor)
    {
        if (!Execute(actor))
        {
            return false;
        }

        return NextElement?.Run(actor) ?? false;
    }

    protected virtual bool Execute(UMLActor actor)
    {
        Debug.Log("Some Element: " + name);
        return true;
    }
}
