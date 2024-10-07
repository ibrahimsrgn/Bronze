using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerHandler : MonoBehaviour
{
    protected CharacterController _CharacterController;
    protected PlayerData _PlayerData;

    public void Awake()
    {
        _CharacterController = GetComponent<CharacterController>();
        _PlayerData = GetComponent<PlayerData>();
    }

    private void OnWasd(InputValue Value)
    {
        _PlayerData.MoveInput = Value.Get<Vector2>();
    }

    private void OnLook(InputValue Value)
    {
        _PlayerData.MouseInput = Value.Get<Vector2>();
    }

    private void OnJump(InputValue Value)
    {
        _PlayerData.JumpInput = Value.isPressed;
    }
}
