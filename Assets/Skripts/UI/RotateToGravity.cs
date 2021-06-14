using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified copy of the main Gyro Controllers
public class RotateToGravity : MonoBehaviour
{
    float smoothedRotation = 0f;
    // Update is called once per frame
    void Update()
    {
        //
        float rotation = Mathf.Atan2(-Input.acceleration.x, -Input.acceleration.y)*Mathf.Rad2Deg;
        smoothedRotation = Mathf.LerpAngle(smoothedRotation, rotation, 25.0f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0,0, -smoothedRotation);
    }
}
