using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for rotating an object with the gravity acting on the device.
/// </summary>
public class RotateToGravity : MonoBehaviour
{
    /// <summary>
    /// Saving a time delayed value of the rotation th smoot out the movement
    /// </summary>
    float smoothedRotation = 0f;

    /// <summary>
    /// Rotates the object it is attached to with the gravity.
    /// This is done by measuring the angle between the x and y forces acting upon the device.
    /// Thhis angle is then smoothed out and an subtracted from the object to counter its movement.
    /// </summary>
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
