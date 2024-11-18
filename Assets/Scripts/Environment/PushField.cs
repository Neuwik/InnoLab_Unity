using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PushField : MonoBehaviour
{
    public EDirection2D Direction;
    private Vector3 directionV3;
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

        directionV3 = Converters.EDirection2DToVector3(Direction);

        switch (Direction)
        {
            case EDirection2D.Up:
                SpriteUp.gameObject.SetActive(true);
                break;
            case EDirection2D.Down:
                SpriteDown.gameObject.SetActive(true);
                break;
            case EDirection2D.Left:
                SpriteLeft.gameObject.SetActive(true);
                break;
            case EDirection2D.Right:
                SpriteRight.gameObject.SetActive(true);
                break;
        }
        Text.text = $"{Power}";
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerMovementController player = other.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            StartCoroutine(player.PushInDirection(directionV3, Power));
        }
    }
}
