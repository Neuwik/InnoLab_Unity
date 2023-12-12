using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // MOVE TUTORIAL https://www.youtube.com/watch?v=f473C43s8nE&ab_channel=Dave%2FGameDevelopment
    // DASH TUTORIAL https://www.youtube.com/watch?v=QRYGrCWumFw&t=1s&ab_channel=Dave%2FGameDevelopment (min 4)

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private float jumpInput;
    private Vector3 moveDirection;

    private MovementState movementState = MovementState.walking;
    private float speed = 70;
    private float groundDrag = 5;
    private float maxSpeed;

    private bool isDashing = false;
    private float dashSpeed = 100;
    private float dashDuration = 0.25f;
    private float dashCooldown = 1;
    private float dashCooldownTimer = 0;

    // Tutorial for sprint, crouch, etc., useless?
    public enum MovementState
    {
        walking,
        dashing,
        stunned
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        SpeedControl();

        rb.drag = groundDrag;

        if (jumpInput > 0 && moveDirection != Vector3.zero)
        {
            Dash();
        }
        if(dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        jumpInput = Input.GetAxis("Jump");
    }

    private void MovePlayer()
    {
        moveDirection = new Vector3(1, 0, 0) * horizontalInput + new Vector3(0, 0, 1) * verticalInput;
        rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (isDashing)
        {
            movementState = MovementState.dashing;
            maxSpeed = dashSpeed;
        }
        else
        {
            maxSpeed = speed;
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > maxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
        }
    }

    private void Dash()
    {
        if (dashCooldownTimer > 0)
            return;

        dashCooldownTimer = dashCooldown;

        isDashing = true;

        // Tutorial delay Force useless?
        Vector3 force = moveDirection * dashSpeed;

        rb.AddForce(force, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        isDashing = true;
    }
}
