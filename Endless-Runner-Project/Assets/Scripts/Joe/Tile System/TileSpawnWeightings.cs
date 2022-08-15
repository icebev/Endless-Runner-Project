using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TILE SPAWN WEIGHTINGS CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * 
 */
/// <summary>
/// Determines which difficulty of tile should spawn next using inspector-led weighting values
/// </summary>

public class TileSpawnWeightings : MonoBehaviour
{
    /// <summary>
    /// Struct containing a float probability of each tile difficulty
    /// </summary>
    [System.Serializable]
    public struct NextTileSpawnProbabilities
    {
        public float easyChance;
        public float mediumChance;
        public float hardChance;
    }

    /// <summary>
    /// A set of tile spawn probability sets arranged based on the previous tile's difficulty.
    /// </summary>
    [System.Serializable]
    public struct SpawnProbabilitySet
    {
        // Using maxSetTime allows for the spawn probability sets to be changed over time
        // so that the game becomes more difficult
        [Tooltip("Time before the next set becomes active.")]
        public float maxSetTime;
        public NextTileSpawnProbabilities wasEasyTile;
        public NextTileSpawnProbabilities wasMediumTile;
        public NextTileSpawnProbabilities wasHardTile;
    }

    [Tooltip("The chance of any given tile spawn being a filler.")]
    public float fillerTileChance;
    public SpawnProbabilitySet[] spawnProbabilitySets;

    /// <summary>
    /// Generates a difficulty based on passed in information about the previous tile difficulty
    /// </summary>
    /// <param name="lastTileDifficulty">The difficulty value of the previous.</param>
    /// <param name="setIndex">The spawnProbablitySet to use </param>
    /// <returns></returns>
    public TileDifficulty GenerateNextTileDifficulty(TileDifficulty lastTileDifficulty, int setIndex)
    {
        NextTileSpawnProbabilities chancesForNextTile;

        // Get the correct set of probabilities and store it locally
        switch (lastTileDifficulty)
        {
            case TileDifficulty.Easy:
                chancesForNextTile = this.spawnProbabilitySets[setIndex].wasEasyTile;
                break;
            case TileDifficulty.Medium:
                chancesForNextTile = this.spawnProbabilitySets[setIndex].wasMediumTile;
                break;
            case TileDifficulty.Hard:
                chancesForNextTile = this.spawnProbabilitySets[setIndex].wasHardTile;
                break;
            default:
                chancesForNextTile = this.spawnProbabilitySets[setIndex].wasEasyTile;
                break;
        }

        // Use a randomly generated value to choose a tile difficulty to return
        float randomVal = Random.Range(0.0f, 1.0f - this.fillerTileChance);

        if (randomVal <= chancesForNextTile.easyChance)
        {
            return TileDifficulty.Easy;
        }
        else if (randomVal <= chancesForNextTile.easyChance + chancesForNextTile.mediumChance)
        {
            return TileDifficulty.Medium;
        }
        else if (randomVal <= chancesForNextTile.easyChance + chancesForNextTile.mediumChance + chancesForNextTile.hardChance)
        {
            return TileDifficulty.Hard;
        }
        else
        {
            return TileDifficulty.Filler;
        }
    }
}
