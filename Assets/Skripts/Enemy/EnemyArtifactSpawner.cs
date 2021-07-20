using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Artifact Spawner, which contains all artifacts of dead enemys
/// </summary>
public class EnemyArtifactSpawner : MonoBehaviour
{
    /// <summary>
    /// slime artifact prefab
    /// </summary>
    public Transform enemySlimeArtifactPrefab;

    /// <summary>
    /// rhino artifact prefab
    /// </summary>
    public Transform enemyRhinoArtifactPrefab;

    /// <summary>
    /// bunny artifact prefab
    /// </summary>
    public Transform enemyBunnyArtifactPrefab;

    /// <summary>
    /// Instantiate an Artifact on the position where a enemy has died. The artifact contains the character type information of the dead enemy
    /// </summary>
    /// <param name="characterType">character type of the dead enemy</param>
    /// <param name="position">Position of the dead enemy</param>
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
            case CharacterType.Bee: // not implemented
                break;
            case CharacterType.Snail: // not implemented
                break;
            case CharacterType.NotSpecified: // default
                break;
        }
    }
}