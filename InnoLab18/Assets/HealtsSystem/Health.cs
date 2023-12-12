using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    public int tmpHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(int amount)
    {
        if(tmpHealth > 0)
        {
            tmpHealth -= amount;
            if(tmpHealth < 0) 
            {
                currentHealth += tmpHealth;
                tmpHealth = 0;
            }
        }

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            //Death animation?
            //Game Over
        }
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    void GetShield(int amount)
    {
        tmpHealth += amount;
    }
}
