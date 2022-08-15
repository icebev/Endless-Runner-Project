using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TILE COIN SPAWN CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Added the ability to enter coin set spawn probabilities and toggle exclusivity in the inspector
 * Added requirecomponent of type TileMovement to avoid setup errors.
 */
/// <summary>
/// Using set probabilities, places coins from the coin pool onto tiles
/// based on pre-designed patterns ready to be collected by the player.
/// </summary>
[RequireComponent(typeof(TileMovement))]
public class TileCoinSpawn : MonoBehaviour
{
    [SerializeField] private Transform coinContainer;
    private bool[] shouldSpawnCoinSet;
    private int totalCoinSets;
    [SerializeField] private bool exclusiveCoinSets;
    [SerializeField] private float[] coinSetSpawnProbabilities;

    private void Awake()
    {
        this.totalCoinSets = this.coinContainer.childCount;
        this.shouldSpawnCoinSet = new bool[this.totalCoinSets];

        #region Exclusive coin set spawn
        if (this.exclusiveCoinSets == true)
        {
            float probabilitiesTotal = 0.0f;

            foreach (float coinSetProbability in this.coinSetSpawnProbabilities)
            {
                probabilitiesTotal += coinSetProbability;
            }

            float randomSelector = Random.Range(0.0f, probabilitiesTotal);

            int selectedIndex = -1;
            while (randomSelector >= 0.0f && selectedIndex < this.coinSetSpawnProbabilities.Length - 1)
            {
                selectedIndex++;
                randomSelector -= this.coinSetSpawnProbabilities[selectedIndex];
            }

            this.shouldSpawnCoinSet[selectedIndex] = true;
        }
        #endregion

        #region Non-Exclusive coin set spawn
        if (this.exclusiveCoinSets == false)
        {
            for (int i = 0; i < this.coinSetSpawnProbabilities.Length; i++)
            {
                float randomValue = Random.Range(0.0f, 1.0f);
                if (randomValue <= this.coinSetSpawnProbabilities[i])
                {
                    this.shouldSpawnCoinSet[i] = true;
                }
            }
        }
        #endregion

    }

    private void Start()
    {
        for (int i = 0; i < this.totalCoinSets; i++)
        {
            Transform coinSet = this.coinContainer.GetChild(i);

            // Spawn the coins in the set if in awake the set was determined as true 
            if (this.shouldSpawnCoinSet[i] == true)
            {
                for (int c = 0; c < coinSet.childCount; c++)
                {
                    // Get a coin from the pool
                    GameObject coin = CoinPool.SharedInstance.GetPooledObject();

                    if (coin != null)
                    {
                        coin.transform.position = coinSet.GetChild(c).position;
                        coin.transform.parent = coinSet.GetChild(c).transform;
                        coin.SetActive(true);
                    }
                }
            }
        }
    }

    /// <summary> 
    /// Releases the coins back to the object pool - to be called before the tile is destroyed
    /// </summary>
    public void ReleaseCoins()
    {
        for (int i = 0; i < this.totalCoinSets; i++)
        {
            Transform coinSet = this.coinContainer.GetChild(i);

            if (this.shouldSpawnCoinSet[i] == true)
            {
                for (int c = 0; c < coinSet.childCount; c++)
                {
                    Transform coin = coinSet.GetChild(c).GetChild(0);
                    coin.parent = CoinPool.SharedInstance.transform;
                    coin.gameObject.SetActive(false);
                }
            }
        }
    }
}
