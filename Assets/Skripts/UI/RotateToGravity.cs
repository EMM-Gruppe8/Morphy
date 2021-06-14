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
        // Measure Angle Between x and y axis and convert to deg.
        float rotation = Mathf.Atan2(-Input.acceleration.x, -Input.acceleration.y)*Mathf.Rad2Deg;

        // Smooth the movement out to not make the controlls jitter
        smoothedRotation = Mathf.LerpAngle(smoothedRotation, rotation, 25.0f * Time.deltaTime);

        //set the angle of the controlls negativ to move against the rotation of the device
        transform.eulerAngles = new Vector3(0,0, -smoothedRotation);
    }
}
