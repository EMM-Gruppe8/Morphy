using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public TimeSpan GetTime()
    {
        return DateTime.UtcNow - _startTime;
    }
}