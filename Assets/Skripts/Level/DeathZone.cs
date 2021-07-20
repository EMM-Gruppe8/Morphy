using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Defines a Zone in which the player should immediately die, when he enters the zone
/// </summary>
public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController == null) return;
        var customEvent = EventManager.Schedule<PlayerEnteredDeathZone>(); // Event to kill the Player
        customEvent.DeathZone = this;
    }
}