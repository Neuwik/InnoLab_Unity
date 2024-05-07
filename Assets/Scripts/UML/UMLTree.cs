using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UMLTree : AUMLElement
{
    [SerializeField]
    private AUMLElement StartElement;
    //public List<AUMLElement> Ends;

    protected override bool Execute(UMLActor actor)
    {
        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing {name}");
        //Debug.Log("Some Tree: " + name);
        NextElement = StartElement;
        return true;
    }

    public override bool ChangeNextAction(AUMLElement NewNextAction, bool conditional = false)
    {
        StartElement = NewNextAction;
        return true;
    }
}
