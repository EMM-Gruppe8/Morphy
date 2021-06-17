using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController == null) return;
        var customEvent = EventManager.Schedule<PlayerEnteredGoal>();
        customEvent.GoalZone = this;
    }
}