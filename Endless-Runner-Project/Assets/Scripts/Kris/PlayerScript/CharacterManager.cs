using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    //The character and Parent.
    //Done this way because _characterParent can be rotated, which allows the _character's position to change 
    //locally to the Parent's rotation. 
    [SerializeField] private GameObject _characterParent;
    [SerializeField] private GameObject _character;
    [SerializeField] private int numberOfLanes;

    private int[] laneBoundaries;
    private int evenLaneBias = (int)movementDirections.left;

    //The player's Audio source and AudioClips. Self-explanatory names.
    private AudioSource _playerAudioS;
    private AudioClip _jumpAudio;
    private AudioClip _moveAudio;
    private AudioClip _deathFallAudio;
    private AudioClip _splatAudio;
    private AudioClip _SmackAudio;

    private int direction = (int)directions.north;                      //Player's current direction

    private int currentLane;                                            //Player's current lane
    private int targetLane = 0;                                         //Which lane the player is trying to switch to.
    private bool transitioning = false;                                 //Used to know if the character is Switching Lanes (For Collision System).
    private bool lockLaneSwitch = false;
    private Vector3 playerPosition = new Vector3(0,0,0);                //Player's current position
    private Vector3 playerTargetPosition = new Vector3(0, 0, 0);        //Which position the player is trying to reach.
    
    private Quaternion currentrotation = Quaternion.Euler(0, 0, 0);     //Where the character is currently rotated
    private Quaternion targetrotation = Quaternion.Euler(0, 0, 0);      //The rotation the character is trying to reach.
    
    private int[] rotationIndex = new int[]                             //Index of all rotations. Used this to avoid an unnecessarily big Switch statement.
    { 0, 90, 180, 270 };        

    private float[] interpolationSpeedIndex = new float[] 
    { 0.125f, 0.25f, 0.5f };

    private float interpolationSpeedLane = 0.25f;
    private float interpolationSpeedRotate = 0.25f;                           //Used for Linear Interpolation for smooth Lane moving and smooth rotation

    private int recentMove = (int)movementDirections.none;
    private float recentMoveTimer = 0;

    //Each possible player Direction
    public enum directions
    {
        north,       
        east,
        south,
        west,
        length,
    }

    public enum movementDirections
    {
        left,
        right,
        up,
        down,
        none,
    }

    public enum interpolationSpeeds
    {
        halfSpeed,
        normalSpeed,
        doubleSpeed,
    }
    private void Start()
    {
        this._playerAudioS = _character.GetComponent<AudioSource>();
        SetLaneLimit(this.numberOfLanes);

    }

    private void FixedUpdate()
    {
        UpdateCharacterData();

        if (this.playerPosition != this.playerTargetPosition)
        {
            Vector3 linearPlayerMove = Vector3.Lerp(this.playerPosition, this.playerTargetPosition, this.interpolationSpeedLane);
            this.playerPosition = linearPlayerMove;
            this._character.transform.localPosition = this.playerPosition;

            this.transitioning = true;

            if (this.currentLane < (this.targetLane - 1))
            {
                this.currentLane = this.targetLane - 1;
            }
            else if (this.currentLane > (this.targetLane + 1))
            {
                this.currentLane = this.targetLane + 1;
            }
        }

        else
        {
            this.currentLane = this.targetLane;
            this.transitioning = false;
        }


        if (this.currentrotation != this.targetrotation)
        {
            Quaternion linearPlayerRotate = Quaternion.Lerp(this.currentrotation, this.targetrotation, this.interpolationSpeedRotate);
            this.currentrotation = linearPlayerRotate;
            this._characterParent.transform.localRotation = this.currentrotation;
        }
    }
    private void UpdateCharacterData()
    {
        this.playerTargetPosition = new Vector3(this.targetLane, 0, 0);
        this.targetrotation = Quaternion.Euler(0, rotationIndex[this.direction], 0);
    }

    private void CheckLaneBounds()
    {
        if (this.targetLane < this.laneBoundaries[(int)movementDirections.left])
        {
            this.targetLane = this.laneBoundaries[(int)movementDirections.left];
        }
        else if (this.targetLane > this.laneBoundaries[(int)movementDirections.right])
        {
            this.targetLane = this.laneBoundaries[(int)movementDirections.right];
        }

    }

    public void SetLaneLimit(int LaneLimit)
    {
        this.numberOfLanes = LaneLimit;
        if (this.numberOfLanes < 1)
        {
            this.numberOfLanes = 1;
        }
        if (this.numberOfLanes % 2 == 0)
        {
            int Boundary = this.numberOfLanes / 2;
            this.laneBoundaries = new int[] { -(Boundary - 1), Boundary };
        }
        else if (this.numberOfLanes % 2 == 1)
        {
            int Boundary = ((this.numberOfLanes - 1) / 2);
            this.laneBoundaries = new int[] { -Boundary, Boundary };
        }
        CheckLaneBounds();
    }

    public void SetLanePos(int lanepos)
    {
        this.targetLane = lanepos;
    }

    public void LockLaneSwitching(bool LockLanes)
    {
        this.lockLaneSwitch = LockLanes;
    }

    public void Move(int Direction)
    {
        if (!this.lockLaneSwitch)
        {
            int previousTargetLane = this.targetLane;
            switch (Direction)
            {
                case (int)movementDirections.left:
                    this.targetLane -= 1;
                    break;

                case (int)movementDirections.right:
                    this.targetLane += 1;
                    break;
            }
            CheckLaneBounds();
            if(!(this.targetLane == previousTargetLane))
            {
                this.recentMove = Direction;
            }
        }
    }

    public void Rotate(int whichway)
    {
        switch (whichway)
        {
            case (int)movementDirections.left:
                this.direction -= 1;
                if (this.direction < (int)directions.north)
                {
                    this.direction = (int)directions.west;
                }
                break;
            case (int)movementDirections.right:
                this.direction += 1;
                if (this.direction > (int)directions.west)
                {
                    this.direction = (int)directions.north;
                }
                break;
        }
    }

    public bool GetPlayerTransitioningState()
    {
        return this.transitioning;
    }

    public int GetPlayerLaneCurrent()
    {
        return this.currentLane;
    }

    public int GetPlayerLaneTarget()
    {
        return this.targetLane;
    }

    public int GetPlayerDirection()
    {
        return this.direction;
    }

}
