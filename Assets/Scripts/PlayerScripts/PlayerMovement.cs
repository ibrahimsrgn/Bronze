using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : InputManagerHandler
{
    void Update()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        _PlayerData.CurrentInput = transform.right * _PlayerData.MoveInput.x + transform.forward * _PlayerData.MoveInput.y;
        _CharacterController.Move(_PlayerData.CurrentInput * _PlayerData.PlayerWalkSpeed * Time.deltaTime);
    }
}
