using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EUMLActionType { DoNothing = 0, DoSomething, DoSomethingElse, MoveUp }

public class UMLAction : AUMLElement
{
    public EUMLActionType ActionType;
    private Action action;

    protected override bool Execute(UMLActor actor)
    {
        Debug.Log("Some Action: " + name);
        SetActionByEnum(actor);
        if (action == null)
        {
            return false;
        }

        action.Invoke();
        return true;
    }

    private void SetActionByEnum(UMLActor actor)
    {
        switch (ActionType)
        {
            case EUMLActionType.DoNothing:
                action = actor.DoNothing;
                break;
            case EUMLActionType.DoSomething:
                action = actor.DoSomething;
                break;
            case EUMLActionType.DoSomethingElse:
                action = actor.DoSomethingElse;
                break;
            case EUMLActionType.MoveUp:
                action = actor.MoveUp;
                break;
            default:
                action = () => {  };
                break;
        }
    }
}


