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
            _characterManager.Rotate(0);
        }


    }
    public void RotateRight(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            _characterManager.Rotate(1);
        }

    }


    public void MoveLeft(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            _characterManager.MoveLeft();
        }
        
       
    }
    public void MoveRight(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            _characterManager.MoveRight();
        }

    }


    private void Start()
    {
        _playerCharacter = this.gameObject;

    }
    private void Update()
    {

    }



}
