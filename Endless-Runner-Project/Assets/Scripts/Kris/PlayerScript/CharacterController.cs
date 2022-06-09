using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{

    private GameObject _playerCharacter;
    [SerializeField] private CharacterManager _characterManager;

    public void MoveLeft()
    {
        _characterManager.MoveLeft();
       
    }
    public void MoveRight()
    {
        _characterManager.MoveRight();

    }


    private void Start()
    {
        _playerCharacter = this.gameObject;

    }
    private void Update()
    {

    }



}
