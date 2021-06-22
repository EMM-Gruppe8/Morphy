using UnityEngine;


public class EnemyDeath : EventManager.Event<HealthIsZero>
{
    public GameObject gameObject;

    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        var spawner = model.enemyArtifactSpawner;

        // Get position and Type of Enemy
        var position = gameObject.transform.position;
        EnemyArtifact enemyArtifact = gameObject.GetComponent(typeof(EnemyArtifact)) as EnemyArtifact;

        // Disable enemy
        gameObject.SetActive(false);

        // Spawn new enemy artifact
        if (!enemyArtifact) return;
        spawner.SpawnArtifact(enemyArtifact.characterType, position);
    }
}