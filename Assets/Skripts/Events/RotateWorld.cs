using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWorld : EventManager.Event<RotateWorld>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        var player = model.player;
        player.controlEnabled = false;
        player.invertedMovement = !player.invertedMovement;
        player.spriteRenderer.flipY = !player.spriteRenderer.flipY;
        
        EventManager.Schedule<EnablePlayerInput>(2);
    }
}