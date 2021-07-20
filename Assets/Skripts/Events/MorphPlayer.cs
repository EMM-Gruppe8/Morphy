using UnityEngine;

/// <summary>
///  Event which changes the target charakter type of the player by a enemy artifact
/// </summary>
public class MorphPlayer : EventManager.Event<MorphPlayer>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();
    public GameObject gameObject;

    public override void Execute()
    {
        EnemyArtifact enemyArtifact = gameObject.GetComponent(typeof(EnemyArtifact)) as EnemyArtifact;
        if (!enemyArtifact) return;
        model.player.targetCharacterType = enemyArtifact.characterType; 
        model.player.health.SetMaxHealth();
        gameObject.SetActive(false); // disable artifact
    }
}