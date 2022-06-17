using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class for spawning the tile pieces in the correct position and orientation and then managing them.
/// </summary>
public class TileManager : MonoBehaviour
{
    [Header("Tile Scriptable Objects")]
    public ScriptableTileObject[] scriptableTiles;

    public ScriptableTileObject junctionTile;

    private List<ScriptableTileObject> easyTilesList;
    private List<ScriptableTileObject> mediumTilesList;
    private List<ScriptableTileObject> hardTilesList;
    private List<ScriptableTileObject> cornerTilesList;

    public bool spawningAfterJunction = false;
    public List<GameObject> junctionSpawnedTilesList;
    
    [Header("Parameters")]
    [Tooltip("The number of tiles that exist in a given moment.")]
    [SerializeField] private int tileSpawnCount;

    [Tooltip("The square size of a single tile.")]
    [SerializeField] private float squareTileDimension;
    private float nextTileSpawnGap;

    [Tooltip("How far behind the player the tile is before being destroyed.")]
    public float despawnDistance;

    [Tooltip("Chance of a corner tile being spawned when they are able to.")]
    [SerializeField] private float cornerProbability;
    [Tooltip("Chance of a junction tile being spawned when they are able to.")]
    [SerializeField] private float junctionProbability;

    [HideInInspector] public TrackDirection spawnDirection = TrackDirection.positiveZ;
    [HideInInspector] public TrackDirection runDirection = TrackDirection.positiveZ;
    [HideInInspector] public GameObject tilesContainer;
    [HideInInspector] private TileSpeedIncrementation tileSpeedIncrementation;

    private GameObject finalTile;
    public GameObject currentJunctionTile;
    private float spawnHeightChange = 4.5f;

    public float CurrentTileSpeed
    {
        get { return this.tileSpeedIncrementation.currentTileSpeed; }
    }

    private void Start()
    {
        // Reference speed incrementation script
        this.tileSpeedIncrementation = GetComponent<TileSpeedIncrementation>();

        // List instantiation
        this.easyTilesList = new List<ScriptableTileObject>();
        this.mediumTilesList = new List<ScriptableTileObject>();
        this.hardTilesList = new List<ScriptableTileObject>();
        this.cornerTilesList = new List<ScriptableTileObject>();

        this.junctionSpawnedTilesList = new List<GameObject>();

        // Fill each list
        foreach (ScriptableTileObject tileObject in this.scriptableTiles)
        {
            if (tileObject.hasCorner == false)
            {
                switch (tileObject.difficulty)
                {
                    case TileDifficulty.Easy:
                        this.easyTilesList.Add(tileObject);
                        break;
                    case TileDifficulty.Medium:
                        this.mediumTilesList.Add(tileObject);
                        break;
                    case TileDifficulty.Hard:
                        this.hardTilesList.Add(tileObject);
                        break;
                }
            }
            else
            {
                this.cornerTilesList.Add(tileObject);
            }
        }
        this.tilesContainer = GameObject.FindGameObjectWithTag("TilesContainer");
        this.nextTileSpawnGap = 3.0f;

        // Spawn in the starting tile sequence
        for (int z = 0; z <= this.tileSpawnCount; z++)
        {
            Vector3 tilePos = this.easyTilesList[0].tilePrefab.transform.position + new Vector3(0, this.spawnHeightChange, (z * this.squareTileDimension));
            GameObject newTile = Instantiate(this.easyTilesList[0].tilePrefab, tilePos, this.easyTilesList[0].tilePrefab.transform.rotation);
            newTile.transform.parent = this.tilesContainer.transform;
            if (z == this.tileSpawnCount)
            {
                this.finalTile = newTile;
            }
        }
        this.spawnHeightChange = 0;
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
                newSpawnPos = this.finalTile.transform.position + new Vector3(0, this.spawnHeightChange, this.nextTileSpawnGap);
                break;
            case TrackDirection.negativeX:
                newSpawnPos = this.finalTile.transform.position + new Vector3(-this.nextTileSpawnGap, this.spawnHeightChange, 0);
                break;
            case TrackDirection.negativeZ:
                newSpawnPos = this.finalTile.transform.position + new Vector3(0, this.spawnHeightChange, -this.nextTileSpawnGap);
                break;
            case TrackDirection.positiveX:
                newSpawnPos = this.finalTile.transform.position + new Vector3(this.nextTileSpawnGap, this.spawnHeightChange, 0);
                break;

        }

        this.nextTileSpawnGap = this.squareTileDimension;
        this.spawnHeightChange = 0;
        
        // Account for frame delay caused offset
        switch (this.runDirection)
        {
            case TrackDirection.positiveZ:
                newSpawnPos += new Vector3(0, 0, -Time.fixedDeltaTime * this.CurrentTileSpeed);
                break;
            case TrackDirection.negativeX:
                newSpawnPos += new Vector3(Time.fixedDeltaTime * this.CurrentTileSpeed, 0, 0);
                break;
            case TrackDirection.negativeZ:
                newSpawnPos += new Vector3(0, 0, Time.fixedDeltaTime * this.CurrentTileSpeed);
                break;
            case TrackDirection.positiveX:
                newSpawnPos += new Vector3(-Time.fixedDeltaTime * this.CurrentTileSpeed, 0, 0);
                break;
        }
        

        // Rotation correction based on current track spawn direction
        Quaternion newSpawnRot = Quaternion.AngleAxis(-90 * (int)this.spawnDirection, Vector3.up);

        // Semi randomised selection of a tile prefab to spawn
        GameObject newTile;

        // Position and rotation are updated after the tile is instantiated but not calculated after since corners will change track spawn direction
        ScriptableTileObject chosenScriptableTile;

        float randomVal = Random.Range(0.0f, 1.0f);
        if (randomVal < this.cornerProbability && this.spawnDirection == this.runDirection)
        {
            chosenScriptableTile = this.SelectCornerTile();

            TurnDirection tileTurnDirection = chosenScriptableTile.tilePrefab.GetComponent<CornerTileBehaviour>().turnDirection;
            
            switch (tileTurnDirection)
            {
                case TurnDirection.Left:
                    TrackSpawnLeftTurn();
                    break;
                case TurnDirection.Right:
                    TrackSpawnRightTurn();
                    break;
            }

        }
        else if (randomVal < this.junctionProbability && this.spawnDirection == this.runDirection && this.spawningAfterJunction == false)
        {
            chosenScriptableTile = this.junctionTile;
            this.spawningAfterJunction = true;

        }
        else
        {
            chosenScriptableTile = this.SelectTileByDifficulty(TileDifficulty.Easy);
        }
        newTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
        this.nextTileSpawnGap = chosenScriptableTile.tileLength * this.squareTileDimension;


        newTile.transform.position = newSpawnPos;
        newTile.transform.rotation = newSpawnRot;

        // Final new tile setup
        // Set as a child of the container gameobject
        newTile.transform.parent = this.tilesContainer.transform;
        if (this.spawningAfterJunction)
        {
            this.junctionSpawnedTilesList.Add(newTile);
        }
        
        this.finalTile = newTile;
        if(this.currentJunctionTile == null && this.spawningAfterJunction == true)
        {
            this.currentJunctionTile = newTile;
        }
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

    private ScriptableTileObject SelectCornerTile()
    {

        float weightingsTotal = 0.0f;

        foreach (ScriptableTileObject scriptableTile in this.cornerTilesList)
        {
            weightingsTotal += scriptableTile.spawnProbability;
        }

        float randomSelector = Random.Range(0.0f, weightingsTotal);
        int selectedIndex = -1;

        while (randomSelector > 0.0f && selectedIndex < this.cornerTilesList.Count - 1)
        {
            selectedIndex++;
            randomSelector -= this.cornerTilesList[selectedIndex].spawnProbability;
        }

        return this.cornerTilesList[selectedIndex];
    }

    private ScriptableTileObject SelectTileByDifficulty(TileDifficulty difficulty)
    {
        List<ScriptableTileObject> selectedList = null;

        switch (difficulty)
        {
            case TileDifficulty.Easy:
                selectedList = this.easyTilesList;
                break;
            case TileDifficulty.Medium:
                selectedList = this.mediumTilesList;
                break;
            case TileDifficulty.Hard:
                selectedList = this.hardTilesList;
                break;
        }

        float weightingsTotal = 0.0f;

        foreach (ScriptableTileObject scriptableTile in selectedList)
        {
            weightingsTotal += scriptableTile.spawnProbability;
        }

        float randomSelector = Random.Range(0.0f, weightingsTotal);
        int selectedIndex = -1;

        while (randomSelector > 0.0f && selectedIndex < selectedList.Count - 1)
        {
            selectedIndex++;
            randomSelector -= selectedList[selectedIndex].spawnProbability;
        }

        return selectedList[selectedIndex];
    }

}
