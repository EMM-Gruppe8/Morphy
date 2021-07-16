using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component can be added to all objects that can attack others and that can be attacked by others
/// Other components need to activate the "attackNearest" Method in this class in order for this attack to attack.
/// </summary>
public class AttackableAttacker : MonoBehaviour
{
    /// <summary>
    /// Range this object can use to attack other objects
    /// </summary>
    public float attackRange = 150;

    /// <summary>
    /// Range for near attacks
    /// </summary>
    public float nearAttackRange = 100;

    /// <summary>
    /// Define the character mass needed for calculating knockback forces
    /// </summary>
    public float mass = 1.0f;

    /// <summary>
    /// Knockback Impact vector currently impacting this object
    /// </summary>
    Vector3 impact = Vector3.zero;

    /// <summary>
    /// Is the current object in attack cooldown?
    /// Cooled down objects cannot attack others until the cooldown is over
    /// </summary>
    bool isInCooldown = false;

    /// <summary>
    /// Time the cooldown needs to complete in seconds.
    /// By default, an attacker has a cooldown of 1 second before they can attack again
    /// </summary>
    public float cooldownTimeInSeconds = 1;

    /// <summary>
    /// Attack Button that should be greyed out during cooldown.
    /// If no button is provided, this will be ignored
    /// </summary>
    public GameObject attackButton;

    /// <summary>
    /// Delay the enemies use while attacking
    /// </summary>
    public float enemyAttackDelay = .2f;

    /// <summary>
    /// Saves, if this attack if the first attack of an attacker
    /// </summary>
    private bool initialAttack = true;

    /// <summary>
    /// Get the tag for our enemy, who we should attack
    /// For the player, these will be the "Enemy" objects
    /// For enemies this will be the player
    /// </summary>
    /// <returns>
    /// Unity Tag String "Player" or "Enemy"
    /// </returns>
    public string getEnemyTag() {
        if (gameObject.tag == "Player") {
            return "Enemy";
        }
        return "Player";
    }

    /// <summary>
    /// Calculate the distance from the current object to another GameObject
    /// </summary>
    /// <param name="go">
    /// GameObject to calculate the distance to
    /// </param>
    /// <returns>
    /// Distance
    /// </returns>
    public float calculateDistanceToObject(GameObject go) {
        Vector3 ownPosition = transform.position;
        Vector3 diff = go.transform.position - ownPosition;
        return diff.sqrMagnitude;
    }

    /// <summary>
    /// Get the Game object for our nearest enemy
    /// This will return "null" if no enemy exists
    /// </summary>
    /// <returns>
    /// GameObject or null
    /// </returns>
    public GameObject getNearestEnemy() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(getEnemyTag());

        GameObject closest = null;
        float distance = Mathf.Infinity;
        
        foreach (GameObject go in gos)
        {
            float curDistance = calculateDistanceToObject(go);
            Debug.Log(curDistance);
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    /// <summary>
    /// Attack the nearest enemy to this object using the standard attack
    /// </summary>
    public void attackNearest() {
        GameObject nearestEnemy = getNearestEnemy();
        if (nearestEnemy == null) {
            Debug.Log("No enemy found");
            return;
        }

        attack(nearestEnemy);
    }

    /// <summary>
    /// Attack the nearest enemy with our current special action
    /// </summary>
    public void attackNearestWithCustomAction() {
        GameObject nearestEnemy = getNearestEnemy();
        if (nearestEnemy == null) {
            Debug.Log("No enemy found");
            return;
        }

        attackWithCustomAction(nearestEnemy);
    }

    /// <summary>
    /// Check, if this attacker can currently attack another GameObject.
    /// This will check, that this attacker is not currently in cooldown
    /// and that the other object is in attackable distance
    /// </summary>
    /// <param name="go">
    /// Other object that should be attacked
    /// </param>
    /// <param name="range">
    /// Attack range this attacker has
    /// </param>
    /// <returns>
    /// True or false
    /// </returns>
    private bool canAttack(GameObject go, float range) {
        if (isInCooldown) {
            Debug.Log("Object is currently in cooldown and can't attack");
            return false;
        }
        if (calculateDistanceToObject(go) > attackRange) {
            Debug.Log("No enemy in attack range");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Try to attack a specific GameObject on the map.
    /// </summary>
    /// <param name="go">
    /// GameObject to attack
    /// </param>
    public void attack(GameObject go) {
        if (!canAttack(go, attackRange)) {
            Debug.Log("Can't attack");
            return;
        }

        if (gameObject.tag == "Enemy" && initialAttack) {
            StartCoroutine(InsertDelayForEnemy());
            return;
        }

        Debug.Log("Attacking enemy");
        if (gameObject.tag == "Player") {
            FindObjectOfType<AudioManager>().Play("PlayerAttack");
        }
        go.GetComponent<AttackableAttacker>().getAttacked(gameObject, 1);

        StartCoroutine(cooldown());
    }

    /// <summary>
    /// Insert a delay for the enemy attack
    /// </summary>
    /// <returns>
    /// Nothing
    /// </returns>
    private IEnumerator InsertDelayForEnemy(){
        yield return new WaitForSeconds(enemyAttackDelay);
        initialAttack = false;
    }

    /// <summary>
    /// Try to attack a specific GameObject with our special custom action
    /// </summary>
    /// <param name="go">
    /// GameObject to attack
    /// </param>
    public void attackWithCustomAction(GameObject go) {
        if (!canAttack(go, nearAttackRange)) {
            Debug.Log("Can't attack");
            return;
        }
        
        Debug.Log("Attacking enemy with custom action");
        // Special action can do 2 damage
        go.GetComponent<AttackableAttacker>().getAttacked(gameObject, 2);

        StartCoroutine(cooldown());
    }

    /// <summary>
    /// Let the attack cooldown
    /// </summary>
    /// <returns>
    /// Nothing
    /// </returns>
    private IEnumerator cooldown() {
        isInCooldown = true;
        if (gameObject.tag == "Player") {
            var customEvent = EventManager.Schedule<AttackEntersLeavesCooldown>();
            customEvent.attackButton = attackButton;
            customEvent.isInCooldown = true;
        }
        yield return new WaitForSeconds(cooldownTimeInSeconds);
        isInCooldown = false;
        if (gameObject.tag == "Player") {
            var customEvent = EventManager.Schedule<AttackEntersLeavesCooldown>();
            customEvent.attackButton = attackButton;
            customEvent.isInCooldown = false;
        }
    }

    /// <summary>
    /// Let this object get attacked by another object
    /// This method is mostly called by other AttackableAttacker components to attack each other
    /// </summary>
    /// <param name="attacker">
    /// Attacker Object that attacked us
    /// </param>
    /// <param name="strength">
    /// Strength of the attack
    /// </param>
    public void getAttacked(GameObject attacker, int strength) {
        Vector2 pathFromAttackerToMe = transform.position - attacker.transform.position;
        pathFromAttackerToMe.Normalize();

        AddImpact((2 * pathFromAttackerToMe + new Vector2(0, 2)));
        StartCoroutine(blink());
        
        Debug.Log("Got attacked");
        Debug.Log(pathFromAttackerToMe);

        Health h = GetComponent<Health>();
        if (h != null) {
            h.Decrement(strength);
            Debug.Log("Decremented health by: " + strength);
        }
    }

    /// <summary>
    /// Let the current sprite blink to indicate being attacked
    /// </summary>
    /// <param name="times">
    /// Number of times a "blink" should occur
    /// </param>
    /// <param name="pause">
    /// Pause length between changing the color in seconds
    /// </param>
    /// <returns>
    /// Nothing
    /// </returns>
    IEnumerator blink(int times = 2, float pause = 0.1f) {
        for (int i = 0; i < times; i++) {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(pause);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(pause);
        }
    }

    /// <summary>
    /// Get if the current object is in a cooldown
    /// </summary>
    /// <returns>
    /// True if in cooldown
    /// </returns>
    public bool getIsInCooldown() {
        return isInCooldown;
    }

    /// <summary>
    /// Add a new impact to the object.
    /// 
    /// The impact system is sourced from: http://answers.unity.com/answers/309747/view.html
    /// </summary>
    /// <param name="force">
    /// Force to add to the object
    /// </param>
    void AddImpact(Vector3 force){
        impact += force / mass;
    }
 
    /// <summary>
    /// Update the current object to add the knockback
    /// </summary>
   void Update(){ 
     // apply the impact effect:
     if (impact.magnitude > 0.2f){
        transform.position += impact * Time.deltaTime;
     }
     // impact energy goes by over time:
     impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
   }
}
