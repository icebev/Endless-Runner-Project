using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 18/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * 
 */
/// <summary>
/// A class for implementing custom collision behaviours to objects.
/// </summary>

public class CollidableObjects : MonoBehaviour, iCollidable
{
    //Cooldowns as to not collide every frame
    public static float stumbleCoolDown = 0.3f;   //How often the player can stumble
    public static float pushRightCoolDown = 0.3f; //How often the character can be pushed left and right.
    public static float pushLeftCoolDown = 0.3f;
    
    //List of the different type of collision behaviours. Not all of them are used, but they stay for legacy reasons.
    private enum CollisionBehaviour
    {
        NoCollide,
        StandOn,
        BurnFeet,
        FallThrough,
        HopUp,
        GoDown,
        SlowDown,
        Stumble,
        MoveLeft,
        MoveRight,
        Kill,
    }

    //Expandable arrays for adding Collision Behaviours

    //Up & Down Collisions
    [SerializeField] private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    [SerializeField] private CollisionBehaviour[] UpwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.GoDown };

    //Front Collisions
    [SerializeField] private CollisionBehaviour[] FrontUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.HopUp };
    [SerializeField] private CollisionBehaviour[] FrontLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.Stumble };
    [SerializeField] private CollisionBehaviour[] FrontBothCollision= new CollisionBehaviour[] { CollisionBehaviour.Kill };

    //Right Collisions
    [SerializeField] private CollisionBehaviour[] LeftUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveRight };
    [SerializeField] private CollisionBehaviour[] LeftLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveRight };
    [SerializeField] private CollisionBehaviour[] LeftBothCollision= new CollisionBehaviour[] { CollisionBehaviour.MoveRight };

    //Left Collisions
    [SerializeField] private CollisionBehaviour[] RightUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };
    [SerializeField] private CollisionBehaviour[] RightLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };
    [SerializeField] private CollisionBehaviour[] RightBothCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };

    //To retrieve the character Manager and perform behaviours/
    private CharacterManager charManager;
    private GameObject objCharManager;


    // Ticking the CoolDowns
    public static void TickCooldowns()
    {
        if (CollidableObjects.stumbleCoolDown >= 0.0f) //Cooldown for stumbling.
        {
            CollidableObjects.stumbleCoolDown -= Time.fixedDeltaTime;
        }

        if (CollidableObjects.pushRightCoolDown >= 0.0f) //Cooldown for being pushed right
        {
            CollidableObjects.pushRightCoolDown -= Time.fixedDeltaTime;
        }

        if (CollidableObjects.pushLeftCoolDown >= 0.0f) //Cooldown for being pushed left
        {
            CollidableObjects.pushLeftCoolDown -= Time.fixedDeltaTime;
        }
    }

    //Do Collision Function.
    public void DoCollision(CharacterManager.WhichRay whichRay) 
    {

        //Retrieves the Character Manager when first collision is detected.
        //Saves on small resources instead of keeping it in Start()
        if(this.objCharManager == null)
        {
            this.objCharManager = GameObject.FindGameObjectWithTag("CharacterManager");
            this.charManager = this.objCharManager.GetComponent<CharacterManager>();
        }

        //Switch for doing each type of collision.

        switch (whichRay)
        {
            case CharacterManager.WhichRay.Down: //Checks each collision for Downwards Raycast.
                foreach (CollisionBehaviour gameCollision in this.DownwardsCollision) 
                {
                    this.CollisionReaction(gameCollision); //Does the Down collisions

                }
                break;
            
            case CharacterManager.WhichRay.Up: //Checks each collision for Upwards Raycast.
                foreach (CollisionBehaviour gameCollision in this.UpwardsCollision)
                {
                    this.CollisionReaction(gameCollision); //Does the Up collisions
                }

                break;
            
            case CharacterManager.WhichRay.FrontUp: //Checks each collision for Front Up Raycast.
                foreach (CollisionBehaviour gameCollision in this.FrontUpperCollision)
                {
                    this.CollisionReaction(gameCollision); //Does the Front Up collisions
                }
                break;
            
            case CharacterManager.WhichRay.FrontDown: //Checks each collision for Front Down Raycast.
                foreach (CollisionBehaviour gameCollision in this.FrontLowerCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Front Down collisions
                }
                break;

            case CharacterManager.WhichRay.FrontBoth: //Checks each collision for Front Both Raycast.
                foreach (CollisionBehaviour gameCollision in this.FrontBothCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Front Both collisions
                }
                break;

            case CharacterManager.WhichRay.LeftUp: //Checks each collision for Left Up Raycast.
                foreach (CollisionBehaviour gameCollision in this.LeftUpperCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Left Upper collisions
                }
                break;
            
            case CharacterManager.WhichRay.LeftDown: //Checks each collision for Left Down Raycast.
                foreach (CollisionBehaviour gameCollision in this.LeftLowerCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Left Lower collisions
                }
                break;
            
            case CharacterManager.WhichRay.LeftBoth: //Checks each collision for Left Both Raycast.
                foreach (CollisionBehaviour gameCollision in this.LeftBothCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Left Both collisions
                }
                break;
            
            case CharacterManager.WhichRay.RightUp: //Checks each collision for Right Up Raycast.
                foreach (CollisionBehaviour gameCollision in this.RightUpperCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Right Upper collisions
                }
                break;
            
            case CharacterManager.WhichRay.RightDown: //Checks each collision for Right Down Raycast.
                foreach (CollisionBehaviour gameCollision in this.RightLowerCollision)
                {
                    this.CollisionReaction(gameCollision);//Does the Right Lower collisions
                }
                break;
            
            case CharacterManager.WhichRay.RightBoth: //Checks each collision for Right Both Raycast.
                foreach (CollisionBehaviour gameCollision in this.RightBothCollision)
                {
                    this.CollisionReaction(gameCollision); //Does the Right Both collisions
                }
                break;
        }
    }

    //How the selected collision behaviours react.
    private void CollisionReaction(CollisionBehaviour colBehaviour)
    {
        switch (colBehaviour)
        {
            case CollisionBehaviour.NoCollide: //Unused "No collide"

            break;
            case CollisionBehaviour.StandOn: //Used for Standing on objects
                this.charManager.GroundedCharacter();

                break;
            case CollisionBehaviour.BurnFeet: //Unused "Burn Feet"

                break;
            case CollisionBehaviour.FallThrough: //Unused "Fall Through"

                break;
            case CollisionBehaviour.HopUp: //Used for hopping the character up when stumbling on an object.
                if (GameOverEvent.isPlayerDead == true) return;

                this.charManager.HopUpCharacter(); //Hops up.
                break;
            case CollisionBehaviour.GoDown: //Unused "Go Down"

                break;
            case CollisionBehaviour.SlowDown: //Unused "Slow Down"

                break;
            case CollisionBehaviour.Stumble: //Used for stumbling on objects.
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.stumbleCoolDown > 0 || GameObject.FindObjectOfType<SprintSystem>().speedBoostModeActive == true) return; //Character can't stumble during cooldown and speed boost powerup
                CollidableObjects.stumbleCoolDown = 0.3f;
                if (GameObject.FindObjectOfType<SprintSystem>().isSprinting == false) //Checks to see fi the character is Sprinting
                {
                    GameObject.FindObjectOfType<ObstacleCollisionConsequences>().StumbleSlowDown(CollisionConsequenceType.RegularTrip); //Trips the character lightly when not running
                }
                else
                {
                    GameObject.FindObjectOfType<ObstacleCollisionConsequences>().StumbleSlowDown(CollisionConsequenceType.SprintingTrip);//Trips the character dramatically when running
                }
                break;
                
            case CollisionBehaviour.MoveLeft: //Used for pushing the character left.
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.pushLeftCoolDown > 0) return;
                CollidableObjects.pushLeftCoolDown = 0.3f;
                this.charManager.AddPlayerLaneTarget(-1); //Adds the position down a lane to the left.
                break;
                
            case CollisionBehaviour.MoveRight: //Used for pushing the character right.
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.pushLeftCoolDown > 0) return; 
                CollidableObjects.pushLeftCoolDown = 0.3f;
                this.charManager.AddPlayerLaneTarget(1); //Adds the position up a lane to the right.
                break;
                
            case CollisionBehaviour.Kill: //Used for killing the character when colliding.
                if (GameOverEvent.isPlayerDead == true) return;
                GameObject.FindObjectOfType<ObstacleCollisionConsequences>().StumbleSlowDown(CollisionConsequenceType.LethalCollision); //Does a lethal stumble collision that kills instantly.
                break;
        }
    }
}
