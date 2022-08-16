using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TILE MOVEMENT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Refactored the InLineOffset() method to use an if statement instead of a switch, reducing repeated code
 * Created a method called DespawnThisTile() to replace several lines of repeated despawn code.
 */
/// <summary>
/// To be added to each tile prefab to make it move towards the origin over time.
/// Also handles despawning and offset self-correction.
/// </summary>
public class TileMovement : MonoBehaviour
{
    private TileManager tileManager;
    private Rigidbody tileRigidbody;
    private float correctionMaxVal = 1.0f;

    private void Start()
    {
        // Get private references
        this.tileManager = FindObjectOfType<TileManager>();

        // We use a RigidBody component on the tile object
        // rather than changing it's transform on each FixedUpdate()
        // for smoother movement towards the player
        this.tileRigidbody = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Tile offset correction that utilises the current posision of the tile in conjunction 
    /// with the current run durection to ensure that it is centered on the player for seamless gameplay
    /// </summary>
    private void InLineCorrectOffset()
    {
        // If the run direction is z-axis aligned we center the tile around x
        // and likewise if the run direction is x-axis aligned we center the tile around z
        if (this.tileManager.runDirection == TrackDirection.positiveZ || this.tileManager.runDirection == TrackDirection.negativeZ)
        {
            // Correction max val is very important, ensuring that
            // faraway tiles that are spawning in a direction other than the run direction
            // are not all centered around the player
            if (Mathf.Abs(this.transform.position.x) < this.correctionMaxVal && Mathf.Abs(this.transform.position.x) > 0)
            {
                this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
            }
        }
        // The run direction must be x-aligned otherwise
        else
        {
            if (Mathf.Abs(this.transform.position.z) < this.correctionMaxVal && Mathf.Abs(this.transform.position.z) > 0)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            }
        }
    }

    // Destroys this tile after creating a replacement at the opposite end of the treadmill,
    // and releasing any coins back to the object pool
    private void DespawnThisTile()
    {
        this.tileManager.SpawnAdditionalTile();
        if (this.GetComponent<TileCoinSpawn>())
        {
            this.GetComponent<TileCoinSpawn>().ReleaseCoins();
        }
        Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        // Offset correction is ran as part of FixedUpdate to reduce gaps between tiles as much as possible
        this.InLineCorrectOffset();

        // For the current run direction we must execute specific code.
        // First we check if the tile needs to be destroyed then move it along the correct axis.
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z < -this.tileManager.despawnDistance)
                    {
                        this.DespawnThisTile();
                    }

                    // Move the tile in the positive z axis
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(0, 0, this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x > this.tileManager.despawnDistance)
                    {
                        this.DespawnThisTile();
                    }

                    // Move the tile in the negative x axis
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(-this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z > this.tileManager.despawnDistance)
                    {
                        this.DespawnThisTile();
                    }

                    // Move the tile in the negative z axis
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(0, 0, -this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x < -this.tileManager.despawnDistance)
                    {
                        this.DespawnThisTile();
                    }

                    // Move the tile in the positive x axis
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
        }

    }

}
