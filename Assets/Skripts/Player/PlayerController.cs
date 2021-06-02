using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class PlayerController : KinematicObject
{

    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;
    public JumpState jumpState = JumpState.Grounded;
    private bool stopJump;
    private float jumpAccelerationSpeed;
    public Collider2D collider2d;
    public Health health;
    public bool controlEnabled = true;

    bool jump;
    Vector2 movement;

    //SpriteRenderer spriteRenderer;
    //internal Animator animator;
    readonly PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public Bounds Bounds => collider2d.bounds;

    void Awake()
    {
        health = GetComponent<Health>();
        collider2d = GetComponent<Collider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (Input.acceleration.z >= 0.15)
        {
            Debug.Log(Input.acceleration.z);
        }

        if (controlEnabled)
        {
            if (Input.acceleration.x >= 0.03 || Input.acceleration.x <= -0.03)
            {
                movement.x = Input.acceleration.x * 1.5f;
            }
            else
            {
                movement.x = 0;
            }

            if (jumpState == JumpState.Grounded && Input.acceleration.z >= 0.15)
            {
                jumpState = JumpState.PrepareToJump;
                jumpAccelerationSpeed = Input.acceleration.z;
            }
            else if (Input.acceleration.z >= 0.15)
            {
                jumpAccelerationSpeed = Input.acceleration.z;
                //  stopJump = true;
                //  Schedule<PlayerStopJump>().player = this;
            }
        }
        else
        {
            movement.x = 0;
        }

        UpdateJumpState();
        base.Update();
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                stopJump = false;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    //   Schedule<PlayerJumped>().player = this;
                    jumpState = JumpState.InFlight;
                }

                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    //   Schedule<PlayerLanded>().player = this;
                    jumpState = JumpState.Landed;
                }

                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                jumpAccelerationSpeed = 0;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed * model.jumpModifier * (1 + jumpAccelerationSpeed);
            jump = false;
        }
        else if (stopJump)
        {
            stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * model.jumpDeceleration * (1 + jumpAccelerationSpeed);
            }
        }

        if (movement.x > 0.01f)
        {
            // spriteRenderer.flipX = false;
        }
        else if (movement.x < -0.01f)
        {
            //  spriteRenderer.flipX = true;
        }


        //    animator.SetBool("grounded", IsGrounded);
        //    animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = movement * maxSpeed;
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
}