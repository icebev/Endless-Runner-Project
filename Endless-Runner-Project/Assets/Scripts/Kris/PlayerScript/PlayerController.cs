using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //private GameObject _playerCharacter;
    [SerializeField] private CharacterManager _characterManager;

    public void RotateLeft(InputAction.CallbackContext press)
    {
        // (GameOverEvent.isPlayerDead == true) return;
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Rotate(TurnDirection.Left);
    }

    public void RotateRight(InputAction.CallbackContext press)
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Rotate(TurnDirection.Right);
    }

    public void MoveLeft(InputAction.CallbackContext press)
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Move(CharacterManager.movementDirections.left);
    }

    public void MoveRight(InputAction.CallbackContext press)
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Move(CharacterManager.movementDirections.right);
    }

    public void Jump(InputAction.CallbackContext press)
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Jump();      
    }

    public void Slide(InputAction.CallbackContext press)
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Slide();
    }
}
