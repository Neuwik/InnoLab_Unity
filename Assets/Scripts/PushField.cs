using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPushDirection { Up = 0, Down = 1, Left = 2, Right = 3 };
public class PushField : MonoBehaviour
{
    public EPushDirection Direction;
    private Vector3 direction;
    [Min(1)]
    public int Power = 1;

    private void Start()
    {
        switch (Direction)
        {
            case EPushDirection.Up:
                direction = Vector3.forward;
                break;
            case EPushDirection.Down:
                direction = Vector3.back;
                break;
            case EPushDirection.Left:
                direction = Vector3.left;
                break;
            case EPushDirection.Right:
                direction = Vector3.right;
                break;
            default:
                direction = Vector3.zero;
                break;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            StartCoroutine(player.PushInDirection(direction, Power));
        }
    }
}
