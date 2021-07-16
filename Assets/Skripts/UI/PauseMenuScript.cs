using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause Menu Script
/// Handles opening and closing the pause menu
/// </summary>
public class PauseMenuScript : MonoBehaviour
{
    /// <summary>
    /// Is the pause menu currently paused?
    /// </summary>
    public static bool isPaused = false;

    /// <summary>
    /// GameObject for the menu canvas
    /// </summary>
    public GameObject pauseMenuUI;

    /// <summary>
    /// GameObject for the pause button
    /// </summary>
    public GameObject pauseButton;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            UpdatePauseStatus();
        }
    }

    // Functions for external actions

    /// <summary>
    /// Open the Main Menu
    /// </summary>
    public void GoToMenu() {
        Debug.Log("Open Main Menu");
        Resume();
        SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// Update the UI to reflect the current pause status
    /// </summary>
    private void UpdatePauseStatus() {
        pauseMenuUI.SetActive(isPaused);
        pauseButton.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    /// <summary>
    /// Pause the game and show the pause menu
    /// </summary>
    public void Pause() {
        isPaused = true;
        UpdatePauseStatus();
    }

    /// <summary>
    /// Resume the game and hide the pause menu
    /// </summary>
    public void Resume() {
        isPaused = false;
        UpdatePauseStatus();
    }

    /// <summary>
    /// Reset the level
    /// </summary>
    public void reset() {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
