using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AUMLElementTrueFalse : AUMLElement
{
    [SerializeField]
    protected AUMLElement trueNextAction;
    [SerializeField]
    protected AUMLElement falseNextAction;

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
}
