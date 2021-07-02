using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventManager;

public class PlayerController : KinematicObject
{
    public JumpState jumpState = JumpState.Grounded;
    public MovementState movementState = MovementState.Standing;
    public Animator animator;
    public BoxCollider2D collider2d;
    public Health health;
    public SpriteRenderer spriteRenderer;
    public bool controlEnabled = true;
    public CharacterType targetCharacterType = CharacterType.Bunny;
    private CharacterType _currentCharacterType = CharacterType.NotSpecified;

    private bool _stopJump;
    private float _jumpAccelerationSpeed;
    private bool _jump;
    private Vector2 _movement;
    private RoateDirection _rotateDirection = RoateDirection.DOWN;

    private const float WalkingThreshold = 0.05f;
    private const float SprintingThreshold = 0.7f;
    private const float JumpingThreshold = 0.15f;
    private float _maxJumpForce = 0.5f;
    private float _maxSpeed = 8;
    private float _jumpTakeOffSpeed = 7;

    PlatformerModel model = GetModel<PlatformerModel>();
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int Speed = Animator.StringToHash("Speed");

    public Bounds Bounds => collider2d.bounds;

    private void Awake()
    {
        health = GetComponent<Health>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        UpdateGravityRotation();
        ProcessPlayerAccelerationInput();
        UpdateJumpState();
        UpdateMovementState();
        UpdateAnimator();
        UpdateCharacterType();
        UpdateBoxCollider2D();
        base.Update();
    }

    private void ProcessPlayerAccelerationInput()
    {
        if (controlEnabled)
        {
            if (Input.acceleration.x >= WalkingThreshold || Input.acceleration.x <= -WalkingThreshold)
            {
                if (jumpState == JumpState.InFlight && _currentCharacterType == CharacterType.Slime)
                {
                    _movement.x = 0;
                }
                else
                {
                    _movement.x = (Invert * Input.acceleration.x * 1.5f);
                }
            }
            else
            {
                _movement.x = 0;
            }

            // Let only the Bunny Jump
            if (jumpState == JumpState.Grounded && Input.acceleration.z >= JumpingThreshold &&
                _currentCharacterType == CharacterType.Bunny)
            {
                jumpState = JumpState.PrepareToJump;
                _jumpAccelerationSpeed = Input.acceleration.z >= _maxJumpForce ? _maxJumpForce : Input.acceleration.z;
            }
            else if (Input.acceleration.z >= JumpingThreshold)
            {
                _jumpAccelerationSpeed = Input.acceleration.z;
            }
        }
        else
        {
            _movement.x = 0;
        }
    }

    private void UpdateGravityRotation()
    {
        if (_currentCharacterType != CharacterType.Slime) return; // Update rotation only if Character is a Slime
        var angle = Math.Abs(Mathf.Atan2(-Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg);
        switch (_rotateDirection)
        {
            case RoateDirection.DOWN when angle >= 90:
                jumpState = JumpState.PrepareToJump;
                Schedule<RotateWorld>();
                _rotateDirection = RoateDirection.UP;
                break;
            case RoateDirection.UP when angle < 90:
                jumpState = JumpState.PrepareToJump;
                Schedule<RotateWorld>();
                _rotateDirection = RoateDirection.DOWN;
                break;
        }
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
                    jumpState = JumpState.InFlight;
                }

                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }

                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                _jumpAccelerationSpeed = 0;
                break;
            case JumpState.Grounded:
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

    private void UpdateCharacterType()
    {
        if (_currentCharacterType != targetCharacterType)
        {
            ChangeCharacterType(targetCharacterType);
        }
    }

    private void UpdateBoxCollider2D()
    {
        collider2d.size = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
    }

    public float calculateDistanceToObject(GameObject go)
    {
        Vector3 ownPosition = transform.position;
        Vector3 diff = go.transform.position - ownPosition;
        return diff.sqrMagnitude;
    }

    public GameObject getNearestEnemyArtifact()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("EnemyArtifact");
        GameObject closest = null;
        var distance = Mathf.Infinity;

        foreach (GameObject go in gos)
        {
            var curDistance = calculateDistanceToObject(go);

            if (!(curDistance < distance)) continue;
            closest = go;
            distance = curDistance;
        }

        return distance <= 6.0f ? closest : null;
    }

    public void morphNearest()
    {
        var gameObject = getNearestEnemyArtifact();
        if (!gameObject) return;
        var customEvent = Schedule<MorphPlayer>();
        customEvent.gameObject = gameObject;
    }

    private void ChangeCharacterType(CharacterType characterType)
    {
        switch (characterType)
        {
            case CharacterType.Bunny:
            {
                _maxJumpForce = 0.5f;
                _maxSpeed = 8;
                _jumpTakeOffSpeed = 6;

                if (animator.gameObject.activeSelf)
                {
                    animator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animation/BunnyEnemy");
                }

                break;
            }
            case CharacterType.Slime:
            {
                _maxJumpForce = 0.0f;
                _maxSpeed = 3;
                _jumpTakeOffSpeed = 0;

                if (animator.gameObject.activeSelf)
                {
                    animator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animation/SlimeEnemy");
                }

                break;
            }
            case CharacterType.Rhino:
            {
                _maxJumpForce = 0.0f;
                _maxSpeed = 10;
                _jumpTakeOffSpeed = 0;

                if (animator.gameObject.activeSelf)
                {
                    animator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animation/RhinoEnemy");
                }

                break;
            }
            case CharacterType.Bee:
            {
                _maxJumpForce = 0.1f;
                _maxSpeed = 8;
                _jumpTakeOffSpeed = 1;

                if (animator.gameObject.activeSelf)
                {
                    animator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animation/BeeEnemy");
                }

                break;
            }
            case CharacterType.Snail:
            {
                _maxJumpForce = 0.0f;
                _maxSpeed = 1;
                _jumpTakeOffSpeed = 1;

                if (animator.gameObject.activeSelf)
                {
                    animator.runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>("Animation/SnailEnemy");
                }

                break;
            }
            case CharacterType.NotSpecified:
                break;
        }

        _currentCharacterType = characterType;
    }

    protected override void ComputeVelocity()
    {
        if (_jump && IsGrounded)
        {
            velocity.y = _jumpTakeOffSpeed * model.jumpModifier * (1 + _jumpAccelerationSpeed);
            _jump = false;
        }
        else if (_stopJump)
        {
            _stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * model.jumpDeceleration * (1 + _jumpAccelerationSpeed);
            }
        }

        // Texturen spiegeln beim umdrehen der Spielfigur
        if (Invert * _movement.x > 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (Invert * _movement.x < -0.01f)
        {
            spriteRenderer.flipX = false;
        }

        TargetVelocity = _movement * _maxSpeed;
    }

    private void UpdateAnimator()
    {
        if (_currentCharacterType != CharacterType.Bunny) return;
        animator.SetFloat(Speed, Mathf.Abs(_movement.x));

        switch (jumpState)
        {
            case JumpState.Grounded:
            case JumpState.PrepareToJump:
            case JumpState.Landed:
                animator.SetBool(IsJumping, false);
                break;
            case JumpState.Jumping:
            case JumpState.InFlight:
                animator.SetBool(IsJumping, true);
                break;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check position of collision
        var landedOnTop = Bounds.center.y >= collision.collider.bounds.max.y;

        switch (landedOnTop)
        {
            // Special attack if Bunny or Slime jumps on head
            case true when _currentCharacterType == CharacterType.Bunny ||
                           _currentCharacterType == CharacterType.Slime && collision.gameObject:
            {
                var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                attackableAttacker.attackWithCustomAction(collision.gameObject);
                break;
            }
            // Special attack if Rhino sprints on enemy
            case false when _currentCharacterType == CharacterType.Rhino && movementState == MovementState.Sprinting &&
                            collision.gameObject:
            {
                var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                attackableAttacker.attackWithCustomAction(collision.gameObject);
                break;
            }
        }
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