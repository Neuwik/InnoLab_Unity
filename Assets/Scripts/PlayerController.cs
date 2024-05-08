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
    private bool pushesAllowed;

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
        pushesAllowed = false;
        movePoint.parent = transform;
        movePoint.localPosition = Vector3.zero;
    }

    public void Move(Vector3 direction)
    {
        pushesAllowed = true;
        movePoint.position = GetNextMovePointPosition(direction);
    }

    public Vector3 GetNextMovePointPosition(Vector3 direction)
    {
        return GetNextMovePointPosition(direction, moveDistance);
    }

    private Vector3 GetNextMovePointPosition(Vector3 direction, int distance)
    {
        movePoint.parent = null;
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
        if (pushesAllowed)
        {
            movePoint.position = GetNextMovePointPosition(direction, distance);
        }
    }
    public IEnumerator WaitForMovementFinished()
    {
        yield return new WaitUntil(() => transform.position == movePoint.position);
    }
    public void StopPushes()
    {
        pushesAllowed = false;
    }
}
