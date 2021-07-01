using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnteredGoal : EventManager.Event<PlayerEnteredGoal>
{
    public GoalZone GoalZone;

    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        model.player.controlEnabled = false;
        var t = model.highscoreController.GetCurrentTime();
        string timeString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            t.Hours,
            t.Minutes,
            t.Seconds,
            t.Milliseconds);


        if (model.highscoreController.CheckTimeIsNewHighScore(t))
        {
            HighscoreController.SaveHighScore(t);
            Debug.Log("####### GEWONNEN #######\n####### Neue Bestzeit! ####### \n Zeit: " + timeString + "\n ");
        }
        else
        {
            var t2 = model.highscoreController.GetSavedHighScore();
            string timeString2 = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                t2.Hours,
                t2.Minutes,
                t2.Seconds,
                t2.Milliseconds);

            Debug.Log("####### GEWONNEN #######\n####### Keine neue Bestzeit :-( ####### \n Zeit: " + timeString +
                      "\n Best: " + timeString2 + "\n ");
        }

        var customEvent = EventManager.Schedule<LoadLevel>();
        var currentLevelName = SceneManager.GetActiveScene().name;
        if (currentLevelName.ToLower().Contains("level1"))
        {
            LevelController.setHighestCompletedLevel(1);
            customEvent.levelName = "Level2";
        }
        else if (currentLevelName.ToLower().Contains("level2"))
        {
            LevelController.setHighestCompletedLevel(2);
            customEvent.levelName = "Level3";
        }
        else
        {
            LevelController.setHighestCompletedLevel(3);
            customEvent.levelName = "MainMenuScene";
        }
    }
}