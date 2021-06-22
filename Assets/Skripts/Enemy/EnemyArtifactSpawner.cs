using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArtifactSpawner : MonoBehaviour
{
    public Transform enemySlimeArtifactPrefab;
    public Transform enemyRhinoArtifactPrefab;
    public Transform enemyBunnyArtifactPrefab;


    public void SpawnArtifact(CharacterType characterType, Vector3 position)
    {
        switch (characterType)
        {
            case CharacterType.Slime:
                Instantiate(enemySlimeArtifactPrefab, position, Quaternion.identity);
                break;
            case CharacterType.Bunny:
                Instantiate(enemyBunnyArtifactPrefab, position, Quaternion.identity);
                break;
            case CharacterType.Rhino:
                Instantiate(enemyRhinoArtifactPrefab, position, Quaternion.identity);
                break;
            case CharacterType.Bee:
                break;
            case CharacterType.Snail:
                break;
            case CharacterType.NotSpecified:
                break;
        }
    }
}