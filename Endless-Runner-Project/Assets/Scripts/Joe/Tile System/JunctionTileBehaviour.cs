using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* JUNCTION TILE BEHAVIOUR CLASS
 * Author(s): Joe Bevis
 * Date last modified: 03/07/2022
 *******************************************************************************
 * CHANGE NOTES:
 * 
 * 
 */

/// <summary>
/// Defines behaviour of turning at a junction tile.
/// </summary>
public class JunctionTileBehaviour : MonoBehaviour
{

    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;
    private float turnDist = 0.1f;

    [SerializeField] private bool hasLeftTurn;
    [SerializeField] private bool hasRightTurn;
    [SerializeField] private bool hasCenterCorridor;

    [System.Obsolete]

    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.characterManager = FindObjectOfType<CharacterManager>();
    }
    private void FixedUpdate()
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
            if (this.characterManager.GetPlayerLaneTarget() == 2 && this.hasRightTurn)
            {
                this.characterManager.Rotate(TurnDirection.Right);
                this.tileManager.TrackSpawnRightTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;


                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, 90);
                    //tileObject.GetComponent<TileMovement>().CorrectOffset();
                }
                //foreach (Transform child in this.tileManager.tilesContainer.transform)
                //{
                //    child.GetComponent<TileMovement>().CorrectOffset();
                //}

            }
            else if (this.characterManager.GetPlayerLaneTarget() == -2 && this.hasLeftTurn)
            {
                this.characterManager.Rotate(TurnDirection.Left);
                this.tileManager.TrackSpawnLeftTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;


                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, -90);
                    //tileObject.GetComponent<TileMovement>().CorrectOffset();

                }
            }

            this.tileManager.spawningAfterJunction = false;
            this.characterManager.SetLanePos(0);
            this.hasRotated = true;
            this.tileManager.junctionSpawnedTilesList.Clear();
            this.tileManager.currentJunctionTile = null;
        }
    }
}
