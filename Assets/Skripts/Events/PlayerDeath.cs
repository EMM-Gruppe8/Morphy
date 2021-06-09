using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : EventManager.Event<PlayerDeath>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        var player = model.player;
        if (player.health.IsAlive)
        {
            player.health.Die();
        }
        model.virtualCamera.m_Follow = null;
        model.virtualCamera.m_LookAt = null;
        player.controlEnabled = false;
        player.collider2d.enabled = false;
        EventManager.Schedule<PlayerSpawn>(2);
    }
}