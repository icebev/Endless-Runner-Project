using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float despawnZPosition;
    public TileManager tileManager;
    public float moveSpeed;

    public float CurrentSpeed
    {
        get
        {
            return this.moveSpeed;
        }
    }

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

        switch (this.tileManager.runDirection)
        {
            case TrackDirection.positiveZ:
                {
                    if (this.transform.position.z < -5)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed * Time.fixedDeltaTime);
                    if (this.tileManager.runDirection == TrackDirection.positiveZ)
                    {
                        //this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                    break;
                }
            case TrackDirection.negativeX:
                {
                    if (this.transform.position.x > 5)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    //this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed * Time.fixedDeltaTime);

                    this.transform.position = this.transform.position - new Vector3(-this.moveSpeed * Time.fixedDeltaTime, 0, 0);
                    if (this.tileManager.runDirection == TrackDirection.negativeX)
                    {
                        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }
                    break;
                }
            case TrackDirection.negativeZ:
                {
                    if (this.transform.position.z > 5)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    //this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed * Time.fixedDeltaTime);

                    this.transform.position = this.transform.position - new Vector3(0, 0, -this.moveSpeed * Time.fixedDeltaTime);
                    if (this.tileManager.runDirection == TrackDirection.negativeZ)
                    {
                        //this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);
                    }
                    break;
                }
            case TrackDirection.positiveX:
                {
                    if (this.transform.position.x < -5)
                    {
                        this.tileManager.SpawnAdditionalTile();
                        Destroy(this.gameObject);
                    }
                    //this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed * Time.fixedDeltaTime);

                    this.transform.position = this.transform.position - new Vector3(this.moveSpeed * Time.fixedDeltaTime, 0, 0);
                    if (this.tileManager.runDirection == TrackDirection.positiveX)
                    {
                        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
                    }

                    break;
                }
        }
    }

}
