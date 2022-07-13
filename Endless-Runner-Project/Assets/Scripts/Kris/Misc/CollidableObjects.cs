using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableObjects : MonoBehaviour, iCollidable
{
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

    [SerializeField] private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    [SerializeField] private CollisionBehaviour[] UpwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.GoDown };

    [SerializeField] private CollisionBehaviour[] FrontUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.HopUp };
    [SerializeField] private CollisionBehaviour[] FrontLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.Stumble };
    [SerializeField] private CollisionBehaviour[] FrontBothCollision= new CollisionBehaviour[] { CollisionBehaviour.Kill };

    [SerializeField] private CollisionBehaviour[] LeftUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveRight };
    [SerializeField] private CollisionBehaviour[] LeftLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveRight };
    [SerializeField] private CollisionBehaviour[] LeftBothCollision= new CollisionBehaviour[] { CollisionBehaviour.MoveRight };

    [SerializeField] private CollisionBehaviour[] RightUpperCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };
    [SerializeField] private CollisionBehaviour[] RightLowerCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };
    [SerializeField] private CollisionBehaviour[] RightBothCollision = new CollisionBehaviour[] { CollisionBehaviour.MoveLeft };

    private CharacterManager charManager;
    private GameObject objCharManager;

    /*private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    private CollisionBehaviour[] DownwardsCollision = new CollisionBehaviour[] { CollisionBehaviour.StandOn };
    */

    public void DoCollision(CharacterManager.WhichRay whichRay) 
    {
        if(this.objCharManager == null)
        {
            this.objCharManager = GameObject.FindGameObjectWithTag("CharacterManager");
            this.charManager = this.objCharManager.GetComponent<CharacterManager>();
        }

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

                break;
            case CollisionBehaviour.GoDown:

                break;
            case CollisionBehaviour.SlowDown:

                break;
            case CollisionBehaviour.Stumble:

                break;
            case CollisionBehaviour.MoveLeft:
                this.charManager.AddPlayerLaneTarget(-1);
                break;
            case CollisionBehaviour.MoveRight:
                this.charManager.AddPlayerLaneTarget(1);
                break;
            case CollisionBehaviour.Kill:

                break;
        }
    }
}
