using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* TILE MANAGER CLASS
 * Author(s): Joe Bevis
 * Date last modified: 03/07/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Added a filler tile list.
 * 
 */
/// <summary>
/// A class for spawning the tile pieces in the correct position and orientation and then managing them.
/// </summary>
public class TileManager : MonoBehaviour
{


    #region Configurable Inspector Variables
    [Header("Tile Scriptable Objects")]
    [SerializeField] private ScriptableTileObject[] scriptableTiles;
   
    [Header("Parameters")]
    [Tooltip("The number of tiles that exist in a given moment.")]
    [SerializeField] private int tileSpawnCount;
    [Tooltip("The square size of a single tile.")]
    [SerializeField] private float squareTileDimension;
    [Tooltip("How far behind the player the tile is before being destroyed.")]
    public float despawnDistance;
    [Tooltip("Chance of a corner tile being spawned when they are able to.")]
    [SerializeField] private float cornerProbability;
    [Tooltip("Chance of a junction tile being spawned when they are able to.")]
    [SerializeField] private float junctionProbability;
    #endregion

    #region Tile Lists
    private List<ScriptableTileObject> fillerTilesList;
    private List<ScriptableTileObject> veryEasyTilesList;
    private List<ScriptableTileObject> easyTilesList;
    private List<ScriptableTileObject> mediumTilesList;
    private List<ScriptableTileObject> hardTilesList;
    private List<ScriptableTileObject> cornerTilesList;
    private List<ScriptableTileObject> junctionTilesList;
    #endregion

    #region Hidden Public Variables
    [HideInInspector] public bool spawningAfterJunction = false;
    [HideInInspector] public List<GameObject> junctionSpawnedTilesList;
    [HideInInspector] public TrackDirection spawnDirection = TrackDirection.positiveZ;
    [HideInInspector] public TrackDirection runDirection = TrackDirection.positiveZ;
    [HideInInspector] public GameObject tilesContainer;
    [HideInInspector] public GameObject currentJunctionTile;
    #endregion

    #region Private Variables
    private TileSpeedIncrementation tileSpeedIncrementation;
    private GameObject finalTile;
    private float spawnHeightChange;
    private float nextTileSpawnGap;
    #endregion

    public float CurrentTileSpeed
    {
        get { return this.tileSpeedIncrementation.currentTileSpeed; }
    }

    private void Start()
    {
        // Reference speed incrementation script
        this.tileSpeedIncrementation = GetComponent<TileSpeedIncrementation>();

        // List instantiation
        this.fillerTilesList = new List<ScriptableTileObject>();
        this.veryEasyTilesList = new List<ScriptableTileObject>();
        this.easyTilesList = new List<ScriptableTileObject>();
        this.mediumTilesList = new List<ScriptableTileObject>();
        this.hardTilesList = new List<ScriptableTileObject>();
        this.cornerTilesList = new List<ScriptableTileObject>();
        this.junctionTilesList = new List<ScriptableTileObject>();

        this.junctionSpawnedTilesList = new List<GameObject>();

        // Fill each list
        foreach (ScriptableTileObject tileObject in this.scriptableTiles)
        {
            if (tileObject.hasCorner == false && tileObject.hasJunction == false)
            {
                switch (tileObject.difficulty)
                {
                    case TileDifficulty.VeryEasy:
                        this.veryEasyTilesList.Add(tileObject);
                        break;
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
            else if (tileObject.hasCorner == true)
            {
                this.cornerTilesList.Add(tileObject);
            }
            else if (tileObject.hasJunction == true)
            {
                this.junctionTilesList.Add(tileObject);
            }
        }

        this.tilesContainer = GameObject.FindGameObjectWithTag("TilesContainer");
        this.nextTileSpawnGap = this.squareTileDimension;

        this.finalTile = InstantiateSemiRandomTile();
        this.finalTile.transform.Translate(new Vector3(0, 4.5f, 0));
        // Spawn in the starting tile sequence
        for (int z = 0; z <= this.tileSpawnCount; z++)
        {
            this.SpawnAdditionalTile();
            //Vector3 tilePos = this.veryEasyTilesList[0].tilePrefab.transform.position + new Vector3(0, this.spawnHeightChange, (z * this.squareTileDimension));
            //GameObject newTile = Instantiate(this.veryEasyTilesList[0].tilePrefab, tilePos, this.veryEasyTilesList[0].tilePrefab.transform.rotation);
            //newTile.transform.parent = this.tilesContainer.transform;
            //if (z == this.tileSpawnCount)
            //{
            //    this.finalTile = newTile;
            //}
        }
        //this.spawnHeightChange = 0;
    }


    public Vector3 CalculateSpawnPosition()
    {
        Vector3 spawnPos = new Vector3();

        switch (this.spawnDirection)
        {
            case TrackDirection.positiveZ:
                spawnPos = this.finalTile.transform.position + new Vector3(0, this.spawnHeightChange, this.nextTileSpawnGap);
                break;
            case TrackDirection.negativeX:
                spawnPos = this.finalTile.transform.position + new Vector3(-this.nextTileSpawnGap, this.spawnHeightChange, 0);
                break;
            case TrackDirection.negativeZ:
                spawnPos = this.finalTile.transform.position + new Vector3(0, this.spawnHeightChange, -this.nextTileSpawnGap);
                break;
            case TrackDirection.positiveX:
                spawnPos = this.finalTile.transform.position + new Vector3(this.nextTileSpawnGap, this.spawnHeightChange, 0);
                break;
        }

        // Account for frame delay caused offset
        switch (this.runDirection)
        {
            case TrackDirection.positiveZ:
                spawnPos += new Vector3(0, 0, -Time.fixedDeltaTime * this.CurrentTileSpeed);
                break;
            case TrackDirection.negativeX:
                spawnPos += new Vector3(Time.fixedDeltaTime * this.CurrentTileSpeed, 0, 0);
                break;
            case TrackDirection.negativeZ:
                spawnPos += new Vector3(0, 0, Time.fixedDeltaTime * this.CurrentTileSpeed);
                break;
            case TrackDirection.positiveX:
                spawnPos += new Vector3(-Time.fixedDeltaTime * this.CurrentTileSpeed, 0, 0);
                break;
        }
        return spawnPos;
    }

    public GameObject InstantiateSemiRandomTile()
    {
        ScriptableTileObject chosenScriptableTile;
        GameObject spawnedTile;
        float randomVal = Random.Range(0.0f, 1.0f);
        if (randomVal < this.cornerProbability && this.spawnDirection == this.runDirection && this.spawningAfterJunction == false)
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
        else if (randomVal < (this.junctionProbability + this.cornerProbability) && this.spawnDirection == this.runDirection && this.spawningAfterJunction == false)
        {
            chosenScriptableTile = this.SelectJunctionTile();
            spawnedTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
            this.spawningAfterJunction = true;
            return spawnedTile;

        }
        else
        {
            chosenScriptableTile = this.SelectTileByDifficulty(TileDifficulty.VeryEasy);
        }

        spawnedTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
        this.nextTileSpawnGap = chosenScriptableTile.tileLength * this.squareTileDimension;
        spawnedTile.transform.parent = this.tilesContainer.transform;

        if (this.spawningAfterJunction)
        {
            this.junctionSpawnedTilesList.Add(spawnedTile);
        }




        return spawnedTile;
    }

    public void ResetTilesContainer()
    {
        foreach (Transform child in this.transform)
        {
            SpawnAdditionalTile();
            Destroy(child);
        }
    }
    /// <summary>
    /// Creates an additional tile at the end of the track
    /// </summary>
    public void SpawnAdditionalTile()
    {
        // Calculate where the new tile should spawn - at newSpawnPos
        Vector3 newSpawnPos = CalculateSpawnPosition();

        this.nextTileSpawnGap = this.squareTileDimension;
        this.spawnHeightChange = 0;
        
        // Rotation correction based on current track spawn direction
        Quaternion newSpawnRot = Quaternion.AngleAxis(-90 * (int)this.spawnDirection, Vector3.up);

        // Semi randomised selection of a tile prefab to spawn
        GameObject newTile = InstantiateSemiRandomTile();

        // Position and rotation are updated after the tile is instantiated but not calculated after since corners will change track spawn direction
        newTile.transform.position = newSpawnPos;
        newTile.transform.rotation = newSpawnRot;

        // Final new tile setup
        // Set as a child of the container gameobject
    

        
        this.finalTile = newTile;
        if(this.currentJunctionTile == null && this.spawningAfterJunction == true)
        {
            this.currentJunctionTile = newTile;
        }

        if (newTile.GetComponent<TileMovement>() == null)
        {
            newTile.AddComponent<TileMovement>();
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

        ScriptableTileObject selectedScriptableTileObject = this.SelectTileFromWeightedList(this.cornerTilesList);

        return selectedScriptableTileObject;
    }


    private ScriptableTileObject SelectJunctionTile()
    {

        ScriptableTileObject selectedScriptableTileObject = this.SelectTileFromWeightedList(this.junctionTilesList);

        return selectedScriptableTileObject;
    }

    private ScriptableTileObject SelectTileFromWeightedList(List<ScriptableTileObject> scriptableTilesList)
    {
        float weightingsTotal = 0.0f;

        foreach (ScriptableTileObject scriptableTile in scriptableTilesList)
        {
            weightingsTotal += scriptableTile.spawnProbability;
        }

        float randomSelector = Random.Range(0.0f, weightingsTotal);

        int selectedIndex = -1;
        while (randomSelector >= 0.0f && selectedIndex < scriptableTilesList.Count - 1)
        {
            selectedIndex++;
            randomSelector -= scriptableTilesList[selectedIndex].spawnProbability;
        }

        return scriptableTilesList[selectedIndex];
    }

    private ScriptableTileObject SelectTileByDifficulty(TileDifficulty difficulty)
    {
        List<ScriptableTileObject> selectedList = null;

        switch (difficulty)
        {
            case TileDifficulty.Filler:
                selectedList = this.veryEasyTilesList;
                break;
            case TileDifficulty.VeryEasy:
                selectedList = this.veryEasyTilesList;
                break;
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

        ScriptableTileObject selectedScriptableTileObject = this.SelectTileFromWeightedList(selectedList);

        return selectedScriptableTileObject;
    }

}
