using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AttackableAttacker: This component can be added to all objects that can attack others and that can be attacked by others
// Other components need to activate the "attackNearest" Method in this class in order for this attack to attack.
public class AttackableAttacker : MonoBehaviour
{
    // Range this object can use to attack other objects
    public float attackRange = 150;

    // Range for near attacks
    public float nearAttackRange = 100;

    // define the character mass
    public float mass = 1.0f;

    // Impact currently impacting this object
    Vector3 impact = Vector3.zero;

    // Is the current object in attack cooldown?
    // Cooled down objects cannot attack others until the cooldown is over
    bool isInCooldown = false;

    // Time the cooldown needs to complete
    public float cooldownTimeInSeconds = 1;

    // Attack Button that should be greyed out during cooldowm
    public GameObject attackButton;

    public float enemyAttackDelay = .2f;

    private bool initialAttack = true;

    // Get the tag for our enemy, who we should attack
    // For the player, these will be the "Enemy" objects
    // For enemies this will be the player
    public string getEnemyTag() {
        if (gameObject.tag == "Player") {
            return "Enemy";
        }
        return "Player";
    }

    // Calculate the distance to another GameObject
    public float calculateDistanceToObject(GameObject go) {
        Vector3 ownPosition = transform.position;
        Vector3 diff = go.transform.position - ownPosition;
        return diff.sqrMagnitude;
    }

    // Get the Game object for our nearest enemy
    // This will return "null" if no enemy exists
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

    // Attack the nearest GameObject to this object
    public void attackNearest() {
        GameObject nearestEnemy = getNearestEnemy();
        if (nearestEnemy == null) {
            Debug.Log("No enemy found");
            return;
        }

        attack(nearestEnemy);
    }

    // Attack the nearest enemy with our current special action
    public void attackNearestWithCustomAction() {
        GameObject nearestEnemy = getNearestEnemy();
        if (nearestEnemy == null) {
            Debug.Log("No enemy found");
            return;
        }

        attackWithCustomAction(nearestEnemy);
    }

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

    // Try to attack a specific GameObject on the map
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
        go.GetComponent<AttackableAttacker>().getAttacked(gameObject, 1);

        StartCoroutine(cooldown());
    }

    private IEnumerator InsertDelayForEnemy(){
        yield return new WaitForSeconds(enemyAttackDelay);
        initialAttack = false;
    }

    // Try to attack a specific GameObject with our special custom action
    public void attackWithCustomAction(GameObject go) {
        if (!canAttack(go, nearAttackRange)) {
            Debug.Log("Can't attack");
            return;
        }
        
        // TODO: Handle custom actions. This can only be implemented once skills have been implemented

        Debug.Log("Attacking enemy with custom action");
        // Special action can do 2 damage
        go.GetComponent<AttackableAttacker>().getAttacked(gameObject, 2);

        StartCoroutine(cooldown());
    }

    // Let the attack cooldown
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

    // Let this object get attacked by another object
    // This method is mostly called by other AttackableAttacker components to attack each other
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

    // Let the current sprite blink to indicate being attacked
    IEnumerator blink(int times = 2, float pause = 0.1f) {
        for (int i = 0; i < times; i++) {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(pause);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(pause);
        }
    }

    // Get if the current object is in a cooldown
    public bool getIsInCooldown() {
        return isInCooldown;
    }

    // Impact System, Source: http://answers.unity.com/answers/309747/view.html
    void AddImpact(Vector3 force){
        impact += force / mass;
    }
 
   void Update(){ 
     // apply the impact effect:
     if (impact.magnitude > 0.2f){
        transform.position += impact * Time.deltaTime;
     }
     // impact energy goes by over time:
     impact = Vector3.Lerp(impact, Vector3.zero, 5*Time.deltaTime);
   }
}
