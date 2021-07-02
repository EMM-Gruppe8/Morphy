using System;
using UnityEngine.SceneManagement;

public class LoadLevel : EventManager.Event<LoadLevel>
{
    public string levelName;

    public override void Execute()
    {
        try
        {
            SceneManager.LoadScene(levelName);
        }
        catch (Exception e)
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}