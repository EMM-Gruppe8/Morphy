using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Main Menu Script
 * Handles actions on the main menu scene
 */
public class MainMenuScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelection;
    public GameObject[] levels;

    // Setup dynamic parts of the menu
    public void Start() {
        updateDisabledLevels();
    }

    // Enable all levels in the level selection screen that the user has unlocked
    private void updateDisabledLevels() {
        int enabledLevels = LevelController.getHighestCompletedLevel();
        Debug.Log("Enabeling level selection up to level " + (enabledLevels + 1));
        for(int i = 0; i <= enabledLevels; i++) {
            if (levels.Length > i) {
                levels[i].transform.Find("Disabled").gameObject.SetActive(false);
                Debug.Log("Enabling Level" + (i + 1));
            }
        }
    }

    /**
     * Open the Game Scene to start the game
     */
    public void StartGame() {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Level1");

        int currentLevel = LevelController.getHighestCompletedLevel() + 1;
        if (currentLevel > 3) currentLevel = 3;
        OpenLevel(currentLevel);
  }

    /**
     * Start the game at a specific level
     */
    public void OpenLevel(int level) {
        Debug.Log("Start Game at level " + level);

        if (LevelController.getHighestCompletedLevel() + 1 < level) {
            Debug.Log("Level is not unlocked yet");
            return;
        }

        switch(level){
            case 1:
                SceneManager.LoadScene("Level1");
                break;
            case 2:
                SceneManager.LoadScene("Level2");
                break;
            case 3:
                SceneManager.LoadScene("Level3");
                break;
        }
    }

    /**
     * Quit the application
     */
    public void QuitGame() {
        Application.Quit();
    }

    /**
     * Open the Level Select screen and hide the main menu
     */
    public void OpenLevelSelect() {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true);
    }

    /**
     * Open the Main Menu screen and hide the level select screen
     */
    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        levelSelection.SetActive(false);
    }
}
