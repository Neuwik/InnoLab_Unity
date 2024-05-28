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

    private BatteryCollectable collectable;

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
        collectable = null;
        ResetEnergy();
    }

    private void ResetEnergy()
    {
        CurrentEnergy = MaxEnergy;
        UpdateUIText();
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

        UpdateUI();

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

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateUIText();
        CurrentBackground.Background?.SetActive(false);
        CurrentBackground = BackgroundBreakpoints.Where(b => b.Percent <= PercentEnergy).OrderByDescending(b => b.Percent).FirstOrDefault();
        CurrentBackground.Background?.SetActive(true);
    }
    private void UpdateUIText()
    {
        Text.text = $"{CurrentEnergy}";
        //Text.text = $"{CurrentEnergy} ({PercentEnergy * 100}%)";
    }

    public void CollectBattery()
    {
        if (collectable == null)
        {
            GameManager.Instance.Console.LogWarning("Collecting", name, $"Can not collect Battery");
        }
        else
        {
            GameManager.Instance.Console.Log("Collecting", name, $"Has collected Battery");
            AudioManager.instance.PlayerGarbageCollectSound();
            GainEnergy(collectable.Power);
            collectable.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<BatteryCollectable>(out BatteryCollectable c))
        {
            collectable = c;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<BatteryCollectable>(out BatteryCollectable c))
        {
            if (collectable == c)
            {
                collectable = null;
            }
        }
    }
}
