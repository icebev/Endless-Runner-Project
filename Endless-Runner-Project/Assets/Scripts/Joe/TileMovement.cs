using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private TileManager tileManager;
  
    private void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
    }

    public void CorrectOffset()
    {
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z > 0)
                    {
                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x < 0)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z < 0)
                    {
                        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x > 0)
                    {
                        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                break;
                }
        }
    }

    private void FixedUpdate()
    {
        // Check if the tile needs to be destroyed then move it using the run direction
        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z < -this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    this.transform.position = this.transform.position - new Vector3(0, 0, this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime);
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    this.transform.position = this.transform.position - new Vector3(-this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z > this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    this.transform.position = this.transform.position - new Vector3(0, 0, -this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime);
                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x < -this.tileManager.despawnDistance)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    this.transform.position = this.transform.position - new Vector3(this.tileManager.CurrentTileSpeed * Time.fixedDeltaTime, 0, 0);
                    break;
                }
        }
    }

}
