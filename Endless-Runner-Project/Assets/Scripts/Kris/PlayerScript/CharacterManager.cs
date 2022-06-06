using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject _character;
    private AudioSource _playerAudio;
    private AudioClip _jumpAudio;
    private AudioClip _moveAudio;
    private AudioClip _deathFallAudio;
    private AudioClip _splatAudio;
    private AudioClip _SmackAudio;

    private int currentLane;
    private int targetLane;

    private void Start()
    {
        _playerAudio = _character.GetComponent<AudioSource>();

    }


}
