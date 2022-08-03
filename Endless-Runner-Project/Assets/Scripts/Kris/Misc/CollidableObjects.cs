using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 16/07/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for implementing custom collision behaviours to objects.
/// </summary>

public class CollidableObjects : MonoBehaviour, iCollidable
{
    //Cooldowns as to not collide every frame
    public static float stumbleCoolDown = 0.3f;     
    public static float pushRightCoolDown = 0.3f; //Push is when the character is moved left or right
    public static float pushLeftCoolDown = 0.3f;
    
    //List of the different type of collision behaviours.
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


    //Please Refactor. Ticking the CoolDowns
    public static void TickCooldowns()
    {
        if (CollidableObjects.stumbleCoolDown >= 0.0f)
        {
            CollidableObjects.stumbleCoolDown -= Time.fixedDeltaTime;
        }

        if (CollidableObjects.pushRightCoolDown >= 0.0f)
        {
            CollidableObjects.pushRightCoolDown -= Time.fixedDeltaTime;
        }

        if (CollidableObjects.pushLeftCoolDown >= 0.0f)
        {
            CollidableObjects.pushLeftCoolDown -= Time.fixedDeltaTime;
        }
    }
    public void DoCollision(CharacterManager.WhichRay whichRay) 
    {

        //Retrieves the Character Manager when first collision is detected.
        //Saves on small resources instead of keeping it in Start()
        if(this.objCharManager == null)
        {
            this.objCharManager = GameObject.FindGameObjectWithTag("CharacterManager");
            this.charManager = this.objCharManager.GetComponent<CharacterManager>();
        }


        //Note for future kris: Please Refactor.
        //Create a temporary CollisionBehaviour and store the current collision.
        ////EXAMPLE:
        // case CharacterManager.WhichRay.Down:
        //  TemporaryColInfo = this.DownwardsCollision
        //  break;
        // .... 
        //  foreach(CollisionBehaviour gameCollision in TemporaryColInfo)
        //

        switch (whichRay)
        {
            case CharacterManager.WhichRay.Down:
                foreach(CollisionBehaviour gameCollision in this.DownwardsCollision)
                {
                    this.CollisionReaction(gameCollision);

                }
                break;
            
            case CharacterManager.WhichRay.Up:
                foreach (CollisionBehaviour gameCollision in this.UpwardsCollision)
                {
                    this.CollisionReaction(gameCollision);
                }

                break;
            
            case CharacterManager.WhichRay.FrontUp:
                foreach (CollisionBehaviour gameCollision in this.FrontUpperCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;
            
            case CharacterManager.WhichRay.FrontDown:
                foreach (CollisionBehaviour gameCollision in this.FrontLowerCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;

            case CharacterManager.WhichRay.FrontBoth:
                foreach (CollisionBehaviour gameCollision in this.FrontBothCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;

            case CharacterManager.WhichRay.LeftUp:
                foreach (CollisionBehaviour gameCollision in this.LeftUpperCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;
            
            case CharacterManager.WhichRay.LeftDown:
                foreach (CollisionBehaviour gameCollision in this.LeftLowerCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;
            
            case CharacterManager.WhichRay.LeftBoth:
                foreach (CollisionBehaviour gameCollision in this.LeftBothCollision)
                {
                    this.CollisionReaction(gameCollision);
                }

                break;
            
            case CharacterManager.WhichRay.RightUp:
                foreach (CollisionBehaviour gameCollision in this.RightUpperCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;
            
            case CharacterManager.WhichRay.RightDown:
                foreach (CollisionBehaviour gameCollision in this.RightLowerCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;
            
            case CharacterManager.WhichRay.RightBoth:
                foreach (CollisionBehaviour gameCollision in this.RightBothCollision)
                {
                    this.CollisionReaction(gameCollision);
                }
                break;

        }

    }


    //How the selected collision behaviours react.
    private void CollisionReaction(CollisionBehaviour colBehaviour)
    {
        switch (colBehaviour)
        {


            case CollisionBehaviour.NoCollide:

            break;
            case CollisionBehaviour.StandOn:
                this.charManager.GroundedCharacter();

                break;
            case CollisionBehaviour.BurnFeet:

                break;
            case CollisionBehaviour.FallThrough:

                break;
            case CollisionBehaviour.HopUp:
                if (GameOverEvent.isPlayerDead == true) return;

                this.charManager.HopUpCharacter();
                break;
            case CollisionBehaviour.GoDown:

                break;
            case CollisionBehaviour.SlowDown:

                break;
            case CollisionBehaviour.Stumble:
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.stumbleCoolDown > 0 || GameObject.FindObjectOfType<SprintSystem>().speedBoostModeActive == true) return; 
                CollidableObjects.stumbleCoolDown = 0.3f;
                if (GameObject.FindObjectOfType<SprintSystem>().isSprinting == false)
                {
                    GameObject.FindObjectOfType<ObstacleCollisionConsequences>().StumbleSlowDown(0);
                }
                else
                {
                    GameObject.FindObjectOfType<ObstacleCollisionConsequences>().StumbleSlowDown(1);
                }
                break;
                
            case CollisionBehaviour.MoveLeft:
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.pushLeftCoolDown > 0) return;
                CollidableObjects.pushLeftCoolDown = 0.3f;
                this.charManager.AddPlayerLaneTarget(-1);
                break;
                
            case CollisionBehaviour.MoveRight:
                if (GameOverEvent.isPlayerDead == true) return;

                if (CollidableObjects.pushLeftCoolDown > 0) return; 
                CollidableObjects.pushLeftCoolDown = 0.3f;
                this.charManager.AddPlayerLaneTarget(1);
                break;
                
            case CollisionBehaviour.Kill:

                break;
        }
    }
}
