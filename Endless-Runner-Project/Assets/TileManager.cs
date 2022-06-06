using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum TrackDirection
{
    positiveZ,
    negativeX,
    negativeZ,
    positiveX

}

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject tileCornerLeft;
    public GameObject tileCornerRight;
    public GameObject tiles;

    public TrackDirection spawnDirection = TrackDirection.positiveZ;
    public TrackDirection runDirection = TrackDirection.positiveZ;
    public int tileSpawnCount;
    public float squareTileDimension;
    public float tileSpeed;

    private Transform finalTileTransform;

    public UnityEvent createNewTile;

    private float MaxTileDistance
    {
        get { return this.tileSpawnCount * this.squareTileDimension; }
    }

    private void Start()
    {
        for (int z = this.tileSpawnCount; z >= 0; z--)
        {
            Vector3 tilePos = this.tilePrefab.transform.position + new Vector3(0, 0, this.MaxTileDistance - (z * this.squareTileDimension));
            GameObject newTile = Instantiate(this.tilePrefab, tilePos, this.tilePrefab.transform.rotation);
            newTile.transform.parent = this.tiles.transform;
            if (z == 0)
            {
                this.finalTileTransform = newTile.transform;
            }
        }
    }

    public void SpawnAdditionalTile()
    {
        Vector3 newSpawnPos = new Vector3(0,0,0);
        switch (this.spawnDirection)
        {
            case TrackDirection.positiveZ:
                newSpawnPos = this.finalTileTransform.position + new Vector3(0, 0, this.squareTileDimension);
                break;
            case TrackDirection.negativeX:
                newSpawnPos = this.finalTileTransform.position + new Vector3(-this.squareTileDimension, 0, 0);
                break;
            case TrackDirection.negativeZ:
                newSpawnPos = this.finalTileTransform.position + new Vector3(0, 0, -this.squareTileDimension);
                break;
            case TrackDirection.positiveX:
                newSpawnPos = this.finalTileTransform.position + new Vector3(this.squareTileDimension, 0, 0);
                break;
        }

        switch (this.runDirection)
        {
            case TrackDirection.positiveZ:
                newSpawnPos += new Vector3(0, 0, -Time.fixedDeltaTime * this.tileSpeed);
                break;
            case TrackDirection.negativeX:
                newSpawnPos += new Vector3(Time.fixedDeltaTime * this.tileSpeed, 0, 0);
                break;
            case TrackDirection.negativeZ:
                newSpawnPos += new Vector3(0, 0, Time.fixedDeltaTime * this.tileSpeed);
                break;
            case TrackDirection.positiveX:
                newSpawnPos += new Vector3(-Time.fixedDeltaTime * this.tileSpeed, 0, 0);
                break;
        }

        int randInt = Random.Range(0, 11);
        GameObject newTile;
        if (randInt > 8 && this.spawnDirection == this.runDirection)
        {
            newTile = Instantiate(this.tileCornerLeft, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
            int currentDirInt = (int)this.spawnDirection;
            if (currentDirInt == 3)
            {
                this.spawnDirection = (TrackDirection)(0);

            }
            else
            {
                this.spawnDirection = (TrackDirection)(currentDirInt + 1);

            }
        }
        else if (randInt > 5 && this.spawnDirection == this.runDirection)
        {
            newTile = Instantiate(this.tileCornerRight, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
            int currentDirInt = (int)this.spawnDirection;
            if (currentDirInt == 0)
            {
                this.spawnDirection = (TrackDirection)(3);

            }
            else
            {
                this.spawnDirection = (TrackDirection)(currentDirInt - 1);

            }
        }
        else
        {
            newTile = Instantiate(this.tilePrefab, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
        }
        newTile.transform.parent = this.tiles.transform;

        Quaternion newSpawnRot;
        if ((this.spawnDirection == TrackDirection.positiveX || this.spawnDirection == TrackDirection.negativeX) && newTile.CompareTag("CornerTile") == false)
        {
            newSpawnRot = Quaternion.AngleAxis(90, Vector3.up);
            newTile.transform.rotation = newSpawnRot;
        }
        else if (newTile.CompareTag("CornerTile") == true)
        {
            switch (this.spawnDirection)
            {
                case TrackDirection.negativeZ:
                {
                    newSpawnRot = Quaternion.AngleAxis(90, Vector3.up);
                    newTile.transform.rotation = newSpawnRot;
                    break;
                }


            }

        }
        newTile.transform.position = newSpawnPos;
        this.finalTileTransform = newTile.transform;
    }

}
