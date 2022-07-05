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

    private float[] laneBoundaries;
    //private int evenLaneBias = (int)movementDirections.left;
    [SerializeField] private int evenLaneBias;

    //The player's Audio source and AudioClips. Self-explanatory names.
    private AudioSource _playerAudioS;
    private AudioClip _jumpAudio;
    private AudioClip _moveAudio;
    private AudioClip _deathFallAudio;
    private AudioClip _splatAudio;
    private AudioClip _SmackAudio;

    private int direction = (int)directions.north;                      //Player's current direction

    [SerializeField] private float LaneSize;
    private float currentLane;                                            //Player's current lane
    private float targetLane = 0;                                         //Which lane the player is trying to switch to.
    private bool transitioning = false;                                 //Used to know if the character is Switching Lanes (For Collision System).
    [SerializeField] private bool lockLaneSwitch;
    //private bool lockLaneSwitch = false;
    //private Vector3 playerPosition = new Vector3(0, 0, 0);                //Player's current position
    //private Vector3 playerTargetPosition = new Vector3(0, 0, 0);        //Which position the player is trying to reach.

    private Vector3 playerPosition = new Vector3(0, 0, 0);
    //private Vector3 playerTargetPosition = new Vector3(0, 0, 0);

    private Quaternion currentrotation = Quaternion.Euler(0, 0, 0);     //Where the character is currently rotated
    private Quaternion targetrotation = Quaternion.Euler(0, 0, 0);      //The rotation the character is trying to reach.
    
    private int[] rotationIndex = new int[]                             //Index of all rotations. Used this to avoid an unnecessarily big Switch statement.
    { 0, 90, 180, 270 };

    private float[] interpolationSpeedIndex = new float[] { 0.125f, 0.25f, 0.5f };

    private float interpolationSpeedLane = 0.25f;
    private float interpolationSpeedRotate = 0.25f;                           //Used for Linear Interpolation for smooth Lane moving and smooth rotation

    private Vector3[] LeftLocal = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1) };
    private Vector3[] RightLocal = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, 1) };
    private Vector3[] ForwardLocal = new Vector3[] { new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0) };

    private movementDirections recentMove = movementDirections.none;
    private movementDirections recentMovePrevious;

    private float recentMoveTimer = 0;
    private float recentMoveMaxTime = 0.18f;
    private float timerSpeed = 0.02f; //This equates to 1 second.

    private bool doPhysics = true;
    private bool isGrounded = true;
    private float playerYVelocity = 0;
    private float gravity = 0.01f;
    private float jumpHeight = 0.25f;
    private float playerRaycastSizeDown = 0.42f;
    private float playerRaycastSizeSide = 0.28f;

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

    public enum playerStates
    {
        grounded,
        jumping,
        quickfall,
        
    }

    public enum AnimationState
    {
        Roll,
        Fall,
        Jump,
        LaneSwitch,
        RegularRun,


    }

    private void Start()
    {
        this._playerAudioS = _character.GetComponent<AudioSource>();
        SetLaneLimit(this.numberOfLanes);

    }

    private void FixedUpdate()
    {
        UpdateCharacterData();
        UpdateDoubleTapTimer();
        UpdateLanePositionAndRotation();
        UpdatePhysics();

        //GroundCheck();

    }
    private void UpdateCharacterData()
    {
        //this.playerTargetPosition = new Vector3(this.targetLane, this._character.transform.localPosition.y, 0);
        //this.playerTargetPosition = new Vector3(this.targetLane, this.playerPosition.y, 0);
        this.targetrotation = Quaternion.Euler(0, rotationIndex[this.direction], 0);
    }

    public void SetEvenLaneBias(int Bias)
    {
        this.evenLaneBias = Bias;
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
            float Boundary = (this.numberOfLanes / 2) * this.LaneSize;

            switch (evenLaneBias)
            {
                case (int)movementDirections.left:
                    this.laneBoundaries = new float[] { -Boundary, (Boundary - 1)};
                    break;

                case (int)movementDirections.right:
                    this.laneBoundaries = new float[] { -(Boundary - 1), Boundary };
                    break;
            }   
        }
        else if (this.numberOfLanes % 2 == 1)
        {
            float Boundary = ((this.numberOfLanes - 1) / 2) * this.LaneSize;
            this.laneBoundaries = new float[] { -Boundary, Boundary };
        }
        CheckLaneBounds();
    }

    public void SetLanePos(int lanepos)
    {
        this.targetLane = lanepos;
        CheckLaneBounds();
    }

    public void LockLaneSwitching(bool LockLanes)
    {
        this.lockLaneSwitch = LockLanes;
    }

    public void Move(movementDirections Direction, int Amount)
    {
        if (!this.lockLaneSwitch)
        {
            float previousTargetLane = this.targetLane;
            switch (Direction)
            {
                case movementDirections.left:
                    this.targetLane -= this.LaneSize * Amount;
                    
                    break;

                case movementDirections.right:
                    this.targetLane += this.LaneSize * Amount;
                    break;
            }
            this.recentMove = Direction;
            CheckLaneBounds();
            /*
            if(!(this.targetLane == previousTargetLane))
            {
                this.recentMovePrevious = this.recentMove;
                this.recentMove = Direction;
                if(this.recentMoveTimer > 0 && this.recentMove == this.recentMovePrevious)
                {
                    this.interpolationSpeedLane = this.interpolationSpeedIndex[(int)interpolationSpeeds.doubleSpeed];
                }
                else
                {
                    this.interpolationSpeedLane = this.interpolationSpeedIndex[(int)interpolationSpeeds.normalSpeed];
                    this.recentMoveTimer = this.recentMoveMaxTime;
                }
            }
            */
        }
    }

    public void Rotate(TurnDirection turnDirection)
    {
        switch (turnDirection)
        {
            case TurnDirection.Left:
                this.direction -= 1;
                if (this.direction < (int)directions.north)
                {
                    this.direction = (int)directions.west;
                }
                break;
            case TurnDirection.Right:
                this.direction += 1;
                if (this.direction > (int)directions.west)
                {
                    this.direction = (int)directions.north;
                }
                break;
        }

    }

    private void UpdateDoubleTapTimer()
    {
        if (this.recentMoveTimer >= 0)
        {
            this.recentMoveTimer -= this.timerSpeed;
        }
    }

    private void UpdateLanePositionAndRotation()
    {

        if (this.playerPosition.x != this.targetLane)
        {
            //Vector3 linearPlayerMove = Vector3.Lerp(this.playerPosition, this.playerTargetPosition, this.interpolationSpeedLane);

            float linearPlayerXMove = Mathf.Lerp(this.playerPosition.x, this.targetLane, this.interpolationSpeedLane);
            this.playerPosition.x = linearPlayerXMove;
            //this.playerPosition.x = linearPlayerXMove;
            

            this.transitioning = true;

            if (this.currentLane < (this.targetLane - this.LaneSize))
            {
                this.currentLane = this.targetLane - this.LaneSize;
            }
            else if (this.currentLane > (this.targetLane + this.LaneSize))
            {
                this.currentLane = this.targetLane + this.LaneSize;
            }

            if (Mathf.Abs(this.playerPosition.x - this.targetLane) < 0.01f)
            {
                this.playerPosition.x = this.targetLane;
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
            this._characterParent.transform.rotation = this.currentrotation;
        }
        
    }

    private void UpdatePhysics()
    {

        print(targetLane);
        if (doPhysics) { 
            
            ApplyGravity();
            
            this.playerPosition.y += this.playerYVelocity;
            
            Vector3 relativePlayerPos = this._characterParent.transform.InverseTransformDirection(this.playerPosition);  
            RaycastHit GroundHit;
            //Vector3 relativeRayCastX = this._characterParent.transform.InverseTransformDirection(Vector3.left);
            //Vector3 relativeRayCastXa = this._characterParent.transform.InverseTransformDirection(Vector3.right);
            //Vector3 relativeRayCastZ = this._characterParent.transform.InverseTransformDirection(Vector3.forward);

            //Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), transform.TransformDirection(Vector3.down), out GroundHit, 0.42f);
            //Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), transform.TransformDirection(Vector3.down) * 0.42f, Color.red);

            Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), transform.TransformDirection(Vector3.down), out GroundHit, this.playerRaycastSizeDown);
            
            if (GroundHit.collider != null)
            {
                this.isGrounded = true;
                if (this.playerYVelocity < 0)
                {
                    this.playerYVelocity = 0;
                }
                this.playerPosition.y = GroundHit.point.y - 0.02f ;
                Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), transform.TransformDirection(Vector3.down) * this.playerRaycastSizeDown, Color.green);
                //this.playerYVelocity = 0;
            }
            else 
            {
                this.isGrounded = false;
                Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), transform.TransformDirection(Vector3.down) * this.playerRaycastSizeDown, Color.red);
            }

            if (this.transitioning)
            {
                RaycastHit SideHitUpper;
                RaycastHit SideHitLower;

                switch (this.recentMove)
                {
                    case movementDirections.left:
                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.LeftLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.LeftLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                        Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                        Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);

                        break;

                    case movementDirections.right:

                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.RightLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.RightLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                        Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                        Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);

                        break;
                    default:
                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.RightLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                        Physics.Raycast(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.RightLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);
                        break;
                }

                if(SideHitUpper.collider != null)
                {
                    
                    
                    
                    //this._characterParent.gameObject.SetActive(false);
                    //this.gameObject.SetActive(false);
                }


            }
           

            Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 0.4f, -relativePlayerPos.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeSide, Color.red);
            Debug.DrawRay(new Vector3(relativePlayerPos.x, this.playerPosition.y + 1.2f, -relativePlayerPos.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeSide, Color.red);

            this._character.transform.localPosition = this.playerPosition;
        }
    }

    private void ApplyGravity()
    {
        if (!this.isGrounded)
        {
            this.playerYVelocity -= gravity;
        }
    }

    public void Jump()
    {
        if (this.isGrounded)
        {
            
            this.playerYVelocity += jumpHeight;
        }
    }

    public bool GetPlayerTransitioningState()
    {
        return this.transitioning;
    }

    public float GetPlayerLaneCurrent()
    {
        return (this.currentLane / this.LaneSize);
    }

    public float GetPlayerLaneTarget()
    {
        return (this.targetLane / this.LaneSize);
    }

    public float GetPlayerPositionXCurrent()
    {
        return (this.currentLane);
    }

    public float GetPlayerPositionXTarget()
    {
        return (this.targetLane);
    }

    public int GetPlayerDirection()
    {
        return this.direction;
    }

    public int GetEvenLaneBias()
    {
        return this.evenLaneBias;
    }

    public int GetNumberOfLanes()
    {
        return this.numberOfLanes;
    }

    public float GetLaneBoundaries(int direction)
    {
        return this.laneBoundaries[direction];
    }
}
