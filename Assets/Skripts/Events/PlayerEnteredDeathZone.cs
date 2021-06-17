using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnteredDeathZone : EventManager.Event<PlayerEnteredDeathZone>
{
    public DeathZone DeathZone;

    PlatformerModel _model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        EventManager.Schedule<PlayerDeath>(0);
    }
}