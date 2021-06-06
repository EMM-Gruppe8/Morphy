using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayerInput : EventManager.Event<EnablePlayerInput>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        var player = model.player;
        player.controlEnabled = true;
    }
}
