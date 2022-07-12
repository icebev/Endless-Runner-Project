using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnWeightings : MonoBehaviour
{
    [System.Serializable]
    public struct NextTileSpawnProbabilities
    {
        public float easyChance;
        public float mediumChance;
        public float hardChance;
    }

    [System.Serializable]
    public struct SpawnProbabilitySet
    {
        public float maxSetTime;
        public NextTileSpawnProbabilities wasEasyTile;
        public NextTileSpawnProbabilities wasMediumTile;
        public NextTileSpawnProbabilities wasHardTile;
    }

    public float fillerTileChance;
    public SpawnProbabilitySet[] spawnProbabilitySets;


    public TileDifficulty GenerateNextTileDifficulty(TileDifficulty lastTileDifficulty, int setIndex)
    {
        NextTileSpawnProbabilities chancesForNextTile;

        switch (lastTileDifficulty)
        {
            case TileDifficulty.Easy:
            { 
                chancesForNextTile = this.spawnProbabilitySets[setIndex].wasEasyTile;
                break;
            }
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
