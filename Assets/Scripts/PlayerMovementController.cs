using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PlayerMovementController : MovementController
{
    private bool pushesAllowed;

    public new void Reset()
    {
        pushesAllowed = false;
        base.Reset();
    }

    public new void Move(Vector3 direction)
    {
        pushesAllowed = true;
        base.Move(direction);
    }

    public IEnumerator PushInDirection(Vector3 direction, int distance)
    {
        yield return WaitForMovementFinished();
        if (pushesAllowed)
        {
            movePoint.position = GetNextMovePointPosition(direction, distance);
        }
    }
    public void StopPushes()
    {
        pushesAllowed = false;
    }
}
