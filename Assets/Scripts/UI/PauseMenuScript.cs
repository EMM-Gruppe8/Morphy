using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Pause Menu Script
 * Handles opening and closing the pause menu
 */
public class PauseMenuScript : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pauseButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            UpdatePauseStatus();
        }
    }

    // Functions for external actions

    /**
     * Open the Main Menu
     */
    public void GoToMenu() {
        Debug.Log("Open Main Menu");
        Resume();
        SceneManager.LoadScene("MainMenuScene");
    }

    /**
     * Update the UI to reflect the current pause status
     */
    public void UpdatePauseStatus() {
        pauseMenuUI.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    /**
     * Pause the game and show the pause menu
     */
    public void Pause() {
        isPaused = true;
        UpdatePauseStatus();
    }

    /**
     * Resume the game and hide the pause menu
     */
    public void Resume() {
        isPaused = false;
        UpdatePauseStatus();
    }
}
