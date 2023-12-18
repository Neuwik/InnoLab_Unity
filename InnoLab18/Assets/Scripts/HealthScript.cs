using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public int tmpHealth;

    public Image[] hearts;
    public Sprite fullHearts;
    public Sprite emptyHearts;
    public Sprite ArmoredHeart;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++) {

            if(i < tmpHealth) {
                hearts[i].sprite = ArmoredHeart;
            } else if(i < currentHealth) {
                hearts[i].sprite = fullHearts;
            } else {
                hearts[i].sprite = emptyHearts;
            }

            if(i < maxHealth) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }

    void TakeDamage(int amount)
    {
        if (tmpHealth > 0) {
            tmpHealth -= amount;
            if (tmpHealth < 0) { 
                currentHealth += tmpHealth; 
                tmpHealth = 0;
            }
        } else { 
            currentHealth -= amount;
        }

        if (currentHealth <= 0) {
            currentHealth = 0;
            // Death animation?
            // Game Over
        }
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void GetShield(int amount)
    {
        tmpHealth += amount;
        if (tmpHealth >= maxHealth)
        {
            tmpHealth = maxHealth;
        }
    }

    //Testing
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "HealOrb")
            Heal(1);
        else if (collision.gameObject.tag == "DamageOrb")
            TakeDamage(1);
        else if (collision.gameObject.tag == "ArmoreOrb")
            GetShield(1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Trigger detected with: " + collision.gameObject.name);
        

        if (collision.gameObject.tag == "HealOrb")
        {
            Heal(1);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.tag == "DamageOrb")
            TakeDamage(1);

        else if (collision.gameObject.tag == "ArmoreOrb")
        {
            GetShield(1);
            collision.gameObject.SetActive(false);
        }
    }
}
