using System;
using UnityEngine;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Class defining the behaviour of hostile entities
/// </summary>
public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// The animation object which is used to animate the character.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Defines what kind of character the AI has.
    /// </summary>
    public CharacterType characterType;

    [Header("Pathfinding")]

    /// <summary>
    /// Defines the target the character should move towards.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Defines how far the player has to be away for the character to start moving.
    /// </summary>
    public float activateDistance = 50f;

    /// <summary>
    /// Defines how often the path should be recalculated.
    /// </summary>
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]

    /// <summary>
    /// Defines a baseline speed at which the character should move.
    /// </summary>
    public float speed = 200f;

    /// <summary>
    /// Defines at which point a new Waypoint should be used to follow the path.
    /// </summary>
    public float nextWaypointDistance = 3f;

    /// <summary>
    /// Defines at which height-differenc of the path the charakter should attempt jumping.
    /// </summary>
    public float jumpNodeHeightRequirement = 0.8f;

    /// <summary>
    /// Defines the multiplier that gets applied into the y-direction if a charakter jumps.
    /// </summary>
    public float jumpModifier = 0.3f;

    /// <summary>
    /// Defines a little bit of wiggle-room to decide if the character touches the ground or not.
    /// </summary>
    public float jumpCheckOffset = 0.1f;

    /// <summary>
    /// Defines if the character should walk on the flor or the ceiling.
    /// </summary>
    public bool gravityDown = true;

    /// <summary>
    /// The layer on which the charakter should move on.
    /// </summary>
    public LayerMask platformLayerMask;
    public LayerMask playerLayerMask;
    /// <summary>
    /// The layer on which the playes is on. 
    /// </summary>

    [Header("Custom Behavior")]

    /// <summary>
    /// Defines if the charakter should follow the player.
    /// </summary>
    public bool followEnabled = true;

    /// <summary>
    /// Defines if the charakter is able to jump.
    /// </summary>
    public bool jumpEnabled = true;

    /// <summary>
    /// Defines if the charakter looks into the direction it is going into.
    /// </summary>
    public bool directionLookEnabled = true;
    public float bunnySpecialAttackDelay = 3f;
    private bool bunnyCanAttack = true;

    /// <summary>
    /// The path the charakter takes.
    /// </summary>
    private Path path;

    /// <summary>
    /// Saves at which point on the path the charakter is right now.
    /// </summary>
    private int currentWaypoint = 0;

    /// <summary>
    /// Signals if the charakter touches the ground at the moment.
    /// </summary>
    RaycastHit2D isGrounded;

    /// <summary>
    /// The script enabling the charakter to calculat a path.
    /// </summary>
    Seeker seeker;

    /// <summary>
    /// The part of the charakter on which applies gravity.
    /// </summary>
    Rigidbody2D rb;

    /// <summary>
    /// The forces getting applied to the charakter at any given moment.
    /// </summary>
    Vector2 force;

    /// <summary>
    /// Script that enables the charakter to attack and to get attacked.
    /// </summary>
    AttackableAttacker attackable;
    /// <summary>
    /// The collider surrounding the charakter.
    /// </summary>
    Collider2D collider2d;

    /// <summary>
    /// The boundaries of the Charakter
    /// </summary>
    Bounds Bounds => collider2d.bounds;

    /// <summary>
    /// The state of movement the charakter currently is in.
    /// </summary>
    MovementState movementState = MovementState.Standing;

    /// <summary>
    /// Indicates if it is the first run of the physics simulation.
    /// It fixes strange behaviour with charakter jumping without it.
    /// </summary>
    bool isInitialRun = true;

    /// <summary>
    /// Defines in which direction the gravity should act on the character.
    /// </summary>
    private Vector2 downVector = Vector2.down;

    /// <summary>
    /// Defines at which speed and aboce a movement is considered walking.
    /// </summary>
    private const float WalkingThreshold = 0.05f;

    /// <summary>
    /// Defines at which speed and above a movement is considered sprinting.
    /// </summary>
    private const float SprintingThreshold = 0.7f;

    /// <summary>
    /// Initializes AI by getting all necessary components,
    /// turning the gravity if nexessary and starting
    /// to compute paths.
    /// </summary>
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        attackable = GetComponent<AttackableAttacker>();
        if(!gravityDown){
            TurnGravity(false);
        }
        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }
    
    /// <summary>
    /// Flipping the gravity effect of the charakter.
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

    /// <summary>
    /// Run by unity for physics based interactions.
    /// Used here to calculate the path of the enemy.
    /// </summary>
    private void FixedUpdate()
    {
        if (!TargetInDistance() || !followEnabled) return;
        attackable.attackNearest();
        PathFollow();
        UpdateAnimation();
    }

    /// <summary>
    /// Refreshes the path.
    /// Checks if the charakter should follow,
    /// the target is near enough to be seen
    /// or it has already reached the target.
    /// </summary>
    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    
    /// <summary>
    /// Updates the animation according to the movement direction
    /// and the state of the movement (walking, jumping, running etc.).
    /// </summary>
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

        if (jumpEnabled && !isGrounded && characterType == CharacterType.Bunny)
        {
            animator.SetBool("isJumping", true);
        } else
        {
            animator.SetBool("isJumping", false);
        }
    }

    /// <summary>
    /// Lets the charakter move towards its target.
    /// The AI moves differently depending on the charakter.
    /// </summary>
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
        
        // Direction calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        force = direction * speed * Time.deltaTime;

        bool nearHole = isNearHole();

        // Stop movement if charakter can't jump and is going towards a hole
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

        // Next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    /// <summary>
    /// Checks if the charakter is near a hole.
    /// This is done by casting a ray downwards in front of the charakter.
    /// If it doesn't hit something there is a hole in front.
    /// The threshold for how deep a hole has to be varies by charakter.
    /// </summary>
    /// <returns>if the charakter is near a hole</returns>
    private bool isNearHole(){
        // Check if hole is ahead
        Vector3 ahead;
        float aheadMultiplier = 2;
        if (characterType == CharacterType.Rhino){
            aheadMultiplier = 3;
        }
        if (target.position.x > this.transform.position.x){
            ahead = Vector3.right*aheadMultiplier;
        } else {
            ahead = Vector3.left*aheadMultiplier;
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

        //Draw Lines corresponding to the setting inside the Editor
        Debug.DrawRay(startDown, downVector *rayLength, rayColor, 0);
        Debug.DrawLine(this.transform.position, this.transform.position + ahead, rayColor, 0);

        return nearHole;
    }

    /// <summary>
    /// Makes the charakter jump by applying an upwards force.
    /// </summary>
    public void Jump(){
        if (isGrounded && jumpEnabled && bunnyCanAttack){
            rb.AddForce(-downVector * speed * jumpModifier);
            StartCoroutine(InsertDelayForEnemy());
        }
    }

    private IEnumerator InsertDelayForEnemy(){
        bunnyCanAttack = false;
        yield return new WaitForSeconds(bunnySpecialAttackDelay);
        bunnyCanAttack = true;
    }

    /// <summary>
    /// Makes a charakter fall on the other side by turning
    /// the gravity around.
    /// </summary>
    private void Fall(){
        TurnGravity(true);
    }

    /// <summary>
    /// Makes a charakter run by multiplying its force in
    /// the x-direction.
    /// </summary>
    public void Run(){
        force.x = force.x*2f;
    }

    /// <summary>
    /// Executes a special movement if the charakter is near the player
    /// This is done by casting a ray in the direction the player has
    /// to come from in order for the move to be effective. The direction
    /// and action taken uppon detection varies by charakter.
    /// </summary>
    private void DoSpecialMovement(){
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

    /// <summary>
    /// Measures if the target is in distance by computing the distance
    /// and comparing it to the set activation distance.
    /// </summary>
    /// <returns>if the target is in distance</returns>
    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    /// <summary>
    /// Executes upon reaching the last point of the path.
    /// Sets the currentWaypoint to zero.
    /// </summary>
    /// <param name="p">The Path taken</param>
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /// <summary>
    /// Executed by colliding with other object.
    /// Decides in which angle of attack and which object is hit.
    /// Based upon this an special attack may be triggered.
    /// This is based upon the type of charakter and the angle in which
    /// its hit.
    /// </summary>
    /// <param name="collision"></param>
     void OnCollisionEnter2D(Collision2D collision)
     {
         if (!collision.gameObject.CompareTag("Player")) return;
         try {
             var dir = collision.transform.position - transform.position;
             dir = collision.transform.InverseTransformDirection(dir);
             var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
             if (!collision.gameObject) return;
             if ((characterType == CharacterType.Bunny || characterType == CharacterType.Slime) && gravityDown && (-(angle) >= 60 && -(angle) <= 120))
             {
                 var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                 attackableAttacker.attackWithCustomAction(collision.gameObject);
             }
             if ((characterType == CharacterType.Bunny || characterType == CharacterType.Slime) && !gravityDown && ((angle) >= 60 && (angle) <= 120))
             {
                 var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                 attackableAttacker.attackWithCustomAction(collision.gameObject);
             }
             if ((characterType == CharacterType.Rhino) && (Math.Abs(angle) >= 150 && Math.Abs(angle) <= 210 || Math.Abs(angle) >= 0 && Math.Abs(angle) <= 30 || Math.Abs(angle) >= 330 && Math.Abs(angle) <= 360))
             {
                 var attackableAttacker = collision.gameObject.GetComponent<AttackableAttacker>();
                 attackableAttacker.attackWithCustomAction(collision.gameObject);
             }
         } catch (NullReferenceException e){}
     }

    /// <summary>
    /// Updates the state of the movement and changes between the states
    /// by comparing the force in x-direction with the configured speed thresholds.
    /// </summary>
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

    /// <summary>
    /// The different kind of states of movement a charakter can be in.
    /// </summary>
    public enum MovementState
    {
        Standing,
        Walking,
        Sprinting
    }
}
