using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : InputManagerHandler
{
    void Update()
    {
        MovementAnimations();
        CharacterMove();
    }

    private void CharacterMove()
    {
        _PlayerData.CurrentInput = transform.right * _PlayerData.MoveInput.x + transform.forward * _PlayerData.MoveInput.y;
        _CharacterController.Move(_PlayerData.CurrentInput * _PlayerData.PlayerWalkSpeed * Time.deltaTime * (_PlayerData.MoveInput.y > 0 ? _PlayerData.SprintSpeedData : 1));
    }

    private void MovementAnimations()
    {
        float targetSpeed = _PlayerData.SprintSpeedData > 1 ? 2f : 1f;
        _PlayerData.SmoothInput = Vector2.Lerp(_PlayerData.SmoothInput, _PlayerData.MoveInput * targetSpeed, _PlayerData.SmoothSpeed * Time.deltaTime);
        _PlayerData.Animator.SetFloat("Horizontal", _PlayerData.SmoothInput.x);
        _PlayerData.Animator.SetFloat("Vertical", _PlayerData.SmoothInput.y);
    }
}
