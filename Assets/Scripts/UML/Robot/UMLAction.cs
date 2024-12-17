using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EUMLActionType { DoNothing = 0, MoveUp = 11, MoveDown = 12, MoveLeft = 13, MoveRight = 14, CollectGarbage = 1, CollectBattery = 2 }

public class UMLAction : AUMLElement
{
    public EUMLActionType ActionType;
    private Action action;

    public override string Name { get { return ActionType.ToString(); } }

    public override bool Execute(UMLActor actor)
    {
        if (!base.Execute(actor))
        {
            return false;
        }

        //Debug.Log("Some Action: " + name);

        SetActionByEnum(actor);
        if (action == null)
        {
            return false;
        }
        action.Invoke();
        return true;
    }

    private void SetActionByEnum(UMLActor actor)
    {
        switch (ActionType)
        {
            case EUMLActionType.DoNothing:
                action = actor.DoNothing;
                break;
            case EUMLActionType.MoveUp:
                action = actor.MoveUp;
                break;
            case EUMLActionType.MoveDown:
                action = actor.MoveDown;
                break;
            case EUMLActionType.MoveLeft:
                action = actor.MoveLeft;
                break;
            case EUMLActionType.MoveRight:
                action = actor.MoveRight;
                break;
            case EUMLActionType.CollectGarbage:
                action = actor.CollectGarbage;
                break;
            case EUMLActionType.CollectBattery:
                action = actor.CollectBattery;
                break;
            default:
                action = () => { };
                break;
        }
    }

    protected override void SelectedValueChanged(int index)
    {
        //Debug.Log("Action Index changed: " + index);
        ActionType = Enum.GetValues(typeof(EUMLActionType)).Cast<EUMLActionType>().ElementAt(index);
    }

    protected override void SeedDropDownOptions()
    {
        if (_dropDownIsSeeded)
        {
            return;
        }
        _dropDownIsSeeded = true;
        //Debug.Log("UMLAction: SeedDropDownOptions");
        dropDown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (EUMLActionType item in Enum.GetValues(typeof(EUMLActionType)).Cast<EUMLActionType>())
        {
            options.Add(new TMP_Dropdown.OptionData(Converters.SplitCamelCase(item.ToString())));
        }
        dropDown.AddOptions(options);

    }

    public override long GetElementLongValue()
    {
        //Debug.Log("Action Value: " + (long)ActionType);
        return (long)ActionType;
    }

    protected override bool SetElementLongValue(long value)
    {
        int index = Array.IndexOf(Enum.GetValues(typeof(EUMLActionType)), (EUMLActionType)value);

        Debug.Log("Action Value -> Index: " + value + " -> " + index);

        if (index < 0)
        {
            return false;
        }

        Debug.Log("Action DropDown options count: " + dropDown.options.Count);
        dropDown.value = index;
        //SelectedValueChanged(index);

        return true;
    }
}


