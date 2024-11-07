using TMPro;
using UnityEngine;

public class UMLForLoop : AUMLElement, IResetable
{
    public override string Name
    {
        get 
        {
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

    [SerializeField]
    private AUMLElement trueNextAction;
    [SerializeField]
    private AUMLElement falseNextAction;

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

    public void Reset()
    {
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

        GameManager.Instance.Console.Log(actor.State.ToString(), actor.name, $"Is executing {Name}");
        
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
