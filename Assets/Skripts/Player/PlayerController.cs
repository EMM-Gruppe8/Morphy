using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class PlayerController : KinematicObject
{
    public float maxSpeed = 8;
    public float jumpTakeOffSpeed = 7;
    public JumpState jumpState = JumpState.Grounded;
    public MovementState movementState = MovementState.Standing;
    public Collider2D collider2d;
    public Health health;
    public bool controlEnabled = true;

    private bool _stopJump;
    private float _jumpAccelerationSpeed;
    private bool _jump;
    private Vector2 _movement;

    private const float WalkingThreshold = 0.05f;
    private const float SprintingThreshold = 0.7f;
    private const float JumpingThreshold = 0.15f;
    private const float MaxJumpForce = 0.5f;

    //SpriteRenderer spriteRenderer;
    //internal Animator animator;
    readonly PlatformerModel _model = EventManager.GetModel<PlatformerModel>();

    public Bounds Bounds => collider2d.bounds;

    private void Awake()
    {
        health = GetComponent<Health>();
        collider2d = GetComponent<Collider2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (controlEnabled)
        {
            if (Input.acceleration.x >= WalkingThreshold || Input.acceleration.x <= -WalkingThreshold)
            {
                _movement.x = Input.acceleration.x * 1.5f;
            }
            else
            {
                _movement.x = 0;
            }

            if (jumpState == JumpState.Grounded && Input.acceleration.z >= JumpingThreshold)
            {
                jumpState = JumpState.PrepareToJump;
                _jumpAccelerationSpeed = Input.acceleration.z >= MaxJumpForce ? MaxJumpForce : Input.acceleration.z;
            }
            else if (Input.acceleration.z >= JumpingThreshold)
            {
                _jumpAccelerationSpeed = Input.acceleration.z;
                //  stopJump = true;
                //  Schedule<PlayerStopJump>().player = this;
            }
        }
        else
        {
            _movement.x = 0;
        }

        UpdateJumpState();
        UpdateMovementState();
        base.Update();
    }

    private void UpdateJumpState()
    {
        _jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                jumpState = JumpState.Jumping;
                _jump = true;
                _stopJump = false;
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
                _jumpAccelerationSpeed = 0;
                break;
        }
    }

    private void UpdateMovementState()
    {
        if (Math.Abs(_movement.x) >= WalkingThreshold && Math.Abs(_movement.x) < SprintingThreshold)
        {
            movementState = MovementState.Walking;
        }
        else if (Math.Abs(_movement.x) >= SprintingThreshold)
        {
            movementState = MovementState.Sprinting;
        }
        else
        {
            movementState = MovementState.Standing;
        }
    }

    protected override void ComputeVelocity()
    {
        if (_jump && IsGrounded)
        {
            velocity.y = jumpTakeOffSpeed * _model.jumpModifier * (1 + _jumpAccelerationSpeed);
            _jump = false;
        }
        else if (_stopJump)
        {
            _stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * _model.jumpDeceleration * (1 + _jumpAccelerationSpeed);
            }
        }

        // Texturen spiegeln beim umdrehen der Spielfigur
        if (_movement.x > 0.01f)
        {
            // spriteRenderer.flipX = false;
        }
        else if (_movement.x < -0.01f)
        {
            //  spriteRenderer.flipX = true;
        }


        //    animator.SetBool("grounded", IsGrounded);
        //    animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = _movement * maxSpeed;
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }

    public enum MovementState
    {
        Standing,
        Walking,
        Sprinting
    }
}