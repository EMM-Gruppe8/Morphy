using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIsZero : EventManager.Event<HealthIsZero>
{
    public Health Health;

    public override void Execute()
    {
        EventManager.Schedule<PlayerDeath>();
    }
}
