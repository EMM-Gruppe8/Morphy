using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // TODO: Implement when Game Engine is ready
    }

    /**
     * Start the game at a specific level
     */
    public void OpenLevel(int level) {
        Debug.Log("Start Game at level " + level);
        // TODO: Implement when Game Engine is ready
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
