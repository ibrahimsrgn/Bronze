using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : InputManagerHandler
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
    }

    private void LookAround()
    {
        _PlayerData.MouseX = _PlayerData.MouseInput.x * _PlayerData.MouseSensivity * Time.deltaTime;
        _PlayerData.MouseY = _PlayerData.MouseInput.y * _PlayerData.MouseSensivity * Time.deltaTime;

        _PlayerData.xRotation -= _PlayerData.MouseY;
        _PlayerData.xRotation = Mathf.Clamp(_PlayerData.xRotation, -90, 90);

        _PlayerData.Camera.localRotation = Quaternion.Euler(_PlayerData.xRotation, 0 ,0);
        transform.Rotate(Vector3.up * _PlayerData.MouseX);
    }
}
