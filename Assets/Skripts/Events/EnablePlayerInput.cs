using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Event which enabled the input of the player
/// </summary>
public class EnablePlayerInput : EventManager.Event<EnablePlayerInput>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>(); // PlatformerModel to access the player

    public override void Execute()
    {
        var player = model.player;
        player.controlEnabled = true;
    }
}