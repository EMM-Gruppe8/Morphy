using System;
using UnityEngine.SceneManagement;

/// <summary>
///  Event which loads a new level
/// </summary>
public class LoadLevel : EventManager.Event<LoadLevel>
{
    public string levelName;

    public override void Execute()
    {
        try
        {
            SceneManager.LoadScene(levelName); // loads level by name
        }
        catch (Exception e)
        {
            SceneManager.LoadScene("MainMenuScene"); // load main menu, if the level could not be loaded
        }
    }
}