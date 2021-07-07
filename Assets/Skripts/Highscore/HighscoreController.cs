using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller who manages all high scores and times. The current implementation only has the time measurement.
/// </summary>
public class HighscoreController : MonoBehaviour
{
    private DateTime _startTime = DateTime.Now;

    /// <summary>
    /// Starts the time measurement.
    /// </summary>
    private void Awake()
    {
        StartMeasurement();
    }

    /// <summary>
    /// sets the the current Time.
    /// </summary>
    public void StartMeasurement()
    {
        _startTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Reads the current TimeSpan, from start to now.
    /// </summary>
    public TimeSpan GetCurrentTime()
    {
        return DateTime.UtcNow - _startTime;
    }

    /// <summary>
    /// Checks if a given TimeSpan is a better highscore fpr the current llevel
    /// </summary>
    public bool CheckTimeIsNewHighScore(TimeSpan timeSpan)
    {
        if (GetSavedHighScore() == TimeSpan.Zero)
        {
            return true;
        }

        return GetSavedHighScore() > timeSpan;
    }

    /// <summary>
    /// Saves a HighScore for the current Level to the user pref.
    /// </summary>
    public static void SaveHighScore(TimeSpan timeSpan)
    {
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name,
            timeSpan.TotalSeconds.ToString(CultureInfo.CurrentCulture));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Reads the Highscore TimeSpan for the current level.
    /// </summary>
    public TimeSpan GetSavedHighScore()
    {
        try
        {
            var seconds = double.Parse(PlayerPrefs.GetString(SceneManager.GetActiveScene().name));
            return TimeSpan.FromSeconds(seconds);
        }
        catch (Exception e)
        {
            return TimeSpan.Zero;
        }
    }

    /// <summary>
    /// Reads the Highscore TimeSpan for a specific level. Return the TimeSpan.Zero,
    /// </summary>
    public TimeSpan GetSavedHighScoreForLevelByName(string level)
    {
        try
        {
            var seconds = double.Parse(PlayerPrefs.GetString(level));
            return TimeSpan.FromSeconds(seconds);
        }
        catch (Exception e)
        {
            return TimeSpan.Zero;
        }
    }
}