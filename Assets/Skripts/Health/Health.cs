using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Basic health controller for the player and the enemies.
/// </summary>
public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public bool IsAlive => currentHp > 0;

    public int currentHp;

    private Slider healthBar = null;

    public GameObject healthBarPrefab;

    /// <summary>
    /// Create health bar
    /// </summary>
    void Start() {
        GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, GetComponent<SpriteRenderer>().bounds.size.y, 0), Quaternion.identity, transform);
        healthBar = bar.GetComponentInChildren<Slider>();
    }

    /// <summary>
    /// Increment the current health and update the healthbar
    /// </summary>
    /// <param name="amount"></param>
    public void Increment(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHP);
        updateHealthBar();
    }

    /// <summary>
    /// Decrement the current health and update the healthbar
    /// </summary>
    /// <param name="amount"></param>
    public void Decrement(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp - amount, 0, maxHP);
        updateHealthBar();
        if (currentHp != 0) return;
        if (gameObject.tag == "Player") {
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
        } else {
            FindObjectOfType<AudioManager>().Play("EnemyDeath");
        }
        var customEvent = EventManager.Schedule<HealthIsZero>();
        customEvent.gameObject = gameObject;

    }

    /// <summary>
    /// Decrement the current health points to zero
    /// </summary>
    public void Die()
    {
        while (currentHp > 0) Decrement();
    }

    /// <summary>
    /// Set the current health points to max
    /// </summary>
    public void SetMaxHealth()
    {
        currentHp = maxHP;
        updateHealthBar();
    }

    /// <summary>
    /// Sets the current health points to max, when instance is created
    /// </summary>
    void Awake()
    {
        currentHp = maxHP;
        updateHealthBar();
    }

    private void updateHealthBar() {
        if (healthBar != null) {
            healthBar.value = (float) currentHp / (float) maxHP;
        }
    }

}