using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreController : MonoBehaviour
{
    private DateTime _startTime = DateTime.Now;

    private void Awake()
    {
        StartMeasurement();
    }

    public void StartMeasurement()
    {
        _startTime = DateTime.UtcNow;
    }

    public TimeSpan GetCurrentTime()
    {
        return DateTime.UtcNow - _startTime;
    }

    public bool CheckTimeIsNewHighScore(TimeSpan timeSpan)
    {
        if (GetSavedHighScore() == TimeSpan.Zero)
        {
            return true;
        }

        return GetSavedHighScore() > timeSpan;
    }

    public static void SaveHighScore(TimeSpan timeSpan)
    {
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, timeSpan.TotalSeconds.ToString(CultureInfo.CurrentCulture));
        PlayerPrefs.Save();
    }

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
}