using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* TILE MANAGER CLASS
 * Author(s): Joe Bevis
 * Date last modified: 17/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Added a filler tile list.
 * Removed the VeryEasy tile list since this difficulty level is no longer used.
 * Removed an unused reference to the current junction tile
 */
/// <summary>
/// A class for spawning the tile pieces in the correct position and orientation and then managing them.
/// </summary>
public class TileManager : MonoBehaviour
{
    #region Configurable Inspector Variables
    [Header("Tile Scriptable Objects")]
    [SerializeField] private ScriptableTileObject[] scriptableTiles;
    [SerializeField] private ScriptableTileObject startingTile;
   
    [Header("Parameters")]
    [Tooltip("The number of tiles that exist in a given moment.")]
    [SerializeField] private int tileSpawnCount;
    [Tooltip("The number of filler tiles that spawn in a row at the beginning.")]
    [SerializeField] private int initialFillerCount;
    [Tooltip("The number of filler tiles that spawn in a row at the beginning if the tutorial is going to play.")]
    [SerializeField] private int initialTutorialFillerCount;
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
    #endregion

    #region Private Variables
    private TileSpeedManagement tileSpeedManagement;
    private TileSpawnWeightings tileSpawnWeightings;
    private GameObject finalTile;
    private GameObject tilesContainer;

    private float spawnHeightChange;
    private float nextTileSpawnGap;
    private TileDifficulty lastNonFillerTileDifficulty;
    private int spawnFillersNextCount;
    private float runPlaytime;
    #endregion

    // Tile speed getter ensures that tile speed is not accidentally changed upon access
    public float GetCurrentTileSpeed
    {
        get { return this.tileSpeedManagement.CurrentTileSpeed; }
    }

    private void Start()
    {
        // Run play time is used to select an increasingly difficult spawn probability set as time passes
        this.runPlaytime = 0.0f;

        // Get references to other related scripts
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.tileSpawnWeightings = FindObjectOfType<TileSpawnWeightings>();

        // List instantiation
        this.fillerTilesList = new List<ScriptableTileObject>();
        this.easyTilesList = new List<ScriptableTileObject>();
        this.mediumTilesList = new List<ScriptableTileObject>();
        this.hardTilesList = new List<ScriptableTileObject>();
        this.cornerTilesList = new List<ScriptableTileObject>();
        this.junctionTilesList = new List<ScriptableTileObject>();
        this.junctionSpawnedTilesList = new List<GameObject>();

        // Fill each list with the scriptable tile objects of the matching difficulty setting
        foreach (ScriptableTileObject tileObject in this.scriptableTiles)
        {
            if (tileObject.hasCorner == false && tileObject.hasJunction == false)
            {
                switch (tileObject.difficulty)
                {
                    case TileDifficulty.Filler:
                        this.fillerTilesList.Add(tileObject);
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
            // Separate lists are used for corner and junction tiles.
            else if (tileObject.hasCorner == true)
            {
                this.cornerTilesList.Add(tileObject);
            }
            else if (tileObject.hasJunction == true)
            {
                this.junctionTilesList.Add(tileObject);
            }
        }

        // The tiles container will contain the instantiated tiles as children to keep the hierarchy tidy.
        this.tilesContainer = GameObject.FindGameObjectWithTag("TilesContainer");
        
        this.nextTileSpawnGap = this.squareTileDimension;

        this.lastNonFillerTileDifficulty = TileDifficulty.Easy;

        // Instantiate the starting tile at the origin
        this.finalTile = Instantiate(this.startingTile.tilePrefab, this.startingTile.tilePrefab.transform.position, this.startingTile.tilePrefab.transform.rotation);

        // Depending on whether the tutorial is complete, set an amount of filler (blank) tiles to spawn next
        // We need more blank tiles if the tutorial is going to take place
        // so that the player doesn't encounter obstacles before they have learned the controls.
        if (PlayerPrefs.GetInt("TutorialComplete") == 1)
        {
            this.spawnFillersNextCount = this.initialFillerCount;
        }
        else
        {
            this.spawnFillersNextCount = this.initialTutorialFillerCount;
        }

        // Spawn in the starting tile sequence based on the configured spawn count paramater
        for (int z = 0; z <= this.tileSpawnCount; z++)
        {
            this.SpawnAdditionalTile();
        }
    }

    private void Update()
    {
        this.runPlaytime += Time.deltaTime;
    }

    /// <summary>
    /// Creates an additional tile at the end of the track. 
    /// To be called at on start and when tiles despawn to create an endless treadmill.
    /// </summary>
    public void SpawnAdditionalTile()
    {
        // Calculate where the new tile should spawn - at newSpawnPos
        Vector3 newSpawnPos = CalculateSpawnPosition();

        this.nextTileSpawnGap = this.squareTileDimension;
        this.spawnHeightChange = 0;

        // Rotation correction based on current track spawn direction
        // This works due to how the track direction enum is structured, as each consecutive direction is orthoganal (at 90 degrees)
        Quaternion newSpawnRot = Quaternion.AngleAxis(-90 * (int)this.spawnDirection, Vector3.up);

        // Semi randomised selection of a tile prefab to spawn.
        GameObject newTile = InstantiateSemiRandomTile();

        // Position and rotation are updated after the tile is instantiated but not calculated after since corners will change track spawn direction
        newTile.transform.position = newSpawnPos;
        newTile.transform.rotation = newSpawnRot;

        // Final new tile setup
        // Set as a child of the container gameobject
        newTile.transform.parent = this.tilesContainer.transform;

        this.finalTile = newTile;

        // Failsafe addition of the tile movement script.
        if (newTile.GetComponent<TileMovement>() == null)
        {
            newTile.AddComponent<TileMovement>();
        }
    }

    #region Tile Spawn Subroutines
    /// <summary>
    /// Calculates the correct point that the new tile should spawn 
    /// based on the position of the last tile in the treadmill and the current spawn direction. 
    /// </summary>
    /// <returns>The spawn position.</returns>
    private Vector3 CalculateSpawnPosition()
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

        // Account for frame delay caused offset as all of the tiles move against the player's run direction
        switch (this.runDirection)
        {
            case TrackDirection.positiveZ:
                spawnPos += new Vector3(0, 0, -Time.fixedDeltaTime * this.GetCurrentTileSpeed);
                break;
            case TrackDirection.negativeX:
                spawnPos += new Vector3(Time.fixedDeltaTime * this.GetCurrentTileSpeed, 0, 0);
                break;
            case TrackDirection.negativeZ:
                spawnPos += new Vector3(0, 0, Time.fixedDeltaTime * this.GetCurrentTileSpeed);
                break;
            case TrackDirection.positiveX:
                spawnPos += new Vector3(-Time.fixedDeltaTime * this.GetCurrentTileSpeed, 0, 0);
                break;
        }
        return spawnPos;
    }

    /// <summary>
    /// Creates a new tile at the origin based on the tile spawn probabilities set up.
    /// </summary>
    private GameObject InstantiateSemiRandomTile()
    {
        ScriptableTileObject chosenScriptableTile;

        GameObject spawnedTile;

        float randomVal = Random.Range(0.0f, 1.0f);

        // Only spawn a corner if strict conditions are met:
        /* The spawn direction is the same as the current running direction - otherwise the track would become too twisty.
         * Cannot spawn after a junction until the junction has been passed.
         * Cannot spawn if a filler tile should spawn next.
         */
        if (randomVal < this.cornerProbability && this.spawnDirection == this.runDirection 
            && this.spawningAfterJunction == false && this.spawnFillersNextCount <= 0)
        {
            chosenScriptableTile = this.SelectCornerTile();

            // Always spawn a filler tile after a corner because otherwise
            // obstacles would be blocked from view which is unfair for the player.
            this.spawnFillersNextCount = 1;

            // Change spawn direction based on the turn direction so that
            // new tiles spawn in the correct orientation after the turn
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

            spawnedTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
            return spawnedTile;
        }
        // Only spawn a junction tile if strict conditions are met, similar to a corner.
        else if (randomVal < (this.junctionProbability + this.cornerProbability) && this.spawnDirection == this.runDirection 
            && this.spawningAfterJunction == false && this.spawnFillersNextCount <= 0)
        {
            chosenScriptableTile = this.SelectJunctionTile();

            this.spawnFillersNextCount = 1;
            this.spawningAfterJunction = true;

            spawnedTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
            return spawnedTile;

        }
        // Regular tile spawn (not a corner or a junction)
        else
        {
            TileDifficulty newTileDifficulty = this.DetermineNextTileDifficulty(this.lastNonFillerTileDifficulty);
            if (newTileDifficulty != TileDifficulty.Filler)
            {
                this.lastNonFillerTileDifficulty = newTileDifficulty;
            }
            chosenScriptableTile = this.SelectTileByDifficulty(newTileDifficulty);
            spawnedTile = Instantiate(chosenScriptableTile.tilePrefab, chosenScriptableTile.tilePrefab.transform.position, chosenScriptableTile.tilePrefab.transform.rotation);
            
            // If the tile has a length greater than 1, the nextTileSpawn gap is used to accommodate it.
            this.nextTileSpawnGap = chosenScriptableTile.tileLength * this.squareTileDimension;

            // The junction spawned tiles list is used to rotate
            // the tiles around the junction when the player turns at the junction
            if (this.spawningAfterJunction)
            {
                this.junctionSpawnedTilesList.Add(spawnedTile);
            }
            return spawnedTile;
        }
    }

    /// <summary>
    /// Get the semi-random difficulty value of the next tile 
    /// based on the spawn probabilities set up.
    /// </summary>
    /// <param name="lastTileDifficulty">The difficulty of the previous tile.</param>
    /// <returns>The difficulty the next tile should spawn at.</returns>
    private TileDifficulty DetermineNextTileDifficulty(TileDifficulty lastTileDifficulty)
    {
        float randomVal = Random.Range(0.0f, 1.0f);

        // Should the next tile be a filler (blank) tile?
        if (randomVal <= this.tileSpawnWeightings.fillerTileChance || this.spawnFillersNextCount > 0)
        {
            if (this.spawnFillersNextCount > 0)
            {
                this.spawnFillersNextCount--;
            }
            return TileDifficulty.Filler;
        }
        else
        {
            // Select which set should be used to get the spawn probabilities from -
            // more difficult sets are chosen later in the game to increase the overall difficulty.
            int currentProbabilitySetIndex = 0;
            while (this.runPlaytime > this.tileSpawnWeightings.spawnProbabilitySets[currentProbabilitySetIndex].maxSetTime)
            {
                currentProbabilitySetIndex++;
            }

            TileDifficulty nextTileDifficulty = this.tileSpawnWeightings.GenerateNextTileDifficulty(lastTileDifficulty, currentProbabilitySetIndex);
            return nextTileDifficulty;
        }

    }

    /// <summary>
    /// Set the track spawn direction to turn left 
    /// </summary>
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

    /// <summary>
    /// Set the track spawn direction to turn right 
    /// </summary>
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
    #endregion

    #region Tile Scriptable Object Selection Methods

    /// <summary>
    /// Select a corner tile from the available corner tiles.
    /// </summary>
    /// <returns>A corner tile scriptable object.</returns>
    private ScriptableTileObject SelectCornerTile()
    {
        ScriptableTileObject selectedScriptableTileObject = this.SelectTileFromWeightedList(this.cornerTilesList);

        return selectedScriptableTileObject;
    }

    /// <summary>
    /// Select a junction tile from the available junction tiles.
    /// </summary>
    /// <returns>A junction tile scriptable object.</returns>
    private ScriptableTileObject SelectJunctionTile()
    {
        ScriptableTileObject selectedScriptableTileObject = this.SelectTileFromWeightedList(this.junctionTilesList);

        return selectedScriptableTileObject;
    }
    
    /// <summary>
    /// Select a tile at random from a weighted list.
    /// </summary>
    /// <param name="scriptableTilesList">The list to select from.</param>
    /// <returns>A tile scriptable object.</returns>
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

    /// <summary>
    /// Select a random tile by difficulty.
    /// </summary>
    /// <param name="difficulty">The difficulty of the tile to be selected.</param>
    /// <returns>A random tile of the passed in difficulty.</returns>
    private ScriptableTileObject SelectTileByDifficulty(TileDifficulty difficulty)
    {
        List<ScriptableTileObject> selectedList = null;

        switch (difficulty)
        {
            case TileDifficulty.Filler:
                selectedList = this.fillerTilesList;
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

    #endregion
}
