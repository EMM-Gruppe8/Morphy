using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Event which manages the case, when a player or a enemy has no health points
/// </summary>
public class HealthIsZero : EventManager.Event<HealthIsZero>
{
    public GameObject gameObject;

    public override void Execute()
    {
        // Event for Player Death
        if (gameObject.CompareTag("Player"))
        {
            EventManager.Schedule<PlayerDeath>();
        }
        else
        {
            // Or Enemy Death
            var customEvent = EventManager.Schedule<EnemyDeath>();
            customEvent.gameObject = gameObject;
        }
    }
}