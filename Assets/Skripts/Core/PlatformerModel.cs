using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformerModel
{
    /// <summary>
    /// The virtual camera in the scene.
    /// </summary>
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// The main component which controls the player sprite, controlled 
    /// by the user.
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// The main component which controls the highscores
    /// </summary>
    public HighscoreController highscoreController;

    /// <summary>
    /// Spawner for Enemy artifact, on enemy death event
    /// </summary>
    public EnemyArtifactSpawner enemyArtifactSpawner;

    /// <summary>
    /// The spawn point in the scene.
    /// </summary>
    public Transform spawnPoint;

    /// <summary>
    /// A global jump modifier applied to all initial jump velocities.
    /// </summary>
    public float jumpModifier = 1.0f;

    /// <summary>
    /// A global jump modifier applied to slow down an active jump when 
    /// the user releases the jump input.
    /// </summary>
    public float jumpDeceleration = 0.5f;
}