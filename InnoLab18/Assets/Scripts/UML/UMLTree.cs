using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLTree : AUMLElement
{
    [SerializeField]
    private AUMLElement Start;
    //public List<AUMLElement> Ends;

    protected override bool Execute(UMLActor actor)
    {
        Debug.Log("Some Tree: " + name);
        NextElement = Start;
        return true;
    }

    public override bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        Start = NewNextAction;
        return true;
    }
}
