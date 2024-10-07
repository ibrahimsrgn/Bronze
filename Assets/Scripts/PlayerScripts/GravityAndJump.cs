using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAndJump : InputManagerHandler
{
    void Update()
    {
        ApplyGravity();
        _CharacterController.Move(_PlayerData.Velocity * Time.deltaTime);
        Jump();
    }

    private void ApplyGravity()
    {
        if (!_CharacterController.isGrounded)
        {
            _PlayerData.Velocity.y += _PlayerData.Gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        if  (_PlayerData.JumpInput && _CharacterController.isGrounded)
        {
            _PlayerData.Velocity.y = Mathf.Sqrt(_PlayerData.JumpHeight * -2f * _PlayerData.Gravity);
        }
        if (_CharacterController.isGrounded && _PlayerData.Velocity.y < 0)
        {
            _PlayerData.Velocity.y = -2f;
        }
        _CharacterController.Move(_PlayerData.Velocity * Time.deltaTime);
    }
}
