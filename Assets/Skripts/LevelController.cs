using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Controller to manage all levels and unlock them.
/// </summary>
public class LevelController : MonoBehaviour
{
    // Highest Level that has been completed
    // This will be persisted on the hard drive
    private static int highestCompletedLevel = 0;

    // Save if we already loaded the data from disk
    private static bool hasLoaded = false;

    // Number of levels that exist in the game - for validation purposes
    private static readonly int MAX_LEVEL = 3;

    /// <summary>
    /// Get the highest level the user completed
    /// </summary>
    /// <returns>highestCompletedLevel as integer</returns>
    public static int getHighestCompletedLevel() {
        // Make sure we are setup first
        setup();
        return highestCompletedLevel;
    }


    /// <summary>
    /// Set the highest level, the player has completed
    /// Example:
    /// LevelController.setHighestCompletedLevel(currentLevel);
    /// </summary>
    /// <param name="val"></param>
    public static void setHighestCompletedLevel(int val) {
        // Make sure we are setup first
        setup();

        if (val <= highestCompletedLevel) {
            Debug.Log("Player is already at a higher level");
            return;
        }
        if (val > MAX_LEVEL) {
            Debug.Log("Trying to set level higher than levels exist");
            return;
        }
        highestCompletedLevel = val;

        // Save new value to the disk
        PlayerPrefs.SetInt("highestCompletedLevel", highestCompletedLevel);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Load the existing persisted data from the disk if it exists and if we are not already setup
    /// </summary>
    private static void setup() {
        if (!hasLoaded) {
            highestCompletedLevel = PlayerPrefs.GetInt("highestCompletedLevel", 0);
            hasLoaded = true;
        }
    }
}
