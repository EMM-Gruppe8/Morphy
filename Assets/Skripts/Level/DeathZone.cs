using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var playerController = collider.gameObject.GetComponent<PlayerController>();
        if (playerController == null) return;
        var customEvent = EventManager.Schedule<PlayerEnteredDeathZone>();
        customEvent.DeathZone = this;
    }
}