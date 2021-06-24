using UnityEngine;

public class MorphPlayer : EventManager.Event<MorphPlayer>
{
    PlatformerModel model = EventManager.GetModel<PlatformerModel>();
    public GameObject gameObject;

    public override void Execute()
    {
        EnemyArtifact enemyArtifact = gameObject.GetComponent(typeof(EnemyArtifact)) as EnemyArtifact;
        if (!enemyArtifact) return;
        model.player.targetCharacterType = enemyArtifact.characterType;
        gameObject.SetActive(false);
    }
}