using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Event that makes the player die when he enters the death zone
/// </summary>
public class PlayerEnteredDeathZone : EventManager.Event<PlayerEnteredDeathZone>
{
    public DeathZone DeathZone;

    PlatformerModel _model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        EventManager.Schedule<PlayerDeath>(0); // kill player
    }
}