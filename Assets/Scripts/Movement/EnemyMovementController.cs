using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MovementController
{
    private Coroutine MovementCoroutine;
    private TickManager TickManager;

    [SerializeField]
    private SpriteRenderer sprite;
    private Vector3 spriteForward = Vector3.left;

    // Movement no longer random
    // List of movement directions + loop bool
    [SerializeField]
    private bool movesRandom = true;
    [SerializeField]
    private bool loopMovement = true;
    [SerializeField]
    private List<EDirection2D> movement = new List<EDirection2D>();
    private int movementIndex = 0;

    protected new void Start()
    {
        base.Start();
        TickManager = GameManager.Instance.TickManager;
    }

    public override void Reset()
    {
        StopCoroutine(Movement());
        base.Reset();
        movementIndex = 0;
    }

    public void StartMovement()
    {
        StartCoroutine(Movement());
    }

    private IEnumerator Movement()
    {
        while (GameManager.Instance.UMLIsRunning)
        {
            yield return TickManager.WaitForEnemyTickStart();

            Vector3 direction = Vector3.zero;

            // while (movePoint.position == transform.position && GameManager.Instance.UMLIsRunning) // needed to make random enemey not get stuck
            // {
            direction = GetNextMovementDirection();

            Move(direction);
            //}

            if (direction.x != 0)
            {
                sprite.flipX = direction.x != spriteForward.x;
            }

            yield return TickManager.WaitForEnemyTickEnd();
        }
    }

    private Vector3 GetNextMovementDirection()
    {
        if (movesRandom)
        {
            Array directions = Enum.GetValues(typeof(EDirection2D));
            int randomIndex = UnityEngine.Random.Range(0, directions.Length);
            return Converters.EDirection2DToVector3((EDirection2D)directions.GetValue(randomIndex));
        }

        if (movement.Count <= 0)
            return Vector3.zero;

        if (!loopMovement && movementIndex >= movement.Count)
            return Vector3.zero;

        if (loopMovement && movementIndex >= movement.Count)
            movementIndex = 0;

        return Converters.EDirection2DToVector3(movement[movementIndex++]);
    }
}
