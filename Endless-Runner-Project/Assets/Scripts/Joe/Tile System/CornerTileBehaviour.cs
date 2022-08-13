using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CORNER TILE BEHAVIOUR CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Added hiddenExtension which stops the player being able to see into the abyss after turning.
 */

/// <summary>
/// Defines behaviour of turning at a corner tile.
/// </summary>
public class CornerTileBehaviour : MonoBehaviour
{
    public TurnDirection turnDirection;
    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;
    [SerializeField] private GameObject hiddenExtension;

    // Start is called before the first frame update
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


    // Update is called once per frame
    void FixedUpdate()
    {

        bool turnReady = this.CheckIfTurnReady();

        // If the turn is ready but has not been completed yet then the code is executed
        if (turnReady == true && this.hasRotated == false)
        {
            this.characterManager.Rotate(this.turnDirection);
            // Add an extra wall behind the tile to fill blank space that sometimes appeared when travelling at high speeds.
            this.hiddenExtension.SetActive(true);
            // Return the character to the center lane
            this.characterManager.SetLanePos(0);

            // Set the new run direction as the current direction at which the tiles are spawning.
            // This works because only one corner tile can ever exist at a time, so the spawn direction
            // is always the direction after the corner when the corner is reached.
            this.tileManager.runDirection = this.tileManager.spawnDirection;
            this.hasRotated = true;

        }
    }
}
