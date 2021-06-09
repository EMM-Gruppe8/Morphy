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

    /**
     * Open the Game Scene to start the game
     */
    public void StartGame() {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Prototype1");
    }

    /**
     * Start the game at a specific level
     */
    public void OpenLevel(int level) {
        Debug.Log("Start Game at level " + level);
        switch(level){
            case 1:
                SceneManager.LoadScene("GyroDemo");
                break;
            case 2:
                SceneManager.LoadScene("Prototype1");
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
