using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified copy of the main Gyro Controllers
public class RotateToGyroScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Input.gyro.enabled = true;
        transform.rotation = GyroToUnity(Input.gyro.attitude);

        // 0 > screen.width
        // (PI / 4) > screen.height
        float zVal = Mathf.Abs(Input.gyro.attitude.z);
        // float zVal = Input.gyro.attitude.z;

        // float widthPercent = (-(Mathf.Cos((zVal + Mathf.PI / 4) * Mathf.PI * 1.5f)) / 2f) + 0.5f;
        // float minPercent = ((float)Screen.height / (float)Screen.width) - 0.1f;
        // float width = minPercent + (widthPercent * (1 - minPercent));
        // transform.localScale = new Vector3(
        //     width,
        //     1,
        //     1
        // );
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(0, 0, -q.z, q.w);
    }
}
