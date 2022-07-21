using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 16/07/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * Added max fall velocity and ensured the player doesn't fall through the ground.
 * Extend the forward raycasts so at high speed the player still interacts with the obstacles.
 * 
 */
/// <summary>
/// A class for implementing character movement and collision.
/// </summary>
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
    private float previousLane = 0;
    private bool transitioning = false;                                 //Used to know if the character is Switching Lanes (For Collision System).
    [SerializeField] private bool lockLaneSwitch;
    //private bool lockLaneSwitch = false;
    //private Vector3 playerPosition = new Vector3(0, 0, 0);                //Player's current position
    //private Vector3 playerTargetPosition = new Vector3(0, 0, 0);        //Which position the player is trying to reach.

    private Vector3 playerPosition = new Vector3(0, 0, 0);
    private Vector3 playerPositionRelative = new Vector3(0, 0, 0);
    //private Vector3 playerTargetPosition = new Vector3(0, 0, 0);

    private Quaternion currentrotation = Quaternion.Euler(0, 0, 0);     //Where the character is currently rotated
    private Quaternion targetrotation = Quaternion.Euler(0, 0, 0);      //The rotation the character is trying to reach.
    
    private int[] rotationIndex = new int[]                             //Index of all rotations. Used this to avoid an unnecessarily big Switch statement.
    { 0, 90, 180, 270 };

    private float[] interpolationSpeedIndex = new float[] { 0.125f, 0.25f, 0.5f };

    [SerializeField] private float interpolationSpeedLane = 0.25f;
    [SerializeField] private float interpolationSpeedRotate = 0.25f;                           //Used for Linear Interpolation for smooth Lane moving and smooth rotation

    private Vector3[] LeftLocal = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1) };
    private Vector3[] RightLocal = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, 1) };
    private Vector3[] ForwardLocal = new Vector3[] { new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0) };

    private movementDirections recentMove = movementDirections.none;
    private movementDirections recentMovePrevious;
    private float recentMoveScheduled = 0;

    private float recentMoveTimer = 0;
    private float recentMoveMaxTime = 0.18f;
    private float timerSpeed = 0.02f; //This equates to 1 second.

    [SerializeField] private playerStates currentState = playerStates.grounded;
    [SerializeField] private animationStates animationState = animationStates.RegularRun;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private bool doPhysics = true;
    private bool isGrounded = true;
    private float playerYSpeed = 0;
    private float playerMaxDownwardVelocity = -0.75f;
    [SerializeField] private float gravity = 0.01f;
    [SerializeField] private float jumpVelocity = 0.25f;
    private float playerRaycastSizeDown = 1.02f;
    private float playerRaycastSizeSide = 0.28f;
    private float playerRaycastSizeFront = 0.84f;

    [SerializeField] private bool IgnoreGroundCollision = false;

    private RaycastHit GroundRayHit;
    private RaycastHit FrontUpperRayHit;

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
        leftDouble,
        rightDouble,
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
        crouching
        
    }

    public enum animationStates
    {
        Roll,
        Fall,
        Jump,
        LaneSwitch,
        RegularRun,


    }

    public enum WhichRay
    {
        LeftUp,             //Left Up
        LeftDown,             //Left Down
        LeftBoth,              //Left Both
        FrontUp,             //Front Up
        FrontDown,             //Front Down
        FrontBoth,              //Front Both
        RightUp,             //Right Up
        RightDown,             //Right Down
        RightBoth,              //Right Both
        Up,              //Up
        Down,              //Down
                       
    }

    private LoadingManager _loadingManager;

    private void Start()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        this._loadingManager = _loadingManagerObject.GetComponent<LoadingManager>();
        this._playerAudioS = _character.GetComponent<AudioSource>();
        SetLaneLimit(this.numberOfLanes);

    }

    private void FixedUpdate()
    {
        CollidableObjects.TickCooldowns();
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

    public void SetLanePos(float lanepos)
    {
        this.targetLane = lanepos * this.LaneSize;
        CheckLaneBounds();
    }
    public void AddLanePos(float lanepos)
    {
        this.targetLane += lanepos * this.LaneSize;
        CheckLaneBounds();
    }

    public void LockLaneSwitching(bool LockLanes)
    {
        this.lockLaneSwitch = LockLanes;
    }

    public void Move(movementDirections Direction, int Amount)
    {
        if (!this.lockLaneSwitch && this.currentState != playerStates.crouching)
        {

            this.previousLane = this.targetLane;
            switch (Direction)
            {
                case movementDirections.left:

                    if (this.transitioning)
                    {
                        this.recentMoveScheduled -= this.LaneSize * Amount;
                    }
                    else
                    {
                        this.targetLane -= this.LaneSize * Amount;
                    }
                    
                    
                    break;

                case movementDirections.right:

                    if (this.transitioning)
                    {
                        this.recentMoveScheduled += this.LaneSize * Amount;
                    }
                    else
                    {
                        this.targetLane += this.LaneSize * Amount;
                    }

                    break;
            }
            this.recentMove = Direction;
            this.CheckLaneBounds();
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


            float linearPlayerXMove = Mathf.MoveTowards(this.playerPosition.x, this.targetLane, this.interpolationSpeedLane);

            //float linearPlayerXMove = Mathf.Lerp(this.playerPosition.x, this.targetLane, this.interpolationSpeedLane);
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

            if (Mathf.Abs(this.playerPosition.x - this.targetLane) < 0.18f)
            {
                this.playerPosition.x = this.targetLane;
            }
        }

        else
        {
            this.currentLane = this.targetLane;
            this.transitioning = false;
            if(this.recentMoveScheduled != 0)
            {
                this.targetLane += this.recentMoveScheduled;
                this.recentMoveScheduled = 0;
                this.CheckLaneBounds();
            }

        }

        if (this.currentrotation != this.targetrotation)
        {
            Quaternion linearPlayerRotate = Quaternion.Lerp(this.currentrotation, this.targetrotation, this.interpolationSpeedRotate);
            this.currentrotation = linearPlayerRotate;
            this._characterParent.transform.rotation = this.currentrotation;
        }
        
        
    }


    //This is where the Physics are updated. Within the physics are the raycasts for collision and charactermovement.
    private void UpdatePhysics()
    {
        if (!doPhysics) return; //Check to see if Physics are enabled.

        ApplyGravity();
        
        //Apply Speed to the player.
        this.playerPosition.y += this.playerYSpeed;

        //Get the player's Local Position from the rotated world position.
        this.playerPositionRelative = this._characterParent.transform.InverseTransformDirection(this.playerPosition);

        RaycastHit TempGroundRayHit; //Used to Nullify a RayHit if nothing is detected

        //Downwards raycast.
        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1f, -this.playerPositionRelative.z), transform.TransformDirection(Vector3.down), out TempGroundRayHit, this.playerRaycastSizeDown);
        this.GroundRayHit = TempGroundRayHit; //Setting or Nullyfing the ground ray hit.

        ////////Refactor this.
        if (TempGroundRayHit.collider != null) //If it isn't Null send collision information to the collision handler.
        {
            this.HandleCollision(TempGroundRayHit, WhichRay.Down);
        }
        else
        {
            this.isGrounded = false; //If it is null, the character is not grounded.
        }
        ///////


        //Check to see if the player is moving left or right.
        if (this.transitioning)
        {
            RaycastHit SideHitUpper; //Raycasts hit info for the sides. Upper and Lower.
            RaycastHit SideHitLower;

            //Checks the recent move to see whether or not the raycast will be on the left or right.
            //Kris - Please change this to be a check of the character's x pos in relation to Target Pos to solve info
           
            switch (this.recentMove)
            {
                case movementDirections.left:
                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                    Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                    Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);

                    break;

                case movementDirections.right:

                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                    Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                    Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);

                    break;
                default:
                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                    Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);
                    break;
            }

            //Handles the Side collisions. This detects if top and bottom raycasts were hit at the same time.
            if ((SideHitUpper.collider != null) && (SideHitLower.collider != null))
            {
                this.HandleCollision(SideHitUpper, this.GetDirectionRay(2));
                print("SIDE HIT BOTH!");
            }
            //This detects if only the top raycast was hit 
            else if ((SideHitUpper.collider != null) && (SideHitLower.collider == null))
            {
                this.HandleCollision(SideHitUpper, this.GetDirectionRay(0));
                print("SIDE HIT UPPER!");
            }
            //This detects if only the bottom raycast was hit 
            else if ((SideHitUpper.collider == null) && (SideHitLower.collider != null))
            {
                this.HandleCollision(SideHitLower, this.GetDirectionRay(1));
                print("SIDE HIT LOWER!");
            }



        }


        // Front raycast hit detection
        RaycastHit FrontHitUpper;
        RaycastHit FrontHitLower;


        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction], out FrontHitUpper, this.playerRaycastSizeFront);
        Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeFront, Color.red);


        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction], out FrontHitLower, this.playerRaycastSizeFront);
        Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeFront, Color.red);

        if ((FrontHitUpper.collider != null) && (FrontHitLower.collider != null) && (this.currentState != playerStates.crouching))
        {
            this.HandleCollision(FrontHitUpper, WhichRay.FrontBoth);
            //print("FRONT HIT BOTH!");
        }
        else if ((FrontHitUpper.collider != null) && (FrontHitLower.collider == null) && (this.currentState != playerStates.crouching))
        {
            this.HandleCollision(FrontHitUpper, WhichRay.FrontUp);
            //print("FRONT HIT UPPER!");
        }
        else if (((FrontHitUpper.collider == null) || (this.currentState == playerStates.crouching)) && (FrontHitLower.collider != null))
        {
            this.HandleCollision(FrontHitLower, WhichRay.FrontDown);
            print("FRONT HIT LOWER!");
        }




        this._character.transform.localPosition = this.playerPosition;

    }

    private void HandleCollision(RaycastHit rayHit, WhichRay whichRay)
    {
        //print(rayHit);
        //print("HANDLING COLLISION");
        CollidableObjects gameCollision = rayHit.collider.GetComponent<CollidableObjects>();
        if (gameCollision != null)
        {
            gameCollision.DoCollision(whichRay);
            //print("DO COLLISION");
        }

    }



    private void ApplyGravity()
    {
        if (!this.isGrounded || this.IgnoreGroundCollision == true)
        {
            this.playerYSpeed -= this.gravity;
            if (this.playerYSpeed < this.playerMaxDownwardVelocity )
            {
                this.playerYSpeed = this.playerMaxDownwardVelocity;
            }

            //print("Player Y Velocity " + this.playerYSpeed);
        }
    }

    public void Slide()
    {
        if (this.isGrounded)
        {
            this.playerAnimator.Play("Slide");
            this.currentState = playerStates.crouching;
            StartCoroutine(EndSlide(1.0f));
        }
    }

    public IEnumerator EndSlide(float slideTime)
    {
        yield return new WaitForSeconds(slideTime);

        this.currentState = playerStates.grounded;
    }

    public void Jump()
    {
        if (this.isGrounded)
        {
            this.currentState = playerStates.grounded;
            this.playerYSpeed += this.jumpVelocity;
            this.isGrounded = false;
        }
    }

    private WhichRay GetDirectionRay(int Index)
    {
        switch (Index)
        {

            case 0:
                if (this.recentMove == movementDirections.left || this.recentMove == movementDirections.leftDouble)
                {
                    return (WhichRay.LeftUp);
                }
                else if (this.recentMove == movementDirections.right || this.recentMove == movementDirections.rightDouble)
                {
                    return (WhichRay.RightUp);
                }
                break;

            case 1:
                if (this.recentMove == movementDirections.left || this.recentMove == movementDirections.leftDouble)
                {
                    return (WhichRay.LeftDown);
                }
                else if (this.recentMove == movementDirections.right || this.recentMove == movementDirections.rightDouble)
                {
                    return (WhichRay.RightDown);
                }
                break;

            case 2:
                if (this.recentMove == movementDirections.left || this.recentMove == movementDirections.leftDouble)
                {
                    return (WhichRay.LeftBoth);
                }
                else if (this.recentMove == movementDirections.right || this.recentMove == movementDirections.rightDouble)
                {
                    return (WhichRay.RightBoth);
                }
                break;
        }
        return WhichRay.LeftBoth;
    }

    public void GroundedCharacter()
    {
        //print("groundedChar");

        this.isGrounded = true;
        //if (this.playerYVelocity < 0)
        //{
            this.playerYSpeed = 0;
        //}
        this.playerPosition.y = this.GroundRayHit.point.y - 0.02f;
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

    public void SetPlayerLaneTarget(int lane)
    {
        this.targetLane = lane * this.LaneSize;
    }

    public void AddPlayerLaneTarget(int lane)
    {
        this.targetLane += lane * this.LaneSize;
    }

    public void PreviousLaneReturn()
    {
        this.targetLane = this.previousLane;
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

    private int loading = 0;

   public void RestartLevel()
    {
        if(loading == 0)
        {
            loading += 1;
            this._loadingManager.LoadGameScene1(3, true, 0);
        }
        
    }
    public void MainMenu()
    {
        if (loading == 0)
        {
            loading += 1;
            this._loadingManager.LoadGameScene1(2, true, 0);
        }
    }


}
