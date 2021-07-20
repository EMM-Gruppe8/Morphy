using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Event which handles the death of the player
/// </summary>
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

        // Lose camera Focus and deactivate input for smooth respawn
        model.virtualCamera.m_Follow = null;
        model.virtualCamera.m_LookAt = null;
        player.controlEnabled = false;
        player.collider2d.enabled = false;
        var customEvent = EventManager.Schedule<LoadLevel>(2);
        customEvent.levelName = SceneManager.GetActiveScene().name; // reload scene
    }
}