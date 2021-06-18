using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public int maxHP = 100;
    public bool IsAlive => currentHp > 0;

    public int currentHp;

    public void Increment(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHP);
    }

    public void Decrement(int amount = 1)
    {
        currentHp = Mathf.Clamp(currentHp - amount, 0, maxHP);
        if (currentHp == 0)
        {
            var customEvent = EventManager.Schedule<HealthIsZero>();
            customEvent.Health = this;
        }
    }

    public void Die()
    {
        while (currentHp > 0) Decrement();
    }

    public void SetMaxHealth()
    {
        currentHp = maxHP;
    }

    void Awake()
    {
        currentHp = maxHP;
    }
}