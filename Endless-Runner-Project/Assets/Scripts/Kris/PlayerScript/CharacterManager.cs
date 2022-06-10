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
    private bool transitioning = true;
    private float playerLanePos = 0;
    private int targetLane = 0;
    private Vector3 playerPosition = new Vector3(0,0,0);
    private Vector3 playerTargetPosition = new Vector3(0, 0, 0);
    private float interpolationSpeed = 0.25f;
    private Quaternion currentrotation = Quaternion.Euler(0, 0, 0);
    private Quaternion targetrotation = Quaternion.Euler(0, 0, 0);
    private float yRotation = 0;



    public enum directions{
        north,       
        east,
        south,
        west,
        length
    }

    public void Rotate(int whichway)
    {
        print("Bloop! "+whichway);
        switch (whichway)
        {
            case 0:

                this.direction -= 1;
                this.yRotation -= 30f;
                
                if (this.direction < 0)
                {
                    this.direction = (int)directions.length - 1;
                    
                }

                break;
            case 1:
                this.direction += 1;
                //this.yRotation += 30;
                if (this.direction >= (int)directions.length)
                {
                    this.direction = 0;
                    
                }
                break;

        }
        
    }


    public void MoveLeft()
    {
        targetLane -= 1;
    }

    public void MoveRight()
    {
        targetLane += 1;
    }

    public bool GetPlayerTransitioningState()
    {
        return this.transitioning;
    }



    private void UpdateLanesPos(int Direction)
    {
        switch (direction)
        {
            case (int)directions.north:
                //this.playerPosition = new Vector3(this.playerLanePos, 0, 0);
                this.playerTargetPosition = new Vector3(this.targetLane, 0, 0);
                
                break;
            case (int)directions.south:
                //this.playerPosition = new Vector3(-this.playerLanePos, 0, 0);
                this.playerTargetPosition = new Vector3(-this.targetLane, 0, 0);
                
                break;
            case (int)directions.east:
                //this.playerPosition = new Vector3(0, 0, this.playerLanePos);
                this.playerTargetPosition = new Vector3(0, 0, this.targetLane);
                
                break;
            case (int)directions.west:
                //this.playerPosition = new Vector3(0, 0, -this.playerLanePos);
                this.playerTargetPosition = new Vector3(0, 0, -this.targetLane);
          
                break;

             
        }
        this.targetrotation = new Quaternion(0, yRotation, 0, 0);

    }

    private void FixedUpdate()
    {
        UpdateLanesPos(this.direction);
        if (this.playerPosition != this.playerTargetPosition)
        {

            
            Vector3 linearPlayerMove = Vector3.Lerp(this.playerPosition, this.playerTargetPosition, this.interpolationSpeed);
            this.playerPosition = linearPlayerMove;
            this._character.transform.position = this.playerPosition;
            this.transitioning = true;

        }
        else
        {
            this.transitioning = false;
        }


            Quaternion linearPlayerRotate = Quaternion.Lerp(this.currentrotation, this.targetrotation, 0.01f);
            
           this.currentrotation = linearPlayerRotate;
           this._character.transform.rotation = this.targetrotation;
        print(this.targetrotation);


        /*
            print("PlayerPos: " + this.playerPosition);
            print("TargetPos: " + this.playerTargetPosition);

        switch (this.transitioning)
        {
            case true:
                Vector3 linearPlayerMove = Vector3.Lerp(this.playerPosition, this.playerTargetPosition, this.interpolationSpeed);
                this.playerPosition = linearPlayerMove;
                this._character.transform.position = this.playerPosition;

                break;


            case false:

                break;
        }
        */
        /*
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
        */
    }

    private void Start()
    {
        _playerAudio = _character.GetComponent<AudioSource>();

    }




}
