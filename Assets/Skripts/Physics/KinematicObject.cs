using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implements game physics for some in game entity.
/// </summary>
public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// The minimum normal (dot product) considered suitable for the entity sit on.
    /// </summary>
    public float minGroundNormalY = .65f;

    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    public float gravityModifier = 1f;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public Vector2 velocity;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public bool invertedMovement = false;

    /// <summary>
    /// Is the entity currently sitting on a surface?
    /// </summary>
    /// <value></value>
    protected bool IsGrounded { get; private set; }

    protected Vector2 TargetVelocity;
    private Vector2 _groundNormal;
    private Rigidbody2D _body;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private const float MINMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

    protected int Invert = 1;
    private float _tmpMinGroundNormalY = 1;

    /// <summary>
    /// Bounce the object's vertical velocity.
    /// </summary>
    /// <param name="value"></param>
    public void Bounce(float value)
    {
        velocity.y = value;
    }

    /// <summary>
    /// Bounce the objects velocity in a direction.
    /// </summary>
    /// <param name="dir"></param>
    public void Bounce(Vector2 dir)
    {
        velocity.y = dir.y;
        velocity.x = dir.x;
    }

    /// <summary>
    /// Teleport to some position.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        _body.position = position;
        velocity *= 0;
        _body.velocity *= 0;
    }

    protected virtual void OnEnable()
    {
        _body = GetComponent<Rigidbody2D>();
        _body.isKinematic = true;
    }

    protected virtual void OnDisable()
    {
        _body.isKinematic = false;
    }

    protected virtual void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        _contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        TargetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {
    }

    protected virtual void FixedUpdate()
    {
        if (invertedMovement)
        {
            Invert = -1;
            _tmpMinGroundNormalY = (-minGroundNormalY) - 1;
        }
        else
        {
            Invert = 1;
            _tmpMinGroundNormalY = minGroundNormalY;
        }

        if (velocity.y < 0)
            velocity += Physics2D.gravity * (gravityModifier * Invert * Time.deltaTime);
        else
            velocity += Physics2D.gravity * (Invert * Time.deltaTime);

        velocity.x = TargetVelocity.x;
        IsGrounded = false;
        var deltaPosition = velocity * Time.deltaTime;
        var moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        var move = moveAlongGround * deltaPosition.x;
        PerformMovement(move, false);
        move = Vector2.up * deltaPosition.y;
        PerformMovement(move, true);
    }

    void PerformMovement(Vector2 move, bool yMovement)
    {
        var distance = move.magnitude;
        if (distance > MINMoveDistance)
        {
            var count = _body.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);
            for (var i = 0; i < count; i++)
            {
                var currentNormal = _hitBuffer[i].normal;
                if (currentNormal.y > _tmpMinGroundNormalY)
                {
                    IsGrounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                if (IsGrounded)
                {
                    var projection = Vector2.Dot(velocity, currentNormal);
                    if (projection < 0)
                    {
                        velocity = velocity - projection * currentNormal;
                    }
                }
                else
                {
                    velocity.x *= 0;
                    velocity.y = Mathf.Min(velocity.y, 0);
                }

                var modifiedDistance = _hitBuffer[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _body.position += move.normalized * distance;
    }
}