using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollectable : MonoBehaviour, IResetable
{
    public int Power = 1;
    private Vector3 originalPosition;

    public void Start()
    {
        originalPosition = transform.position;
    }

    public void Reset()
    {
        transform.position = originalPosition;
        gameObject.SetActive(true);
    }
}
