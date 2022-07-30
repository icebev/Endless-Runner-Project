using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CornerTileBehaviour : MonoBehaviour
{
    public TurnDirection turnDirection;
    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;

    // Start is called before the first frame update
    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.characterManager = FindObjectOfType<CharacterManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        bool turnReady = false;

        switch (this.tileManager.runDirection)
        {
            case (TrackDirection.negativeX):
                {
                    if (this.transform.position.x >= 0.0f)
                    {
                        turnReady = true;
                    }
                    break;
                }
            case (TrackDirection.positiveX):
                {
                    if (this.transform.position.x <= 0.0f)
                    {
                        turnReady = true;
                    }
                    break;
                }
            case (TrackDirection.negativeZ):
                {
                    if (this.transform.position.z >= 0.0f)
                    {
                        turnReady = true;
                    }
                    break;
                }
            case (TrackDirection.positiveZ):
                {
                    if (this.transform.position.z <= 0.0f)
                    {
                        turnReady = true;
                    }
                    break;
                }
        }

        if (turnReady == true && this.hasRotated == false)
        {
            if (this.hasRotated == false)
            {
                this.characterManager.Rotate(this.turnDirection);
                this.characterManager.SetLanePos(0);

                this.hasRotated = true;
                this.tileManager.runDirection = this.tileManager.spawnDirection;

            }
        }
    }
}
