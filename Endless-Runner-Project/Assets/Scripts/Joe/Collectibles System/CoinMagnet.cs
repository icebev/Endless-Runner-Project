using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* COIN MAGNET CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * 
 */
/// <summary>
/// Handles coin pulling effect when the player collects the coin magnet power-up
/// </summary>
public class CoinMagnet : MonoBehaviour
{
    private List<GameObject> caughtCoins;
    private float coinMoveSpeed;
    [Tooltip("At faster tile speeds, the coins move towards the player faster based on this curve.")]
    [SerializeField] private AnimationCurve coinMoveSpeedCurve;
    private TileSpeedManagement tileSpeedManagement;

    // Start is called before the first frame update
    void Start()
    {
        this.caughtCoins = new List<GameObject>();
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
    }

    void FixedUpdate()
    {
        this.coinMoveSpeed = this.coinMoveSpeedCurve.Evaluate(this.tileSpeedManagement.CurrentTileSpeed);

        // Removes and incative caught coins from the caughtCoins array
        for (int i = 0; i < this.caughtCoins.Count; i++)
        {
            if (this.caughtCoins[i].activeInHierarchy == false)
            {
                this.caughtCoins.RemoveAt(i);
                i--;
            }
        }

        // Magnet effect: Move all caught coins towards the player
        if (this.caughtCoins.Count > 0)
        {
            foreach (GameObject coin in this.caughtCoins)
            {
                this.MoveCoinTowardsPlayer(coin);
            }
        }
    }

    // Once a coin enters the magnet's radius, it is considered caught and is added to the caught coins list
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            this.caughtCoins.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            this.caughtCoins.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// Moves the coin towards the player based on the coinMoveSpeed
    /// </summary>
    /// <param name="coinObject"></param>
    private void MoveCoinTowardsPlayer(GameObject coinObject)
    {
        Vector3 newCoinPosition = Vector3.MoveTowards(coinObject.transform.position, this.transform.position, this.coinMoveSpeed);
        coinObject.transform.position = newCoinPosition;
    }
}
