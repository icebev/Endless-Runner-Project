using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* JUNCTION TILE BEHAVIOUR CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Refactored code to be split into methods to increase readabiltiy.
 */

/// <summary>
/// Defines behaviour of turning at a junction tile.
/// </summary>
public class JunctionTileBehaviour : MonoBehaviour
{

    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;

    [SerializeField] private JunctionTurnPositionOverride positionOverride;
    [SerializeField] private bool hasLeftTurn;
    [SerializeField] private bool hasRightTurn;
    [SerializeField] private bool hasCenterCorridor;

    [System.Obsolete]
    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.characterManager = FindObjectOfType<CharacterManager>();
    }

    /// <summary>
    /// Check if the turn is ready by comparing the transform position against the origin based on the current run direction 
    /// </summary>
    private bool CheckIfTurnReady()
    {
        switch (this.tileManager.runDirection)
        {
            case (TrackDirection.negativeX):
                {
                    if (this.transform.position.x >= 0.0f)
                    {
                        return true;
                    }
                    break;
                }
            case (TrackDirection.positiveX):
                {
                    if (this.transform.position.x <= 0.0f)
                    {
                        return true;
                    }
                    break;
                }
            case (TrackDirection.negativeZ):
                {
                    if (this.transform.position.z >= 0.0f)
                    {
                        return true;
                    }
                    break;
                }
            case (TrackDirection.positiveZ):
                {
                    if (this.transform.position.z <= 0.0f)
                    {
                        return true;
                    }
                    break;
                }
        }

        return false;
    }


    // A private method containing code that is called regardless of which direction the character has turned.
    // Written to avoid duplicated code.
    /// <summary>
    /// Returns the character to the center lane, and resets the junction spawning system ready for the next junction.
    /// </summary>
    private void CompleteTurn()
    {
        this.characterManager.SetLanePos(0);
        this.tileManager.spawningAfterJunction = false;
        this.tileManager.junctionSpawnedTilesList.Clear();
        this.tileManager.currentJunctionTile = null;
        this.hasRotated = true;
    }

    private void FixedUpdate()
    {
        bool turnReady = this.CheckIfTurnReady();

        if (turnReady == true && this.hasRotated == false)
        {
            // If the player is in the right-hand lane and the junction has a right turn 
            if (this.characterManager.GetPlayerLaneTarget() == 2 && this.hasRightTurn)
            {
                this.characterManager.Rotate(TurnDirection.Right);
                this.tileManager.TrackSpawnRightTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;

                // Rotate all of the junction spawned tiles - the tiles spawed after the junction before the player reaches it - around the center of the junction.
                // This gives the illusion of the tiles always being there after the turn is complete.
                // Rotated to the right for a right turn
                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, 90);
                }

                // Begin the position override while the turn is taking place
                this.positionOverride.ActivateRightTurn();
                this.CompleteTurn();

            }
            // If the player is in the left-hand lane and the junction has a left turn 
            else if (this.characterManager.GetPlayerLaneTarget() == -2 && this.hasLeftTurn)
            {
                this.characterManager.Rotate(TurnDirection.Left);
                this.tileManager.TrackSpawnLeftTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;

                // Rotate all of the junction spawned tiles - the tiles spawed after the junction before the player reaches it - around the center of the junction.
                // This gives the illusion of the tiles always being there after the turn is complete.
                // Rotated to the left for a left turn
                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, -90);

                }

                // Begin the position override while the turn is taking place
                this.positionOverride.ActivateLeftTurn();
                this.CompleteTurn();

            }
            // Center door taken 
            else
            {
                this.CompleteTurn();
            }

            
        }
    }
}
