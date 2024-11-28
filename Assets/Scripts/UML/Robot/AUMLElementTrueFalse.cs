using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUMLElementTrueFalse : AUMLElement
{
    [SerializeField]
    protected AUMLElement trueNextAction;
    [SerializeField]
    protected AUMLElement falseNextAction;

    public override bool ChangeNextElement(AUMLElement NewNextAction, bool condition)
    {
        if (condition)
        {
            trueNextAction = NewNextAction;
        }
        else
        {
            falseNextAction = NewNextAction;
        }
        return true;
    }

    public void SwitchNextActions()
    {
        (trueNextAction, falseNextAction) = (falseNextAction, trueNextAction);
    }

    public override AUMLElement GetNextElement(bool condition = true)
    {
        return condition ? trueNextAction : falseNextAction;
    }
}
