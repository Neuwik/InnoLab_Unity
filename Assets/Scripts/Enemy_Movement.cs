using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour, IResetable
{
    public float moveSpeed = 1f;
    private Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    private Vector3 startPosition;
    private IEnumerator MovementEnumerator;
    private TickManager TickManager;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        TickManager = GameManager.Instance.TickManager;
    }

    public void Reset()
    {
        StopCoroutine(MovementEnumerator);
        transform.position = startPosition;
    }

    public void StartMovement()
    {
        StartCoroutine(MovementEnumerator = Movement());
    }

    private IEnumerator Movement()
    {
        while (GameManager.Instance.UMLIsRunning)
        {
            yield return TickManager.WaitForPlayerTickEnd();
            int randomIndex = Random.Range(0, directions.Length);
            Vector3 randomDirection = directions[randomIndex];
            Move(randomDirection);
            yield return TickManager.WaitForPlayerTickStart();
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * moveSpeed;
    }

    public IEnumerator WaitForMovementFinished()
    {
        // yield return wait until Enemy done moveing
        yield return new WaitForSeconds(0.1f);
    }
}
