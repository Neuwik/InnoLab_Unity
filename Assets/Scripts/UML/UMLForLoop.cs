using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UMLForLoop : AUMLElement, IResetable
{
    [SerializeField]
    private AUMLElement trueNextAction;
    [SerializeField]
    private AUMLElement falseNextAction;

    public TMP_InputField input;
    private int ForMaxIndex = 3;
    private int ForCurrentIndex;

    protected new void Start()
    {
        ForCurrentIndex = 0;
        SynchronizeMaxIndex();
        input.interactable = true;
        base.Start();
    }

    public void Reset()
    {
        ForCurrentIndex = 0;
        SynchronizeMaxIndex();
        input.interactable = true;
    }

    private void SynchronizeMaxIndex()
    {
        if (string.IsNullOrEmpty(input.text))
        {
            input.text = ForMaxIndex.ToString();
            return;
        }
        int newMaxIndex = int.Parse(input.text);
        if (newMaxIndex <= 0)
        {
            input.text = ForMaxIndex.ToString();
            return;
        }
        ForMaxIndex = newMaxIndex;
    }

    protected override bool Execute(UMLActor actor)
    {
        if (input.interactable)
        {
            input.interactable = false;
            SynchronizeMaxIndex();
        }

        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing ForLoop ({ForCurrentIndex}<{ForMaxIndex})");
        
        if (ForCurrentIndex < ForMaxIndex)
        {
            ForCurrentIndex++;
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
}
