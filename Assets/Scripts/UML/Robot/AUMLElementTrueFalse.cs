using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUMLElementTrueFalse : AUMLElement
{
    [SerializeField]
    protected AUMLElement trueNextAction;
    [SerializeField]
    protected AUMLElement falseNextAction;

    public override bool ChangeNextAction(AUMLElement NewNextAction, bool condition)
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
}
