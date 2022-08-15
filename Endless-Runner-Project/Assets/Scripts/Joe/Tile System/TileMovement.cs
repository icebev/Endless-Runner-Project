using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TILE MOVEMENT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// To be added to each tile prefab to make it move towards the origin over time.
/// </summary>
public class TileMovement : MonoBehaviour
{
    private TileManager tileManager;
    private Rigidbody tileRigidbody;
    private float offesetCorrectionThreshold = 0.5f;
    private float correctionMaxVal = 1.0f;

    private void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.tileRigidbody = this.GetComponent<Rigidbody>();
    }


    public void InLineCorrectOffset()
    {
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (Mathf.Abs(this.transform.position.x) < this.correctionMaxVal && Mathf.Abs(this.transform.position.x) > 0)
                    {
                        //this.tileRigidbody.MovePosition(new Vector3(0, this.tileRigidbody.position.y, this.tileRigidbody.position.z));
                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (Mathf.Abs(this.transform.position.z) < this.correctionMaxVal && Mathf.Abs(this.transform.position.z) > 0)
                    {
                        //this.tileRigidbody.MovePosition(new Vector3(this.tileRigidbody.position.x, this.tileRigidbody.position.y, 0));

                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (Mathf.Abs(this.transform.position.x) < this.correctionMaxVal && Mathf.Abs(this.transform.position.x) > 0)
                    {
                        //this.tileRigidbody.MovePosition(new Vector3(0, this.tileRigidbody.position.y, this.tileRigidbody.position.z));

                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (Mathf.Abs(this.transform.position.z) < this.correctionMaxVal && Mathf.Abs(this.transform.position.z) > 0)
                    {
                        //this.tileRigidbody.MovePosition(new Vector3(this.tileRigidbody.position.x, this.tileRigidbody.position.y, 0));

                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                    break;
                }
        }
    }

    public void CorrectOffset()
    {
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z > this.offesetCorrectionThreshold)
                    {
                        this.tileRigidbody.MovePosition(new Vector3(0, this.tileRigidbody.position.y, this.tileRigidbody.position.z));
                        //this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x < this.offesetCorrectionThreshold)
                    {
                        this.tileRigidbody.MovePosition(new Vector3(this.tileRigidbody.position.x, this.tileRigidbody.position.y, 0));

                        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z < this.offesetCorrectionThreshold)
                    {
                        this.tileRigidbody.MovePosition(new Vector3(0, this.tileRigidbody.position.y, this.tileRigidbody.position.z));

                        //this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x > this.offesetCorrectionThreshold)
                    {
                        this.tileRigidbody.MovePosition(new Vector3(this.tileRigidbody.position.x, this.tileRigidbody.position.y, 0));

                        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                break;
                }
        }
    }

    private void FixedUpdate()
    {

        //if (this.tileManager.runDirection == this.tileManager.spawnDirection)
        //{
        //    this.CorrectOffset();
        //}

        // Check if the tile needs to be destroyed then move it using the run direction
        this.InLineCorrectOffset();
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z < -this.tileManager.despawnDistance )
                    {
                        this.tileManager.SpawnAdditionalTile();
                        if (this.GetComponent<TileCoinSpawn>())
                        {
                            this.GetComponent<TileCoinSpawn>().ReleaseCoins();
                        }
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(0, 0, this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        if (this.GetComponent<TileCoinSpawn>())
                        {
                            this.GetComponent<TileCoinSpawn>().ReleaseCoins();
                        }
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(-this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        if (this.GetComponent<TileCoinSpawn>())
                        {
                            this.GetComponent<TileCoinSpawn>().ReleaseCoins();
                        }
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(0, 0, -this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x < -this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        if (this.GetComponent<TileCoinSpawn>())
                        {
                            this.GetComponent<TileCoinSpawn>().ReleaseCoins();
                        }
                        Destroy(this.gameObject);

                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.transform.position - new Vector3(this.tileManager.GetCurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
        }



    }

}
