using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Defines a Zone in which the player conpleted the level
/// </summary>
public class GoalZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController == null) return;
        var customEvent = EventManager.Schedule<PlayerEnteredGoal>();
        customEvent.GoalZone = this;
        FindObjectOfType<AudioManager>().Play("LevelComplete");
    }
}