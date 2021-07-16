using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Main Menu Script
/// Handles actions on the main menu scene
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    /// <summary>
    /// Main Menu canvas
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// Level selection canvas
    /// </summary>
    public GameObject levelSelection;

    /// <summary>
    /// List of level selection screens
    /// </summary>
    public GameObject[] levels;

    /// <summary>
    /// List of text boxes to display the highscores
    /// </summary>
    public GameObject[] highscoreDisplays;

    /// <summary>
    /// Setup dynamic parts of the menu
    /// </summary>
    public void Start() {
        updateDisabledLevels();
        updateHighscores();
    }

    /// <summary>
    /// Enable all levels in the level selection screen that the user has unlocked
    /// </summary>
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

    /// <summary>
    /// Update the highscore text displays to display the player's highscores
    /// </summary>
    private void updateHighscores() {
        for(int i = 0; i < highscoreDisplays.Length; i++) {
            TimeSpan highscore = HighscoreController.GetSavedHighScoreForLevelByName("Level" + (i + 1));
            if (highscore != TimeSpan.Zero) {
                highscoreDisplays[i].GetComponent<Text>().text = highscore.ToString(@"mm\:ss");
            }
        }
    }

    /// <summary>
    /// Open the Game Scene to start the game
    /// </summary>
    public void StartGame() {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Level1");

        int currentLevel = LevelController.getHighestCompletedLevel() + 1;
        if (currentLevel > 3) currentLevel = 3;
        OpenLevel(currentLevel);
  }

    /// <summary>
    /// Start the game at a specific level
    /// </summary>
    /// <param name="level">
    /// Level to start at
    /// </param>
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

    /// <summary>
    /// Quit the application
    /// </summary>
    public void QuitGame() {
        Application.Quit();
    }

    /// <summary>
    /// Open the Level Select screen and hide the main menu
    /// </summary>
    public void OpenLevelSelect() {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true);
    }

    /// <summary>
    /// Open the Main Menu screen and hide the level select screen
    /// </summary>
    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        levelSelection.SetActive(false);
    }
}
