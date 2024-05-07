using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IResetable
{
    public float moveSpeed = 5f;
    public int distance = 1;
    public Transform movePoint;
    public LayerMask obstical;
    public LayerMask damagesource;

    void Update()
    {
        if (transform.position != movePoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(-Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-Vector3.right);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector3.right);
    }

    public void Reset()
    {
        movePoint.parent = transform;
        movePoint.localPosition = Vector3.zero;
    }

    public void Move(Vector3 direction)
    {
        movePoint.parent = null;
        movePoint.position = GetNextMovePointPosition(direction);
        /*
        for (int i = 1; i <= distance; i++)
        {
            if (Physics.OverlapSphere(movePoint.position + direction, .2f, obstical, QueryTriggerInteraction.Collide).Length > 0)
            {
                Debug.Log("Tree");
                return;
            }
            else if (Physics.OverlapSphere(movePoint.position + direction, .2f, damagesource, QueryTriggerInteraction.Collide).Length > 0)
            {
                Debug.Log("Damage");
                movePoint.position += direction;
                return;
            }
            else
            {
                movePoint.position += direction;
            }
        }*/
        /*
        */
        //movePoint.position += direction * distance;
    }

    public Vector3 GetNextMovePointPosition(Vector3 direction)
    {
        Vector3 movePointPosition = movePoint.position;
        for (int i = 1; i <= distance; i++)
        {
            if (Physics.OverlapSphere(movePointPosition + direction, .2f, obstical, QueryTriggerInteraction.Collide).Length > 0)
            {
                //Debug.Log("Tree");
                return movePointPosition;
            }
            else if (Physics.OverlapSphere(movePointPosition + direction, .2f, damagesource, QueryTriggerInteraction.Collide).Length > 0)
            {
                //Debug.Log("Damage");
                return movePointPosition + direction;
            }
            else
            {
                movePointPosition += direction;
            }
        }
        return movePointPosition;
    }
}
