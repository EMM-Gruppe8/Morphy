using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public PlatformerModel model = EventManager.GetModel<PlatformerModel>();

    void OnEnable()
    {
        Instance = this;
    }

    void OnDisable()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (Instance == this) EventManager.Tick();
    }
}