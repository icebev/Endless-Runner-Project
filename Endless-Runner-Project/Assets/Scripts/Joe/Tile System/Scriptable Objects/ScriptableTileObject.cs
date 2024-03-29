using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* SCRIPTABLE TILE OBJECT (SCRIPTABLE OBJECT) CLASS
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// For the creation and cofiguaration of tile objects to avoid 
/// having unneccesary information on the instantiated tile prefabs themselves 
/// and to give greater creative freedom to the designers when setting up the tiles.
/// </summary>

[CreateAssetMenu(fileName = "New Tile", menuName = "ScriptableTileObject")]
public class ScriptableTileObject : ScriptableObject
{
    [Header("Tile Configuration Parameters")]
    [Tooltip("The tile prefab containing 3D models and the basic tile movement script.")]
    public GameObject tilePrefab;

    [Tooltip("Difficulty rating used to determine which type of tile should spawn next.")]
    public TileDifficulty difficulty;

    [Tooltip("The multiple of the tile square dimension that the tile is.")]
    public int tileLength;

    [Tooltip("Relative probability of this tile being chosen out of tiles with the same difficulty rating")]
    public float spawnProbability;

    [Tooltip("Is this tile a corner tile?")]
    public bool hasCorner;

    [Tooltip("Is this tile a junction tile?")]
    public bool hasJunction;

    [Tooltip("The change in elevation over the length of the tile.")]
    public float elevationChange;
}
