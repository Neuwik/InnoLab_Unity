using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MovementController
{
    private Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    private Coroutine MovementCoroutine;
    private TickManager TickManager;

    [SerializeField]
    private SpriteRenderer sprite;
    private Vector3 spriteForward = Vector3.left;

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        TickManager = GameManager.Instance.TickManager;
    }

    public new void Reset()
    {
        StopCoroutine(Movement());
        base.Reset();
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

            Vector3 randomDirection = Vector3.zero;

            while (movePoint.position == transform.position && GameManager.Instance.UMLIsRunning)
            {
                int randomIndex = Random.Range(0, directions.Length);
                randomDirection = directions[randomIndex];

                Move(randomDirection);
            }

            if (randomDirection.x != 0)
            {
                sprite.flipX = randomDirection.x != spriteForward.x;
            }

            yield return TickManager.WaitForEnemyTickEnd();
        }
    }
}
