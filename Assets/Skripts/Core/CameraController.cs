using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool rotate;
    public RoateDirection Direction = RoateDirection.UP;
    float speed = 200f;

    public Cinemachine.CinemachineVirtualCamera virtualCamera;

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