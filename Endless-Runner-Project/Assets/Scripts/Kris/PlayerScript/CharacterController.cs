using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    private GameObject _playerCharacter;
    private Vector3 _PlayerPos;

    private void Start()
    {
        _playerCharacter = this.gameObject;
        _PlayerPos = this.gameObject.transform.position; 
    }




}
