using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnteredGoal : EventManager.Event<PlayerEnteredGoal>
{
    public GoalZone GoalZone;

    PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    public override void Execute()
    {
        model.player.controlEnabled = false;
        var t = model.highscoreController.GetTime();
        string timeString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            t.Hours,
            t.Minutes,
            t.Seconds,
            t.Milliseconds);


        // TODO: Zeit speichern
        // TODO: Event zum nächsten Level Starten
        Debug.Log("####### GEWONNEN #######  \n Zeit: " + timeString + "\n ");
        EventManager.Schedule<PlayerSpawn>(2);
    }
}