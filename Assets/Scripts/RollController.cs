using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        float xValue=Input.acceleration.x;
        float yValue=Input.acceleration.y;

        GUILayout.Label("x-rotation: " + xValue);
        GUILayout.Label("y-rotation: " + yValue);
        
        if(yValue<0){
            GUILayout.Label("up");
        }
        if(yValue>0){
            GUILayout.Label("down");
        }
        if(xValue>0){
            GUILayout.Label("right");
        }
        if(xValue<0){
            GUILayout.Label("left");
        }
    }
}
