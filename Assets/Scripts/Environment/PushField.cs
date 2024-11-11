using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EPushDirection { Up = 0, Down = 1, Left = 2, Right = 3 };
public class PushField : MonoBehaviour
{
    public EPushDirection Direction;
    private Vector3 direction;
    [Min(1)]
    public int Power = 1;

    public TextMeshProUGUI Text;
    public SpriteRenderer SpriteUp;
    public SpriteRenderer SpriteDown;
    public SpriteRenderer SpriteLeft;
    public SpriteRenderer SpriteRight;

    private void Start()
    {
        SpriteUp.gameObject.SetActive(false);
        SpriteDown.gameObject.SetActive(false);
        SpriteLeft.gameObject.SetActive(false);
        SpriteRight.gameObject.SetActive(false);

        switch (Direction)
        {
            case EPushDirection.Up:
                direction = Vector3.forward;
                SpriteUp.gameObject.SetActive(true);
                break;
            case EPushDirection.Down:
                direction = Vector3.back;
                SpriteDown.gameObject.SetActive(true);
                break;
            case EPushDirection.Left:
                direction = Vector3.left;
                SpriteLeft.gameObject.SetActive(true);
                break;
            case EPushDirection.Right:
                direction = Vector3.right;
                SpriteRight.gameObject.SetActive(true);
                break;
            default:
                direction = Vector3.zero;
                break;
        }
        Text.text = $"{Power}";
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
