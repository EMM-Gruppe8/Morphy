using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Indicates if camera should rotating
    /// </summary>
    public bool rotate;
    /// <summary>
    /// defines the roate rirection
    /// </summary>
    public RoateDirection Direction = RoateDirection.UP; // Default ist Up
    float speed = 200f;

    /// <summary>
    /// the virtualCamera object
    /// </summary>
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// If rotating, with methods rotated the virtual Camera to the other direction.
    /// </summary>
    void Update()
    {
        if (rotate)
        {
            if (Direction == RoateDirection.UP)
            {
                virtualCamera.transform.transform.Rotate(0f, 0f, Time.deltaTime * speed);
                if (virtualCamera.transform.eulerAngles.z >= 180f)
                {
                    virtualCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
                    rotate = false;
                    Direction = RoateDirection.DOWN;
                }
            }
            else
            {
                virtualCamera.transform.transform.Rotate(0f, 0f, -Time.deltaTime * speed);
                if (virtualCamera.transform.eulerAngles.z <= 5f)
                {
                    virtualCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rotate = false;
                    Direction = RoateDirection.UP;
                }
            }
        }
    }
}