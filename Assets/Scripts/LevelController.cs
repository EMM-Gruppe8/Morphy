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

    // Get the highest level the user completed
    public static int getHighestCompletedLevel() {
        return highestCompletedLevel;
    }

    public static void setHighestCompletedLevel(int val) {
        if (val <= highestCompletedLevel) {
            Debug.Log("Player is already at a higher level");
            return;
        }
        highestCompletedLevel = val;

        // Save new value to the disk
        PlayerPrefs.SetInt("highestCompletedLevel", highestCompletedLevel);
        PlayerPrefs.Save();
    }

    // Load the existing persisted data from the disk if it exists
    public static void loadPersistedData() {
        highestCompletedLevel = PlayerPrefs.GetInt("highestCompletedLevel", 0);
    }
}
