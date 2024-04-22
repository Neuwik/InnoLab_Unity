using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public enum EUMLConditionType { SomeCondition = 0, RandomCondition }
public class UMLCondition : AUMLElement
{
    [SerializeField]
    private AUMLElement trueNextAction;
    [SerializeField]
    private AUMLElement falseNextAction;

    public EUMLConditionType ConditionType;
    private Func<bool> condition;

    protected override bool Execute(UMLActor actor)
    {
        Debug.Log("Some Condition: " + name);
        SetConditionByEnum(actor);
        if (condition.Invoke())
        {
            NextElement = trueNextAction;
        }
        else
        {
            NextElement = falseNextAction;
        }
        return true;
    }

    public override bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        if (conditional)
        {
            falseNextAction = NewNextAction;
        }
        else
        {
            trueNextAction = NewNextAction;
        }
        return true;
    }

    private void SetConditionByEnum(UMLActor actor)
    {
        switch (ConditionType)
        {
            case EUMLConditionType.SomeCondition:
                condition = actor.SomeCondition;
                break;
            case EUMLConditionType.RandomCondition:
                condition = actor.RandomCondition;
                break;
            default:
                condition = () => { return true; };
                break;
        }
    }
}
