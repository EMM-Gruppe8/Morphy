using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Event that inverts the player movement and flip texture, when the device is rotated
/// </summary>
public class RotateWorld : EventManager.Event<RotateWorld>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        var player = model.player;
        player.invertedMovement = !player.invertedMovement;
        player.spriteRenderer.flipY = !player.spriteRenderer.flipY;
        
    }
}