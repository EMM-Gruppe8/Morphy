using System;
using UnityEngine;
using Pathfinding;
/// <summary>
/// Class defining the behaviour of hostile entities
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public CharacterType characterType;

    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float jumpNodeHeightRequirement = 0.8f;
    public float jumpModifier = 0.3f;
    public float jumpCheckOffset = 0.1f;
    public bool gravityDown = true;

    public LayerMask platformLayerMask;
    public LayerMask playerLayerMask;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool jumpEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    RaycastHit2D isGrounded;
    Seeker seeker;
    Rigidbody2D rb;
    Vector2 force;
    AttackableAttacker attackable;
    Collider2D collider2d;
    Bounds Bounds => collider2d.bounds;
    MovementState movementState = MovementState.Standing;
    bool isInitialRun = true;
    private Vector2 downVector = Vector2.down;

    private const float WalkingThreshold = 0.05f;
    private const float SprintingThreshold = 0.7f;

    /// <summary>
    /// Initializes AI by getting all necessary components,
    /// turning the gravity if nexessary and starting
    /// to compute paths
    /// </summary>
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        if(!gravityDown){
            TurnGravity(false);
        }
        attackable = GetComponent<AttackableAttacker>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }
    
    /// <summary>
    /// Flipping the gravity effect of the charakter
    /// </summary>
    /// <param name="turnSemaphore">Signals if the gravity boolean should be inverted</param>
    private void TurnGravity(bool turnSemaphore){
        rb.gravityScale=-rb.gravityScale;
        downVector = -downVector;
        transform.localScale = new Vector3(transform.localScale.x, -downVector.y * Mathf.Abs(transform.localScale.y), transform.localScale.z);
        jumpNodeHeightRequirement = -jumpNodeHeightRequirement;
        if (turnSemaphore){
            gravityDown = !gravityDown;
        }
    }

    private void FixedUpdate()
    {
        attackable.attackNearest();
        if (TargetInDistance() && followEnabled)
        {
            PathFollow();
            UpdateAnimation();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void UpdateAnimation(){
        // Direction Graphics Handling
        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        // Animation
        if (isGrounded)
        {
            animator.SetFloat("Speed", Mathf.Abs(force.x));
        }
        else if (rb.velocity.magnitude < 0)
        {
            animator.SetFloat("Speed", 0);
        } 

        if (jumpEnabled && !isGrounded)
        {
            animator.SetBool("isJumping", true);
        } else
        {
            animator.SetBool("isJumping", false);
        }
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        isGrounded = Physics2D.Raycast(GetComponent<Collider2D>().bounds.center, downVector, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, platformLayerMask);
        
        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        bool nearHole = isNearHole();

        // Stop movement if Charakter can't jump and is going towards a hole
        if(nearHole && !jumpEnabled){
            force = Vector3.zero;
        }
        
        if (!isInitialRun){
            // Jump if charakter can jump and is near a hole or the player has the highground
            if (nearHole | direction.y > jumpNodeHeightRequirement){
                Jump();
            }
            DoSpecialMovement();
        }

        isInitialRun = false;

        // Disable upward force if Charakter is Jumping
        if (!isGrounded){ 
            force.y = 0;
        }
        UpdateMovementState();
        rb.AddForce(force);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private bool isNearHole(){
        // Check if hole is ahead
        Vector3 ahead;
        if (target.position.x > this.transform.position.x){
            ahead = Vector3.right*2;
        } else {
            ahead = Vector3.left*2;
        }

        float rayLength;
        if (jumpEnabled){
            rayLength = 15;
        } else{
            rayLength = 2;
        }
        
        Vector3 startDown = this.transform.position + ahead;
        RaycastHit2D  raycastHit = Physics2D.Raycast(startDown, downVector, rayLength, platformLayerMask);

        Color rayColor;

        bool nearHole = false;
        if (raycastHit.collider == null){
            nearHole = true;
        }

        if(!nearHole){
            rayColor = Color.green;
        } else {
            rayColor = Color.red;
        }
        Debug.DrawRay(startDown, downVector *rayLength, rayColor, 0);
        Debug.DrawLine(this.transform.position, this.transform.position + ahead, rayColor, 0);

        return nearHole;
    }

    public void Jump(){
        if (isGrounded && jumpEnabled){
            rb.AddForce(-downVector * speed * jumpModifier);  
        }
    }

    public void Fall(){
        TurnGravity(true);
    }

    public void Run(){
        force.x = force.x*1.5f;
    }

    public void DoSpecialMovement(){
        if (characterType == CharacterType.Bunny){
            Vector3 ahead;
            if (target.position.x > this.transform.position.x){
                ahead = Vector3.right;
            } else {
                ahead = Vector3.left;
            }
            float rayLength = 5f;
            RaycastHit2D  raycastHit = Physics2D.Raycast(this.transform.position, ahead, rayLength, playerLayerMask);

            bool nearEnemy = true;
            if (raycastHit.collider == null){
                nearEnemy = false;
            }

            Color rayColor;
            if(!nearEnemy){
                rayColor = Color.yellow;
            } else {
                rayColor = Color.blue;
            }

            Debug.DrawRay(this.transform.position, ahead * rayLength, rayColor, 0);

            if (nearEnemy){
                Jump();
            }
        }
        else if (characterType == CharacterType.Rhino){
            Vector3 ahead;
            if (target.position.x > this.transform.position.x){
                ahead = Vector3.right;
            } else {
                ahead = Vector3.left;
            }
            float rayLength = 15f;
            RaycastHit2D  raycastHit = Physics2D.Raycast(this.transform.position, ahead, rayLength, playerLayerMask);

            bool nearEnemy = true;
            if (raycastHit.collider == null){
                nearEnemy = false;
            }

            Color rayColor;
            if(!nearEnemy){
                rayColor = Color.yellow;
            } else {
                rayColor = Color.blue;
            }

            Debug.DrawRay(this.transform.position, ahead * rayLength, rayColor, 0);

            if (nearEnemy){
                Run();
            } 
        }
        else if (characterType == CharacterType.Slime){
            Vector3 over = -downVector;
            float rayLength = 30f;
            RaycastHit2D  raycastHit = Physics2D.Raycast(this.transform.position, over, rayLength, playerLayerMask);

            bool enemyOver = true;
            if (raycastHit.collider == null){
                enemyOver = false;
            }

            Color rayColor;
            if(!enemyOver){
                rayColor = Color.yellow;
            } else {
                rayColor = Color.blue;
            }

            Debug.DrawRay(this.transform.position, over * rayLength, rayColor, 0);

            if (enemyOver){
                Fall();
            }
        }
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

     void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != gameObject.tag){
            // Check position of collision
            bool landedOnTop;
            if(!gravityDown){
                landedOnTop = Bounds.center.y <= collision.collider.bounds.min.y;
            } else {
                landedOnTop = Bounds.center.y >= collision.collider.bounds.max.y;
            }
            try {
                switch (landedOnTop)
                {
                    // Special attack if Bunny or Slime jumps on head
                    case true when characterType == CharacterType.Bunny ||
                                characterType == CharacterType.Slime && collision.gameObject:
                        {
                            var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                            attackableAttacker.attackWithCustomAction(collision.gameObject);
                            break;
                        }
                    // Special attack if Rhino sprints on enemy
                    case false when characterType == CharacterType.Rhino && movementState == MovementState.Sprinting &&
                                    collision.gameObject:
                        {
                            var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                            attackableAttacker.attackWithCustomAction(collision.gameObject);
                            break;
                        }
                }
            } catch (NullReferenceException e){}
        }
    }

    private void UpdateMovementState()
    {
        if (Math.Abs(force.x) >= WalkingThreshold && Math.Abs(force.x) < SprintingThreshold)
        {
            movementState = MovementState.Walking;
        }
        else if (Math.Abs(force.x) >= SprintingThreshold)
        {
            movementState = MovementState.Sprinting;
        }
        else
        {
            movementState = MovementState.Standing;
        }
    }

    public enum MovementState
    {
        Standing,
        Walking,
        Sprinting
    }
}
