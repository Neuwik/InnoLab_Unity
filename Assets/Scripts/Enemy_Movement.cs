using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float moveSpeed = 1f;
    private float nextMoveTime = 0f; 
    private Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right }; 

    // Start is called before the first frame update
    void Start()
    {
        nextMoveTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextMoveTime)
        {
            int randomIndex = Random.Range(0, directions.Length);
            Vector3 randomDirection = directions[randomIndex];

            Move(randomDirection);

            nextMoveTime = Time.time + 2f;
        }
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction * moveSpeed;
    }
}
