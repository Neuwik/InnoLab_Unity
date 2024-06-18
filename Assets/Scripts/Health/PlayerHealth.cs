using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health, ILooseCondition, IResetable
{
    [SerializeField]
    private Image[] health;
    public Sprite One;
    public Sprite Zero;

    private int maxHPBinaryLength;
    public int tmpHealth;

    public Action OnLoose { get; set; }

    private void Start()
    {
        currentHealth = maxHealth;
        maxHPBinaryLength = Convert.ToString(currentHealth, 2).Length;
        UpdateHealthUIBinary();
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        maxHPBinaryLength = Convert.ToString(currentHealth, 2).Length;
        UpdateHealthUIBinary();
    }

    public override void TakeDamage(int amount)
    {
        GameManager.Instance.Console.LogWarning("Damaging", name, $"Has taken {amount} Damage");
        if (tmpHealth > 0)
        {
            tmpHealth -= amount;
            if (tmpHealth < 0)
            {
                currentHealth += tmpHealth;
                tmpHealth = 0;
            }
        }

        else
        {
            base.TakeDamage(amount);
        }

        if (currentHealth <= 0)
        {
            OnLoose?.Invoke();
        }

        UpdateHealthUIBinary();
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
        UpdateHealthUIBinary();
    }

    //Testing
    /*public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "HealOrb")
            Heal(1);
        else if (collision.gameObject.tag == "DamageOrb")
            TakeDamage(1);
        else if (collision.gameObject.tag == "ArmoreOrb")
            GetShield(1);
    }*/

    private void OnTriggerEnter(Collider collision)
    {
        //UnityEngine.Debug.Log("Trigger detected with: " + collision.gameObject.name);

        DamageScript ds = collision.gameObject.GetComponent<DamageScript>();

        if (ds != null)
        {
            if (ds.instantDeath)
            {
                TakeDamage(currentHealth);
                AudioManager.instance.PlayWaterSplashSound();
                GetComponent<VFXController>()?.PlayWAterSplash();
            }

            else
            {
                TakeDamage(ds.damage);
                AudioManager.instance.PlayFireDamageSound();
            }

            //UnityEngine.Debug.Log("Current Health: " + currentHealth);
        }
    }

    private void UpdateHealthUIBinary()
    {
        string binaryStr = Convert.ToString(currentHealth, 2);
        while (binaryStr.Length < maxHPBinaryLength)
        {
            binaryStr = '0' + binaryStr;
        }

        char[] binary = binaryStr.ToCharArray();
        for (int i = 0; i < health.Length; i++)
        {
            if (i >= maxHPBinaryLength)
            {
                health[i].enabled = false;
                continue;
            }

            if (binary[i] == '1')
            {
                health[i].sprite = One;
            }
            else
            {
                health[i].sprite = Zero;
            }
        }
    }
}
