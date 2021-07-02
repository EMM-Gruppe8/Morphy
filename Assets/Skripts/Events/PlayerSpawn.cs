using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : EventManager.Event<PlayerSpawn>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        // Respawn player on spawn, activate Input and Set max health
        var player = model.player;
        var highscoreController = model.highscoreController;
        highscoreController.StartMeasurement();
        player.collider2d.enabled = true;
        player.controlEnabled = false;
        player.health.SetMaxHealth();
        player.Teleport(model.spawnPoint.transform.position);
        player.jumpState = PlayerController.JumpState.Grounded;
        // let camera follow player
        model.virtualCamera.m_Follow = player.transform;
        model.virtualCamera.m_LookAt = player.transform;
        EventManager.Schedule<EnablePlayerInput>(2f);
    }
}