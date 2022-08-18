using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 18/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * Added max fall velocity and ensured the player doesn't fall through the ground.
 * Extend the forward raycasts so at high speed the player still interacts with the obstacles.
 * Fixed glitch where you can slide through two-tall obstacles.
 * Refactored the code to utilise more guard-clauses.
 * Implemented State Machine
 * Added character Animation Communication
 * Commenting
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

    //The player's Audio source and AudioClips. Self-explanatory names. This is old legacy and is Unused.
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

    private Vector3 playerPosition = new Vector3(0, 0, 0);              //The player's X Y Z position
    private Vector3 playerPositionRelative = new Vector3(0, 0, 0);      //Relative position by inverting the Player Position. This is a fix for the rotation issue.

    private Quaternion currentrotation = Quaternion.Euler(0, 0, 0);     //Where the character is currently rotated
    private Quaternion targetrotation = Quaternion.Euler(0, 0, 0);      //The rotation the character is trying to reach.
    
    private int[] rotationIndex = new int[]                             //Index of all rotations. Used this to avoid an unnecessarily big Switch statement.
    { 0, 90, 180, 270 };

    private float[] interpolationSpeedIndex = new float[] { 0.125f, 0.25f, 0.5f }; //Unused, the character used to move faster depending on the type of movement.

    [SerializeField] private float interpolationSpeedLane = 0.25f;  //Used for Linear Interpolation for smooth Lane moving and smooth rotation
    [SerializeField] private float interpolationSpeedRotate = 0.25f;  

    //These "Left Local", "Right Local", "Forward Local" was to fix a bug where the raycasts would rotate the wrong way.
    //These specifically have values so that the raycasts will rotate correctly with the character.
    //No clue why this is the case, but this fix works well.
    private Vector3[] LeftLocal = new Vector3[] { new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1) };
    private Vector3[] RightLocal = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, 1) };
    private Vector3[] ForwardLocal = new Vector3[] { new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1), new Vector3(-1, 0, 0) };

    private movementDirections recentMove = movementDirections.none; //Recent move is used to help create certain raycasts.
    private movementDirections recentMovePrevious; //Unused in the final code, kept just in case.
    private float recentMoveScheduled = 0;  //Value that's added to the TargetLane when the scheduled movement is available.

    [SerializeField] private playerStates currentState = playerStates.grounded;  //Current state, used mostly for Animations.
    [SerializeField] private Animator playerAnimator; //The animator for the runner.
    private CharacterAnimManager animManager; //The animation manager to perform animations.
    [SerializeField] private CharacterMovementSFXPlayer characterMovementSFXPlayer; //The sound effects for the character movement.

    [SerializeField] private bool doPhysics = true; //Whether or not physics should be performed (Always set to true, but can be disabled
    private bool isGrounded = true; //Used to check if the character is grounded or not. This is important for most of the movement.
    private float playerYSpeed = 0; //The player's Y speed that adjusts their Y position.
    private float playerMaxDownwardVelocity = -0.75f; //Fall speed limit
    [SerializeField] private float gravity = 0.01f; //Gravity amount.
    [SerializeField] private float jumpVelocity = 0.25f; //Jump amount.
    private float playerRaycastSizeDown = 1.02f; //The sizes of the raycasts, for Down, Side, and Front.
    private float playerRaycastSizeSide = 0.28f;
    private float playerRaycastSizeFront = 0.84f;

    //The variables for Hopping up: including the Speed, total timer, and the timer tick.
    [SerializeField] private float hopUpSpeed = 0.1f;  
    private float hopUpTime = 0.1f;
    private float hopUpTick = 0.1f;

    //The variables for Sliding: including total timer, and the timer tick, and (unused) whether its scheduled.
    [SerializeField] private float slideTime = 2f;
    private float slideTick;
    private bool slideScheduled;

    //Toggle to see whether or not the player should be able to slide and move or not.
    [SerializeField] bool moveWhileSliding = false; 

    //The raycast hits for ground and front upper ray hit. Used so it is accessible throughout the script.
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
    //All the different movement directions that can be done. (None, LeftDouble, RightDouble are not used in the end.)
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

    //Unused Enum for the different types of interpolation speeds.
    public enum interpolationSpeeds
    {
        halfSpeed,
        normalSpeed,
        doubleSpeed,
    }

    //Enum to check the playerstates. Mainly used by the Character Animator.
    public enum playerStates
    {
        grounded,
        jumping,
        quickfall,
        crouching
    }

    //The different type of raycasts in the game. Used for the raycast system.
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

    private void Start()
    {
        this.hopUpTick = this.hopUpTime;
        this.slideTick = this.slideTime;
        this._playerAudioS = _character.GetComponent<AudioSource>();
        this.animManager = this.gameObject.GetComponent<CharacterAnimManager>();
        SetLaneLimit(this.numberOfLanes);
    }

    //Main Update loop for the Character Manager.
    private void FixedUpdate()
    {
        this.animManager.UpdateVelocity(this.playerYSpeed); //Updates the velocity in the Animation manager, so a specific animation can be done.
        CollidableObjects.TickCooldowns(); //Does the tick cool down on collisions so that they cant be activated multiple times.
        UpdateTimers(); //Updates the character manager's timers.
        //ScheduledMovements(); //legacy, scheduled movements is now unused.
        UpdateCharacterData(); //Updates the character's target rotation
        UpdateLanePositionAndRotation(); //Moves the character to the target position, and rotates them to their target rotation.
        UpdatePhysics(); //Updates physics.
        this.animManager.ManageAnimation(this.currentState); //Updates the animator manager to know the character's playerstate.
    }

    //Updates the target rotation based on the rotation index. This was a bigger function, but then was optimised.
    private void UpdateCharacterData() { this.targetrotation = Quaternion.Euler(0, rotationIndex[this.direction], 0); }

    //Old legacy code when the lane sizes were configurable.
    public void SetEvenLaneBias(int Bias) { this.evenLaneBias = Bias; }

    //Checks the lane's bounds so the character can't go out of bounds.
    private void CheckLaneBounds()
    {
        if (this.targetLane < this.laneBoundaries[(int)movementDirections.left])
        {
            this.targetLane = this.laneBoundaries[(int)movementDirections.left]; //Sets the lane bounds back if out.
        }
        else if (this.targetLane > this.laneBoundaries[(int)movementDirections.right])
        {
            this.targetLane = this.laneBoundaries[(int)movementDirections.right];//Sets the lane bounds back if out.
        }
    }

    //Sets the lane limit. Its slightly redundant now due to the fact the lanes don't adjust, but it's still calculated.
    public void SetLaneLimit(int LaneLimit)
    {
        this.numberOfLanes = LaneLimit; //The number of lanes are set by the lane limit.
        if (this.numberOfLanes < 1)
        {
            this.numberOfLanes = 1; //Sets the lane to 1 if its been set to less than 1.
        }
        if (this.numberOfLanes % 2 == 0) //If it's even, it'll generate an even lane.
        {
            float Boundary = (this.numberOfLanes / 2) * this.LaneSize; //Sets the bounding.

            switch (this.evenLaneBias)
            {
                case (int)movementDirections.left: //Adds the extra lane to the left or right depending on the selected direction.
                    this.laneBoundaries = new float[] { -Boundary, (Boundary - 1)};
                    break;

                case (int)movementDirections.right:
                    this.laneBoundaries = new float[] { -(Boundary - 1), Boundary };
                    break;
            }   
        }
        else if (this.numberOfLanes % 2 == 1) //If it's odd, it can just find out how many lanes to add to either side, shown below.
        {
            float Boundary = ((this.numberOfLanes - 1) / 2) * this.LaneSize; 
            this.laneBoundaries = new float[] { -Boundary, Boundary };
        }
        CheckLaneBounds(); //Check's the character's lanes so they go back into bounds.
    }

    public void SetLanePos(float lanepos) //Sets the character's lane position.
    {
        this.targetLane = lanepos * this.LaneSize;
        CheckLaneBounds(); //Fixes the character if they are outside the lane boundaries.
    }
    public void AddLanePos(float lanepos) //Add lane position
    {
        this.targetLane += lanepos * this.LaneSize;
        CheckLaneBounds(); //checks to see if the character went outside the lanes.
    }

    //Locks whether you can switch lanes or not, used for junctions.
    public void LockLaneSwitching(bool LockLanes) { this.lockLaneSwitch = LockLanes; }

    //Move function, used by the player controller script for moving left and right.
    public void Move(movementDirections Direction)
    {
        //Disallows movement if lane switching is locked, or sliding and you're not allowed to move while sliding.
        if (this.lockLaneSwitch || (this.currentState == playerStates.crouching && !this.moveWhileSliding)) return;
        
        this.previousLane = this.targetLane; //Sets the previous lane to target lane.
        switch (Direction)
        {
            case movementDirections.left: //Switch case for moving left.
                if (this.transitioning) //If you're currently moving it does a scheduled movement
                {
                    this.recentMoveScheduled -= this.LaneSize;
                }
                else //If not moving, you move immediately.
                {
                    this.targetLane -= this.LaneSize;
                }
                break;

            case movementDirections.right: //Switch case for moving right.
                if (this.transitioning)   //If you're currently moving it does a scheduled movement
                {
                    this.recentMoveScheduled += this.LaneSize;
                }
                else //If not moving, you move immediately.
                {
                    this.targetLane += this.LaneSize;
                }
                break;
        }
        this.recentMove = Direction; //Recent move is set to the direction you were heading.
        this.CheckLaneBounds(); //Checks the lanes bounds so you don't go outside.
        if (this.targetLane != this.playerPosition.x && !this.transitioning)
        {
            this.characterMovementSFXPlayer.PlayLaneChangeSound(); //Plays the switching lane sound when you do switch lanes.
        }
    }

    //Rotate the character.
    public void Rotate(TurnDirection turnDirection)
    {
        switch (turnDirection)
        {
            case TurnDirection.Left: //Rotates the character to the left
                this.direction -= 1;
                if (this.direction >= (int)directions.north) return; //If its less than north, it gets set to west so it scales over.
                this.direction = (int)directions.west;
                
                break;
            case TurnDirection.Right: //Rotates the character to the right
                this.direction += 1;
                if (this.direction <= (int)directions.west) return; //If its more than west, it gets set to north so it scales over.
                this.direction = (int)directions.north;
                break;
        }
    }

    //Updates the character's lane position and rotation.
    private void UpdateLanePositionAndRotation()
    {
        if (this.playerPosition.x != this.targetLane) //Checks to see if the player's position is not the same as target lane. 
        {
            //Moves the current player position towards the target position
            float linearPlayerXMove = Mathf.MoveTowards(this.playerPosition.x, this.targetLane, this.interpolationSpeedLane); 
            this.playerPosition.x = linearPlayerXMove;

            this.transitioning = true; //Used to identify if the character is moving or not.

            if (this.currentLane < (this.targetLane - this.LaneSize)) //Sets the Current Lane based on your current target lane.
            {                                                         //It's done like this because current lane is only set when not transitioning,
                this.currentLane = this.targetLane - this.LaneSize;   //but the player could keep transitioning over and over without the current lane
            }                                                         //being set. This fixes it.
            else if (this.currentLane > (this.targetLane + this.LaneSize))
            {
                this.currentLane = this.targetLane + this.LaneSize;
            }

            //If the player position is very close to the target lane (by a difference of 0.18) it snaps it to the target position.
            //This makes it feel less sluggish and it allows the code to work properly.
            if (Mathf.Abs(this.playerPosition.x - this.targetLane) < 0.18f)
            {
                this.playerPosition.x = this.targetLane;
            }
        }
        else //If the character is in the same lane still
        {
            this.currentLane = this.targetLane; //Sets current lane to target, and tells the game that transitioning is false.
            this.transitioning = false;
            if(this.recentMoveScheduled != 0) //Scheduled recent movement for moving left and right.
            {
                this.targetLane += this.recentMoveScheduled; //Adds the scheduled movement to the target lane
                this.recentMoveScheduled = 0;
                this.CheckLaneBounds(); //Checks lane bounds incase the scheduled movement tries pushing out of bounds.
                if (this.targetLane != this.playerPosition.x)
                { 
                    this.characterMovementSFXPlayer.PlayLaneChangeSound(); //Plays the sound if the scheduled movement is successful.
                }
            }
        }

        //If the current rotation doesn't match the target rotation.
        if (this.currentrotation != this.targetrotation)
        {
            //It lerps between the two different rotations by a interpolation speed, setting it to the rotation until it's fully rotated.
            Quaternion linearPlayerRotate = Quaternion.Lerp(this.currentrotation, this.targetrotation, this.interpolationSpeedRotate);
            this.currentrotation = linearPlayerRotate;
            this._characterParent.transform.rotation = this.currentrotation;
        }
    }


    //This is where the Physics are updated. Within the physics are the raycasts for collision and charactermovement.
    private void UpdatePhysics()
    {
        if (!this.doPhysics) return; //Check to see if Physics are enabled.

        ApplyGravity();
        
        //Apply Speed to the player.
        this.playerPosition.y += this.playerYSpeed;

        //Get the player's Local Position from the rotated world position.
        this.playerPositionRelative = this._characterParent.transform.InverseTransformDirection(this.playerPosition);

        //Downwards raycast.//
        RaycastHit TempGroundRayHit; //Used to Nullify a RayHit if nothing is detected
        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1f, -this.playerPositionRelative.z), transform.TransformDirection(Vector3.down), out TempGroundRayHit, this.playerRaycastSizeDown);
        this.GroundRayHit = TempGroundRayHit; //Setting or Nullyfing the ground ray hit.

        if (TempGroundRayHit.collider != null) //If it isn't Null send collision information to the collision handler.
        {
            this.HandleCollision(TempGroundRayHit, WhichRay.Down);
        }
        else
        {
            this.isGrounded = false; //If it is null, the character is not grounded.

        }

        //Check to see if the player is moving left or right.
        if (this.transitioning)
        {
            RaycastHit SideHitUpper; //Raycasts hit info for the sides. Upper and Lower.
            RaycastHit SideHitLower;

            //This is a quick fix for not getting the issue "Not all paths return a value". The side raycast will still be created, but will be so tiny that it doesn't affect anything
            if (this.playerPosition.x == this.targetLane)
            {
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitUpper, 0);
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitLower, 0);
            }
            
            
            
            //Checks to see if the raycast is on the Left-Side, before creating Two raycasts (and Debug rays for visuals) that come out to the left side, passing out the "SideHit" for collision information.
            else if (this.playerPosition.x > this.targetLane)
            {
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.LeftLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.LeftLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
            }

            //Checks to see if the raycast is on the Right-Side, before creating Two raycasts (and Debug rays for visuals) that come out to the right side, passing out the "SideHit" for collision information.
            else
            {
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitUpper, this.playerRaycastSizeSide);
                Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.RightLocal[this.direction], out SideHitLower, this.playerRaycastSizeSide);

                Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
                Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.RightLocal[this.direction] * this.playerRaycastSizeDown, Color.yellow);
            }

            //Handles the Side collisions. This detects if top and bottom raycasts were hit at the same time.
            if ((SideHitUpper.collider != null) && (SideHitLower.collider != null))
            {
                this.HandleCollision(SideHitUpper, this.GetDirectionRay(2)); //Handles the type of collision
            }
            //This detects if only the top raycast was hit 
            else if ((SideHitUpper.collider != null) && (SideHitLower.collider == null))
            {
                this.HandleCollision(SideHitUpper, this.GetDirectionRay(0));//Handles the type of collision
            }
            //This detects if only the bottom raycast was hit 
            else if ((SideHitUpper.collider == null) && (SideHitLower.collider != null))
            {
                this.HandleCollision(SideHitLower, this.GetDirectionRay(1));//Handles the type of collision
            }
        }
        // Front raycast hit detection
        RaycastHit FrontHitUpper;
        RaycastHit FrontHitLower;

        //Creates the raycasts that stick in front of the player to detect the Front Collisions, for colliding with objects.
        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction], out FrontHitUpper, this.playerRaycastSizeFront);
        Physics.Raycast(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction], out FrontHitLower, this.playerRaycastSizeFront);
        
        //The debug rays to see how the rays look like in the project.
        Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 1.2f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeFront, Color.red);
        Debug.DrawRay(new Vector3(this.playerPositionRelative.x, this.playerPosition.y + 0.4f, -this.playerPositionRelative.z), this.ForwardLocal[this.direction] * this.playerRaycastSizeFront, Color.red);

        //Handles the front collisions. This detects if the front top and bottom raycasts were hit at the same time.
        if ((FrontHitUpper.collider != null) && (FrontHitLower.collider != null) && (this.currentState != playerStates.crouching))
        {
            this.HandleCollision(FrontHitUpper, WhichRay.FrontBoth); //Handles the type of collision
        }
        //This only detects if the front top raycast is hit.
        else if ((FrontHitUpper.collider != null) && (FrontHitLower.collider == null) && (this.currentState != playerStates.crouching))
        {
            this.HandleCollision(FrontHitUpper, WhichRay.FrontUp); //Handles the type of collision
        }
        //This only detects if the front bottom raycast is hit.
        else if (((FrontHitUpper.collider == null) || (this.currentState == playerStates.crouching)) && (FrontHitLower.collider != null))
        {
            this.HandleCollision(FrontHitLower, WhichRay.FrontDown); //Handles the type of collision
        }
        
        //Sets the player's local position to the PlayerPosition we have been modifying this entire time.
        this._character.transform.localPosition = this.playerPosition;
    }

    //Handle collision function which takes raycast hit information and which ray to 
    private void HandleCollision(RaycastHit rayHit, WhichRay whichRay)
    {
        //Searches for CollidableObjects script with each collision.
        CollidableObjects gameCollision = rayHit.collider.GetComponent<CollidableObjects>();
        if (gameCollision == null) return;
        gameCollision.DoCollision(whichRay);  //If it finds the collidable objects, it does the Collision, passing in which ray activated the collision/
    }
    
    private void ApplyGravity()
    {
        //if (!(!this.isGrounded || this.IgnoreGroundCollision == true)) return;
        if (this.isGrounded) return;
        this.playerYSpeed -= this.gravity;
        //if (!(this.playerYSpeed < this.playerMaxDownwardVelocity)) return;
        if (this.playerYSpeed > this.playerMaxDownwardVelocity) return;
        this.playerYSpeed = this.playerMaxDownwardVelocity;
    }

    //Allows the player to slide through character controller.
    public void Slide()
    {
        ////// LEGACY SLIDE TIMER
        //if (this.slideTick < this.slideTime || this.currentState == playerStates.crouching)
        //{
        //    this.slideScheduled = true;
        //    return;
        //} 

        if (!this.isGrounded || this.slideTick < this.slideTime) return;
        this.characterMovementSFXPlayer.PlaySlideSound(); //Plays slide theme.
        this.slideTick = 0; //Sets slide timer to 0
        
        this.currentState = playerStates.crouching; //Sets the current state to crouching
    }

    //Jump function used by the character controller.
    public void Jump()
    {
        if (!this.isGrounded) return;
        this.characterMovementSFXPlayer.PlayJumpSound(); //Plays the jump sound effect
        this.currentState = playerStates.jumping; //Sets playerstate to jumping
        this.playerYSpeed += this.jumpVelocity; //Adds the jump velocity to playerspeed.
        this.isGrounded = false;    //Sets grounded to false
    }

    //Gets the direction of the rays used for the Side collision (Side upper, Side lower, Side both.)
    private WhichRay GetDirectionRay(int Index)
    {
        switch (Index)
        {
            case 0: //Upper Collision
                if (this.recentMove == movementDirections.left) return (WhichRay.LeftUp); //Returns left if character was moving left
                if (this.recentMove == movementDirections.right) return (WhichRay.RightUp); //Returns right if character was moving right
                break;

            case 1: //Lower Collision
                if (this.recentMove == movementDirections.left) return (WhichRay.LeftDown); //Returns left if character was moving left
                if (this.recentMove == movementDirections.right) return (WhichRay.RightDown);//Returns right if character was moving right
                break;

            case 2: //Both
                if (this.recentMove == movementDirections.left) return (WhichRay.LeftBoth); //Returns left if character was moving left           
                if (this.recentMove == movementDirections.right) return (WhichRay.RightBoth);//Returns right if character was moving right
                break;
        }
        return WhichRay.LeftBoth;
    }

    //Function to tell the manager the character is grounded.
    public void GroundedCharacter()
    {
        this.isGrounded = true;                         //Tells the manager that the character is grounded so certain behaviours can happen (like sliding)
        this.playerYSpeed = 0;                              //Sets the player's Y speed to 0 so they don't fall through the ground.
        this.playerPosition.y = this.GroundRayHit.point.y - 0.02f; //Places the player on the ground where the raycast is.
        if (this.currentState == playerStates.crouching) return;
        this.currentState = playerStates.grounded; //Sets the player's state to grounded if they aren't sliding.
    }
    
    //Hops the character up. This is used by the character collide system to hop up.
    public void HopUpCharacter()
    {
        if (this.hopUpTick < this.hopUpTime) return;
        this.isGrounded = false;
        this.playerYSpeed = this.hopUpSpeed; //Sets the Y Speed to hop up so the character goes upwards always.
    }


    //Updates the timers used within the manager.
    private void UpdateTimers()
    {
        if (this.hopUpTick < this.hopUpTime)  //Updates the hop up tick. It's a cool down so that the player isn't shot into the sky when hopping up.
        { 
            this.hopUpTick += Time.fixedDeltaTime; 
        }
        
        if (this.slideTick < this.slideTime) //Updates the slide tick. This is so that there is adelay between every time you slide.
        {

            this.slideTick += Time.fixedDeltaTime;

            if (this.currentState != playerStates.crouching) //If the character is no longer in a slidign state (e.g Jumping) it'll finish the timer.
            {
                this.slideTick = this.slideTime;
            }
            if ((this.slideTick >= this.slideTime)) //Once the timer reaches the end, it will set it to grounded if the character is crouching.
            {
                if (this.currentState != playerStates.crouching) return;
                this.currentState = playerStates.grounded;

            }
        }
    }

    //Old legacy code for scheduling the movement. This would have allowed the player to schedule a slide movement.
    //This didn't work correctly, and was easier to cut then figure out.
    private void ScheduledMovements()
    {
        if (this.isGrounded)
        {
            if (this.currentState == playerStates.grounded && this.slideScheduled == true && this.slideTick >= this.slideTime)
            {
                this.slideScheduled = false;
                Slide();
            }
        }
    }

    //Retrieves whether or not the character is transitioning.
    public bool GetPlayerTransitioningState() { return this.transitioning; }
    
    //Retrieves the character's current lane.
    public float GetPlayerLaneCurrent() { return (this.currentLane / this.LaneSize); }
    
    //Retrieves the character's target lane.
    public float GetPlayerLaneTarget() { return (this.targetLane / this.LaneSize); }
    
    //Allows the lane to be set. Unused in the end.
    public void SetPlayerLaneTarget(int lane) {
        this.targetLane = lane * this.LaneSize;
        this.CheckLaneBounds(); 
    }
    
    //Allows the Collision System to push the character left or right.
    public void AddPlayerLaneTarget(int lane) {
        this.targetLane += lane * this.LaneSize;
        this.CheckLaneBounds();
    }
    
    //Returns the character to the previous lane.
    public void PreviousLaneReturn() {
        this.targetLane = this.previousLane;
        this.CheckLaneBounds();
    }
    
    //Retrieves the player's current X local position.
    public float GetPlayerPositionXCurrent() { return (this.currentLane); }
    
    //Retrieves the player's target X local position.
    public float GetPlayerPositionXTarget() { return (this.targetLane); }
    
    //Retrieves where the player is looking (North, South, East, West).
    public int GetPlayerDirection() { return this.direction; }
    
    //Returns whether or not an extra lane is generated on the left or right. Since the lanes are preset, this is redundant
    public int GetEvenLaneBias() { return this.evenLaneBias; } 
    
    //Gets the total number of lanes. Since the lanes are now always 5, it will return 5.
    public int GetNumberOfLanes() { return this.numberOfLanes; }
    
    //Retrieves the lane boundaries (on the left or right, 0 or 1. Since the lanes are set by default, this should return 2 each side.
    public float GetLaneBoundaries(int direction) { return this.laneBoundaries[direction]; }

    //Retrieve the animator of the character.
    public Animator GetAnimator() { return this.playerAnimator; }
}