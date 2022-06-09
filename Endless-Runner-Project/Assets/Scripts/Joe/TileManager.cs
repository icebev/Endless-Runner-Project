using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class for spawning the tile pieces in the correct position and orientation and then managing them.
/// </summary>
public class TileManager : MonoBehaviour
{

    [Header("Tile Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tileCornerLeft;
    [SerializeField] private GameObject tileCornerRight;
    [SerializeField] private GameObject stairsTile;
    [SerializeField] private GameObject raisedPillarTile;
    
    [Header("Parameters")]
    [Tooltip("The number of tiles that exist in a given moment.")]
    [SerializeField] private int tileSpawnCount;

    [Tooltip("The square size of a single tile.")]
    [SerializeField] private float squareTileDimension;

    [Tooltip("The speed of tile movement opposing the run direction.")]
    [SerializeField] private float startingTileSpeed;
    public float tileSpeed;

    [Tooltip("How far behind the player the tile is before being destroyed.")]
    public float despawnDistance;

    [Tooltip("Factor by which the speed increases (by a multiple of the start speed) over 1 minute of gameplay.")]
    [SerializeField] private float speedIncrementFactor;

    [HideInInspector] public TrackDirection spawnDirection = TrackDirection.positiveZ;
    [HideInInspector] public TrackDirection runDirection = TrackDirection.positiveZ;
    [HideInInspector] public GameObject tilesContainer;
    private Transform finalTileTransform;
    private float spawnHeight = 0;

    private void Start()
    {
        this.tileSpeed = this.startingTileSpeed;
        this.tilesContainer = GameObject.FindGameObjectWithTag("TilesContainer");

        // Spawn in the starting tile sequence
        for (int z = 0; z <= this.tileSpawnCount; z++)
        {
            Vector3 tilePos = this.tilePrefab.transform.position + new Vector3(0, 0, (z * this.squareTileDimension));
            GameObject newTile = Instantiate(this.tilePrefab, tilePos, this.tilePrefab.transform.rotation);
            newTile.transform.parent = this.tilesContainer.transform;
            if (z == this.tileSpawnCount)
            {
                this.finalTileTransform = newTile.transform;
            }
        }
    }

    /// <summary>
    /// Creates an additional tile at the end of the track
    /// </summary>
    public void SpawnAdditionalTile()
    {
        // Calculate where the new tile should spawn - at newSpawnPos
        Vector3 newSpawnPos = new Vector3(0, 0, 0);
        switch (this.spawnDirection)
        {
            case TrackDirection.positiveZ:
                newSpawnPos = this.finalTileTransform.position + new Vector3(0, this.spawnHeight, this.squareTileDimension);
                break;
            case TrackDirection.negativeX:
                newSpawnPos = this.finalTileTransform.position + new Vector3(-this.squareTileDimension, this.spawnHeight, 0);
                break;
            case TrackDirection.negativeZ:
                newSpawnPos = this.finalTileTransform.position + new Vector3(0, this.spawnHeight, -this.squareTileDimension);
                break;
            case TrackDirection.positiveX:
                newSpawnPos = this.finalTileTransform.position + new Vector3(this.squareTileDimension, this.spawnHeight, 0);
                break;
        }
        this.spawnHeight = 0;
        // Account for frame delay caused offset
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

        // Rotation correction based on current track spawn direction
        Quaternion newSpawnRot = Quaternion.AngleAxis(-90 * (int)this.spawnDirection, Vector3.up);

        // Rudimentary random selection of a tile prefab to spawn
        int randInt = Random.Range(0, 11);
        GameObject newTile;
        if (randInt >= 9 && this.spawnDirection == this.runDirection)
        {
            newTile = Instantiate(this.tileCornerLeft, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
            this.TrackSpawnLeftTurn();
        }
        else if (randInt >= 7 && this.spawnDirection == this.runDirection)
        {
            newTile = Instantiate(this.tileCornerRight, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
            this.TrackSpawnRightTurn();
        }
        else if (randInt == 0)
        {
            newTile = Instantiate(this.stairsTile, this.stairsTile.transform.position, this.stairsTile.transform.rotation);
            this.spawnHeight += 0.5f;
        }
        else if (randInt == 1)
        {
            newTile = Instantiate(this.raisedPillarTile, this.raisedPillarTile.transform.position, this.raisedPillarTile.transform.rotation);

        }
        else
        {
            newTile = Instantiate(this.tilePrefab, this.tilePrefab.transform.position, this.tilePrefab.transform.rotation);
        }
        // Position and rotation are updated after the tile is instantiated but not calculated after since corners will change track spawn direction
        newTile.transform.position = newSpawnPos;
        newTile.transform.rotation = newSpawnRot;

        //if ((this.spawnDirection == TrackDirection.positiveX || this.spawnDirection == TrackDirection.negativeX) && newTile.CompareTag("CornerTile") == false)
        //{
        //    newSpawnRot = Quaternion.AngleAxis(90, Vector3.up);
        //    newTile.transform.rotation = newSpawnRot;
        //}
        //else if (newTile.CompareTag("CornerTile") == true)
        //{
        //    switch (this.spawnDirection)
        //    {
        //        case TrackDirection.negativeZ:
        //        {
        //            newSpawnRot = Quaternion.AngleAxis(-90 * (int)this.spawnDirection, Vector3.up);
        //            newTile.transform.rotation = newSpawnRot;
        //            break;
        //        }
        //    }
        //}

        // Final new tile setup
        // Set as a child of the container gameobject
        newTile.transform.parent = this.tilesContainer.transform;
        this.finalTileTransform = newTile.transform;
    }

    private void FixedUpdate()
    {
        this.tileSpeed += (Time.fixedDeltaTime / 60f) * this.startingTileSpeed * this.speedIncrementFactor;
    }
    public void TrackSpawnLeftTurn()
    {
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

    public void TrackSpawnRightTurn()
    {
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
}
