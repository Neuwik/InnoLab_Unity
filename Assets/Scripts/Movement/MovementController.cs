using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IResetable
{
    public Transform movePoint;
    public float moveSpeed = 5f;
    public int moveDistance = 3;
    private Vector3 startPosition;

    public LayerMask StopBefor;
    public LayerMask StopOn;

    protected void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (transform.position != movePoint.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        }
    }

    public void Reset()
    {
        transform.position = startPosition;
        movePoint.parent = transform;
        movePoint.localPosition = Vector3.zero;
    }

    public void Move(Vector3 direction)
    {
        movePoint.position = GetNextMovePointPosition(direction);
    }

    public Vector3 GetNextMovePointPosition(Vector3 direction)
    {
        return GetNextMovePointPosition(direction, moveDistance);
    }

    protected Vector3 GetNextMovePointPosition(Vector3 direction, int distance)
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

    public IEnumerator WaitForMovementFinished()
    {
        yield return new WaitUntil(() => transform.position == movePoint.position);
    }
}
