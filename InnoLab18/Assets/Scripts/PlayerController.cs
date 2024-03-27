using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask obstical;
    public LayerMask damagesource;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Move(Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Move(-Vector3.forward);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(-Vector3.right);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector3.right);
    }

    void Move(Vector3 direction)
    {   
        if (!Physics2D.OverlapCircle(movePoint.position + direction, .2f, obstical))
        {
            movePoint.position += direction;
        }
        else if(!Physics2D.OverlapCircle(movePoint.position + direction, .2f, damagesource))
        {
           //Damage
        }
    }
}
