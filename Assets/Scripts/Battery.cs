using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BatteryBackgroundBreakpoint
{
    [Range(0f, 1f)]
    public float Percent;
    public GameObject Background;
}

public class Battery : MonoBehaviour, ILooseCondition, IResetable
{
    [SerializeField]
    private List<BatteryBackgroundBreakpoint> BackgroundBreakpoints;
    private BatteryBackgroundBreakpoint CurrentBackground;

    [SerializeField]
    private TMP_Text Text;

    [SerializeField]
    private int MaxEnergy = 10;
    public int CurrentEnergy { get; private set; }
    public float PercentEnergy { get { return Mathf.Round((float)CurrentEnergy / MaxEnergy * 100.0f) / 100.0f; } }

    public Action OnLoose { get; set; }

    private void Awake()
    {
        ResetEnergy();
        if (Text == null)
        {
            Text = GetComponentInChildren<TMP_Text>();
        }
    }

    public void Reset()
    {
        ResetEnergy();
    }

    private void ResetEnergy()
    {
        CurrentEnergy = MaxEnergy;
        Text.text = $"{CurrentEnergy} ({PercentEnergy * 100}%)";
        foreach (BatteryBackgroundBreakpoint entry in BackgroundBreakpoints)
        {
            entry.Background.SetActive(false);
            if (entry.Percent <= PercentEnergy && (CurrentBackground == null || CurrentBackground.Percent < entry.Percent))
            {
                CurrentBackground = entry;
            }
        }
        CurrentBackground.Background.SetActive(true);
    }

    public void LooseEnergy(int amount = 1)
    {
        CurrentEnergy -= amount;
        if (CurrentEnergy < 0)
        {
            CurrentEnergy = 0;
        }
        GameManager.Instance.Console.Log("Discharging", name, $"Has lost {amount} Energy");

        Text.text = $"{CurrentEnergy} ({PercentEnergy * 100}%)";
        CurrentBackground.Background?.SetActive(false);
        CurrentBackground = BackgroundBreakpoints.Where(b => b.Percent <= PercentEnergy).OrderByDescending(b => b.Percent).FirstOrDefault();
        CurrentBackground.Background?.SetActive(true);

        if (CurrentEnergy <= 0)
        {
            OnLoose?.Invoke();
        }
    }

    public void GainEnergy(int amount = 1)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > MaxEnergy)
        {
            CurrentEnergy = MaxEnergy;
        }
        GameManager.Instance.Console.Log("Charging", name, $"Has gained {amount} Energy");

        Text.text = $"{CurrentEnergy} ({PercentEnergy * 100}%)";
        CurrentBackground.Background?.SetActive(false);
        CurrentBackground = BackgroundBreakpoints.Where(b => b.Percent <= PercentEnergy).OrderByDescending(b => b.Percent).FirstOrDefault();
        CurrentBackground.Background?.SetActive(true);
    }
}
