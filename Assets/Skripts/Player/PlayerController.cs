using System;
using UnityEngine;
using static EventManager;

/// <summary>
/// The player controller manges all actions perfomed by the player, the input, special actions and parameters for the charakter types
/// </summary>
public class PlayerController : KinematicObject
{
    /// <summary>
    /// Defines the jump state. 
    /// </summary>
    public JumpState jumpState = JumpState.Grounded;

    /// <summary>
    /// Defines the movement state
    /// </summary>
    public MovementState movementState = MovementState.Standing;

    /// <summary>
    /// Current animation of the player
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Defines the box collider 2D
    /// </summary>
    public BoxCollider2D collider2d;

    /// <summary>
    /// gives the player the ability to hab health points
    /// </summary>
    public Health health;

    /// <summary>
    /// the texture of the playermodel
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// indicates, if the input will be processed
    /// </summary>
    public bool controlEnabled = true;

    /// <summary>
    /// Defines the target character type
    /// </summary>
    public CharacterType targetCharacterType = CharacterType.Bunny;

    /// <summary>
    /// Defines the actual character type
    /// </summary>
    private CharacterType _currentCharacterType = CharacterType.NotSpecified;

    /// <summary>
    /// Indicate if the player stops jumping
    /// </summary>
    private bool _stopJump;

    /// <summary>
    /// Defines the acceleration speed of a jump
    /// </summary>
    private float _jumpAccelerationSpeed;

    /// <summary>
    /// indicates if the player is jumping
    /// </summary>
    private bool _jump;

    /// <summary>
    /// Defines movemnt as a Vector2
    /// </summary>
    private Vector2 _movement;

    /// <summary>
    /// Defines the current rotate direction
    /// </summary>
    private RoateDirection _rotateDirection = RoateDirection.DOWN;

    /// <summary>
    /// defines the thresholds on wich a walking movent should be detected
    /// </summary>
    private const float WalkingThreshold = 0.05f;

    /// <summary>
    /// defines the thresholds on wich a the sprinting state should be set
    /// </summary>
    private const float SprintingThreshold = 0.7f;

    /// <summary>
    /// defines the thresholds on wich a the jumping state should be set
    /// </summary>
    private const float JumpingThreshold = 0.15f;

    /// <summary>
    /// defines the max jump force
    /// </summary>
    private float _maxJumpForce = 0.5f;

    /// <summary>
    /// defines the max speed
    /// </summary>
    private float _maxSpeed = 8;

    /// <summary>
    /// defines the take of speed of a jump
    /// </summary>
    private float _jumpTakeOffSpeed = 7;

    /// <summary>
    /// The global player model
    /// </summary>
    PlatformerModel model = GetModel<PlatformerModel>();

    /// <summary>
    /// String hash to identify the is jumping parameter of the animator
    /// </summary>
    private static readonly int IsJumping = Animator.StringToHash("isJumping");

    /// <summary>
    /// String hash to identify the is speed parameter of the animator
    /// </summary>
    private static readonly int Speed = Animator.StringToHash("Speed");

    /// <summary>
    /// Defines the bounds of the 2d collider
    /// </summary>
    public Bounds Bounds => collider2d.bounds;

    /// <summary>
    /// instanciate the health and collider of the player
    /// </summary>
    private void Awake()
    {
        health = GetComponent<Health>();
        collider2d = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Processes updates on every frame
    /// </summary>
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

    /// <summary>
    /// Processes the acceleration input from the device, if the controll is enabled and the pause menu is not open.
    /// Invertes the movement, if the player is on the ceiling of the level
    /// </summary>
    private void ProcessPlayerAccelerationInput()
    {
        if (controlEnabled && !PauseMenuScript.isPaused)
        {
            if (Input.acceleration.x >= WalkingThreshold || Input.acceleration.x <= -WalkingThreshold)
            {
                // moves the player horizontal, if the player stands on the ground
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
                FindObjectOfType<AudioManager>().Play("PlayerJump");
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

    /// <summary>
    /// Process the rotation changes if the the player is a slime and the pause menu is not open
    /// </summary>
    private void UpdateGravityRotation()
    {
        if (_currentCharacterType != CharacterType.Slime || PauseMenuScript.isPaused) return; // Update rotation only if Character is a Slime
        var angle = Math.Abs(Mathf.Atan2(-Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg);
        switch (_rotateDirection)
        {
            case RoateDirection.DOWN when (angle >= 90 && Math.Abs(angle - 180f) > 0.000000f):
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

    /// <summary>
    /// processes the change of the Jumo state which is important for the animator and movemnet
    /// </summary>
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

    /// <summary>
    /// Processes the movemnet state, which is important for the animator and the special attack of the rhino
    /// </summary>
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

    /// <summary>
    /// updates the current character type to the target character type, if they are not the same
    /// </summary>
    private void UpdateCharacterType()
    {
        if (_currentCharacterType != targetCharacterType)
        {
            ChangeCharacterType(targetCharacterType);
        }
    }

    /// <summary>
    /// updates the sizes of the collider based on the texture of the current animation
    /// </summary>
    private void UpdateBoxCollider2D()
    {
        collider2d.size = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
    }

    /// <summary>
    ///  calculates the distance to a given game object
    /// </summary>
    /// <param name="go">the game object</param>
    /// <returns></returns>
    public float calculateDistanceToObject(GameObject go)
    {
        Vector3 ownPosition = transform.position;
        Vector3 diff = go.transform.position - ownPosition;
        return diff.sqrMagnitude;
    }

    /// <summary>
    /// find the nearest gameobject in a given distance
    /// </summary>
    /// <returns>the nearest game object</returns>
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

    /// <summary>
    /// changes the character type to the type of the nearest artifact in a given distance
    /// </summary>
    public void morphNearest()
    {
        var gameObject = getNearestEnemyArtifact();
        if (!gameObject) return;
        var customEvent = Schedule<MorphPlayer>();
        customEvent.gameObject = gameObject;
        FindObjectOfType<AudioManager>().Play("PlayerMorph");
    }

    /// <summary>
    /// Processes the change of the character type, loads the new animation and sets the special parameter
    /// </summary>
    /// <param name="characterType">the character type</param>
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

    /// <summary>
    /// Computes the velocity of the player
    /// </summary>
    protected override void ComputeVelocity()
    {
        if (_jump && IsGrounded)
        {
            velocity.y = Invert * _jumpTakeOffSpeed * model.jumpModifier * (1 + _jumpAccelerationSpeed);
            _jump = false;
        }
        else if (_stopJump)
        {
            _stopJump = false;
            if (velocity.y > 0)
            {
                velocity.y = Invert * velocity.y * model.jumpDeceleration * (1 + _jumpAccelerationSpeed);
            }
        }

        // Flips textures to the direction the player is moving
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

    /// <summary>
    /// Updates the animator based on the jump states
    /// </summary>
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

    /// <summary>
    /// handles the collision with an enemy and special attacks based on collision
    /// </summary>
    /// <param name="collision">the other object</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        try
        {
            var dir = collision.transform.position - transform.position;
            dir = collision.transform.InverseTransformDirection(dir);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            if (!collision.gameObject) return;
            // special attack enemy, if the player is a bunny and jumps on the enemy in a given angle
            if ((_currentCharacterType == CharacterType.Bunny || _currentCharacterType == CharacterType.Slime) &&
                _rotateDirection == RoateDirection.DOWN && (-(angle) >= 60 && -(angle) <= 120))
            {
                var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                attackableAttacker.attackWithCustomAction(collision.gameObject);
            }
            // special attack enemy, if the player is a bunny and jumps on the enemy in a given angle on the ceiling of the level
            if ((_currentCharacterType == CharacterType.Bunny || _currentCharacterType == CharacterType.Slime) &&
                _rotateDirection == RoateDirection.UP && ((angle) >= 60 && (angle) <= 120))
            {
                var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                attackableAttacker.attackWithCustomAction(collision.gameObject);
            }

            // special attack enemy, if the player is a rhino and hits the enemy from a given angle
            if ((_currentCharacterType == CharacterType.Rhino) && (Math.Abs(angle) >= 150 && Math.Abs(angle) <= 210 ||
                                                                   Math.Abs(angle) >= 0 && Math.Abs(angle) <= 30 ||
                                                                   Math.Abs(angle) >= 330 && Math.Abs(angle) <= 360))
            {
                var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                attackableAttacker.attackWithCustomAction(collision.gameObject);
            }
        }
        catch (NullReferenceException e)
        {
            // should not happen
        }
    }
    /// <summary>
    /// defines all jump states
    /// </summary>
    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Jumping,
        InFlight,
        Landed
    }
    /// <summary>
    /// Definces all movement states
    /// </summary>
    public enum MovementState
    {
        Standing,
        Walking,
        Sprinting
    }
}