using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerData : InputManagerHandler
{
    [HideInInspector]
    public Vector2 MoveInput;
    [HideInInspector]
    public Vector3 CurrentInput;

    [Header("Player Movement")]
    public float PlayerWalkSpeed;
    public float SprintSpeed;
    [HideInInspector]
    public float SprintSpeedData;

    [Header("Camera Look")]
    public float MouseSensivity;
    public Transform Camera;

    [HideInInspector]
    public Vector2 MouseInput;
    [HideInInspector]
    public float MouseX, MouseY, xRotation;

    [Header("Gravity & Jump")]
    public float Gravity;
    public float JumpHeight;

    [HideInInspector]
    public Vector3 Velocity;
    [HideInInspector]
    public bool JumpInput;

}
