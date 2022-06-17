using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    private GameObject _playerCharacter;
    [SerializeField] private CharacterManager _characterManager;

    public void RotateLeft(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            this._characterManager.Rotate(TurnDirection.Left);
        }
    }

    public void RotateRight(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            this._characterManager.Rotate(TurnDirection.Right);
        }
    }

    public void MoveLeft(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            this._characterManager.Move((int)CharacterManager.movementDirections.left);
        }
    }

    public void MoveRight(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            this._characterManager.Move((int)CharacterManager.movementDirections.right);
        }
    }

    private void Start()
    {
        this._playerCharacter = this.gameObject;
    }
}
