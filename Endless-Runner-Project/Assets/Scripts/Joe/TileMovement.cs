using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private TileManager tileManager;
    private Rigidbody tileRigidbody;
    private float offesetCorrectionThreshold = 1.5f;
    private void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.tileRigidbody = this.GetComponent<Rigidbody>();
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
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z < -this.tileManager.despawnDistance )
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.tileRigidbody.position - new Vector3(0, 0, this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.tileRigidbody.position - new Vector3(-this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.tileRigidbody.position - new Vector3(0, 0, -this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x < -this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    Vector3 newTargetPosition = new Vector3();
                    newTargetPosition = this.tileRigidbody.position - new Vector3(this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    this.tileRigidbody.MovePosition(newTargetPosition);

                    break;
                }
        }



    }

}
