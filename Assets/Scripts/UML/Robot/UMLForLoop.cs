using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UMLForLoop : AUMLElementTrueFalse
{
    public override string Name
    {
        get 
        {
            SynchronizeMaxIndex();

            if (forCurrentIndex >= 0)
            {
                return $"For {forCurrentIndex} < {forMaxIndex}";
            }
            else
            {
                return $"For i < {forMaxIndex}";
            }
        }
    }

    public TMP_InputField Input;
    private int forMaxIndex = 3;
    private int forCurrentIndex;

    protected new void Start()
    {
        base.Start();
        forCurrentIndex = 0;
        SynchronizeMaxIndex();
        Input.interactable = true;
    }

    public override void Reset()
    {
        base.Reset();
        forCurrentIndex = 0;
        SynchronizeMaxIndex();
        Input.interactable = true;
    }

    private void SynchronizeMaxIndex()
    {
        if (string.IsNullOrEmpty(Input.text))
        {
            Input.text = forMaxIndex.ToString();
            return;
        }
        int newMaxIndex = int.Parse(Input.text);
        if (newMaxIndex <= 0)
        {
            Input.text = forMaxIndex.ToString();
            return;
        }
        forMaxIndex = newMaxIndex;
    }

    public override bool Execute(UMLActor actor)
    {
        if (Input.interactable)
        {
            Input.interactable = false;
            SynchronizeMaxIndex();
        }

        if (forCurrentIndex > forMaxIndex) // When current == max then the for loop just ended, but when current > max then the for loop was started a second time
        {
            forCurrentIndex = 0;
        }

        if (!base.Execute(actor))
        {
            return false;
        }

        if (forCurrentIndex < forMaxIndex)
        {
            NextElement = trueNextAction;
        }
        else
        {
            NextElement = falseNextAction;
        }
        forCurrentIndex++;
        return true;
    }
}
