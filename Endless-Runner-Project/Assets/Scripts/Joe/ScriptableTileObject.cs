using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Tooltip("The change in elevation over the length of the tile.")]
    public float elevationChange;
}
