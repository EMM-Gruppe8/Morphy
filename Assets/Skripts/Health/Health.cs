using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public bool IsAlive => currentHp > 0;

    public int currentHp;

    private Slider healthBar = null;

    public GameObject healthBarPrefab;

    void Start() {
        // Create health bar
        GameObject bar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, GetComponent<SpriteRenderer>().bounds.size.y, 0), Quaternion.identity, transform);
        healthBar = bar.GetComponentInChildren<Slider>();
    }

    public void Increment(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHP);
        updateHealthBar();
    }

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

    public void Die()
    {
        while (currentHp > 0) Decrement();
    }

    public void SetMaxHealth()
    {
        currentHp = maxHP;
        updateHealthBar();
    }

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