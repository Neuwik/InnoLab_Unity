using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

public enum EUMLConditionType
{
    SomeCondition = 0,
    IsUpDanger = 11, IsDownDanger = 12, IsLeftDanger = 13, IsRightDanger = 14, IsThisDanger = 15,
    IsUpGarbage = 21, IsDownGarbage = 22, IsLeftGarbage = 23, IsRightGarbage = 24, IsThisGarbage = 25
}

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
        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing {ConditionType}");
        //Debug.Log("Some Condition: " + name);
        SetConditionByEnum(actor);
        actor.GetComponent<Battery>()?.LooseEnergy(1);
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

    public override bool ChangeNextAction(AUMLElement NewNextAction, bool conditional)
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
    public void SwitchNextActions()
    {
        (trueNextAction, falseNextAction) = (falseNextAction, trueNextAction);
    }

    private void SetConditionByEnum(UMLActor actor)
    {
        switch (ConditionType)
        {
            case EUMLConditionType.SomeCondition:
                condition = actor.SomeCondition;
                break;
            case EUMLConditionType.IsUpDanger:
                condition = actor.IsUpDanger;
                break;
            case EUMLConditionType.IsDownDanger:
                condition = actor.IsDownDanger;
                break;
            case EUMLConditionType.IsLeftDanger:
                condition = actor.IsLeftDanger;
                break;
            case EUMLConditionType.IsRightDanger:
                condition = actor.IsRightDanger;
                break;
            case EUMLConditionType.IsThisDanger:
                condition = actor.IsThisDanger;
                break;
            case EUMLConditionType.IsUpGarbage:
                condition = actor.IsUpGarbage;
                break;
            case EUMLConditionType.IsDownGarbage:
                condition = actor.IsDownGarbage;
                break;
            case EUMLConditionType.IsLeftGarbage:
                condition = actor.IsLeftGarbage;
                break;
            case EUMLConditionType.IsRightGarbage:
                condition = actor.IsRightGarbage;
                break;
            case EUMLConditionType.IsThisGarbage:
                condition = actor.IsThisGarbage;
                break;
            default:
                condition = () => { return true; };
                break;
        }
    }
}
