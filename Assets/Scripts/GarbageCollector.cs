using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollector : MonoBehaviour, IResetable
{
    public int GarbageCount = 0;
    private GameObject garbage;
    private float cooldown = 1;
    private float cooldownTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        GarbageCount = 0;
        cooldownTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            if (Input.GetAxis("Jump") > 0)
            {
                CollectGarbage();
                cooldownTimer = cooldown;
            }
        }
    }

    public void Reset()
    {
        GarbageCount = 0;
        cooldownTimer = 0;
    }

    public void CollectGarbage()
    {
        if (garbage == null)
        {
            GameManager.Instance.Console.LogWarning("Collecting", name, $"Can not collect Garbage");
            //Debug.LogError("Player not on Garbage");
        }
        else
        {
            GameManager.Instance.Console.Log("Collecting", name, $"Has collected Garbage");
            AudioManager.instance.PlayerGarbageCollectSound();
            garbage.SetActive(false);
            GarbageCount++;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Trigger detected with: " + collision.gameObject.name);

        if (collision.gameObject.tag == "Garbage")
        {
            garbage = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Garbage")
        {
            garbage = null;
        }
    }
}
