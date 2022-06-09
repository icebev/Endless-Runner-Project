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

    private int direction = (int)directions.north;
    private int currentLane;
    private bool transitioning;
    private float playerLanePos = 0;
    private int targetLane = 0;
    private Vector3 playerPosition = new Vector3(0,0,0);
    private Vector3 playerTargetPosition = new Vector3(0, 0, 0);
    private float interpolationSpeed = 0.1f;

    public enum directions{
        north,
        south,
        east,
        west
    }

    public void MoveLeft()
    {
        targetLane += 1;
    }

    public void MoveRight()
    {
        targetLane -= 1;
    }

    private void UpdateLanesPos(int Direction)
    {
        switch (direction)
        {
            case (int)directions.north:
                this.playerPosition = new Vector3(this.playerLanePos, 0, 0);
                this.playerTargetPosition = new Vector3(this.targetLane, 0, 0);
                break;
            case (int)directions.south:
                this.playerPosition = new Vector3(-this.playerLanePos, 0, 0);
                this.playerTargetPosition = new Vector3(-this.targetLane, 0, 0);
                break;
            case (int)directions.east:
                this.playerPosition = new Vector3(0, 0, this.playerLanePos);
                this.playerTargetPosition = new Vector3(0, 0, this.targetLane);
                break;
            case (int)directions.west:
                this.playerPosition = new Vector3(0, 0, -this.playerLanePos);
                this.playerTargetPosition = new Vector3(0, 0, -this.targetLane);
                break;


        }

    }

    private void FixedUpdate()
    {
        if (this.currentLane != this.targetLane)
        {
            this.transitioning = true;
            UpdateLanesPos(this.direction);
            Vector3 linearPlayerMove = Vector3.Lerp(this.playerPosition, this.playerTargetPosition, this.interpolationSpeed);
            
            this.playerPosition = linearPlayerMove;
            this._character.transform.position = this.playerPosition;
            if (this.playerPosition == this.playerTargetPosition)
            {
                this.transitioning = false;
                this.currentLane = this.targetLane;
                print(this.currentLane + " CurrentLane");
                print(this.targetLane + " targetLane");
                print(this.playerPosition);

            }

        }
    }

    private void Start()
    {
        _playerAudio = _character.GetComponent<AudioSource>();

    }




}