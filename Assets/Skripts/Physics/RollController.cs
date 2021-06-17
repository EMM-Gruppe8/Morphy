using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollController : MonoBehaviour
{
    public int acceleration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xValue=Input.acceleration.x;
        float yValue=Input.acceleration.y;

        Vector2 force = new Vector2(xValue, yValue);

        GetComponent<Rigidbody2D>().velocity =force*acceleration;
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
