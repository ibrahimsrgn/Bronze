using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraLook : InputManagerHandler
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        LookAround();
    }

    private void LookAround()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.y = _PlayerData.Camera.eulerAngles.y;
        transform.eulerAngles = currentRotation;
    }
}
