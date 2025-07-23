using TMPro;
 using UnityEngine.UI;
using UnityEngine;

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
    
    public Slider Input;
    public TextMeshProUGUI LoopText;

    
    [SerializeField]
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

    public void SynchronizeMaxIndex()
    {
        if (Input.value < 0)
        {
            OverrideInputWithCurrentMaxIndex();
            return;
        }
        int newMaxIndex = (int) Input.value;
        if (newMaxIndex <= 0)
        {
            OverrideInputWithCurrentMaxIndex();
            return;
        }
        forMaxIndex = newMaxIndex;
        SetLoopText();
    }

    private void OverrideInputWithCurrentMaxIndex()
    {
        Input.value = forMaxIndex;
        SetLoopText();
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
        SetLoopText();
        forCurrentIndex++;
        return true;
    }

    public override long GetElementLongValue()
    {
        return (long)forMaxIndex;
    }

    protected override bool SetElementLongValue(long value)
    {
        forMaxIndex = (int)value;
        if (forMaxIndex <= 0)
        {
            forMaxIndex = 3;
        }
        OverrideInputWithCurrentMaxIndex();

        return true;
    }
    private void SetLoopText() 
    {
        int displayIndex = forMaxIndex - forCurrentIndex;
        LoopText.SetText( displayIndex == 1 ? $"Repeat {displayIndex} time" : $"Repeat {displayIndex} times");
    }
}
