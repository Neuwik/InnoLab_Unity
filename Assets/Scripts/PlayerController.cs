using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IResetable
{
    public float moveSpeed = 5f;
    public int moveDistance = 3;
    public Transform movePoint;
    public LayerMask StopBefor;
    public LayerMask StopOn;

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
        return GetNextMovePointPosition(direction, moveDistance);
    }

    private Vector3 GetNextMovePointPosition(Vector3 direction, int distance)
    {
        Vector3 movePointPosition = movePoint.position;
        for (int i = 1; i <= distance; i++)
        {
            if (Physics.OverlapSphere(movePointPosition + direction, .2f, StopBefor, QueryTriggerInteraction.Collide).Length > 0)
            {
                //Debug.Log("Tree");
                return movePointPosition;
            }
            else if (Physics.OverlapSphere(movePointPosition + direction, .2f, StopOn, QueryTriggerInteraction.Collide).Length > 0)
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

    public IEnumerator PushInDirection(Vector3 direction, int distance)
    {
        yield return WaitForMovementFinished();
        movePoint.parent = null;
        movePoint.position = GetNextMovePointPosition(direction, distance);
    }
    public IEnumerator WaitForMovementFinished()
    {
        yield return new WaitUntil(() => transform.position == movePoint.position);
    }
}
