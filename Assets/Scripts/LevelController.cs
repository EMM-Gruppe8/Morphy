using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Controller for Level Management
public class LevelController : MonoBehaviour
{
    // Highest Level that has been completed
    // This will be persisted on the hard drive
    private static int highestCompletedLevel = 0;

    // Save if we already loaded the data from disk
    private static bool hasLoaded = false;

    // Number of levels that exist in the game - for validation purposes
    private static readonly int MAX_LEVEL = 3;

    // Get the highest level the user completed
    public static int getHighestCompletedLevel() {
        // Make sure we are setup first
        setup();
        return highestCompletedLevel;
    }

    // Set the highest level, the player has completed
    // Example:
    // LevelController.setHighestCompletedLevel(currentLevel);
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

    // Load the existing persisted data from the disk if it exists and if we are not already setup
    private static void setup() {
        if (!hasLoaded) {
            highestCompletedLevel = PlayerPrefs.GetInt("highestCompletedLevel", 0);
            hasLoaded = true;
        }
    }
}
