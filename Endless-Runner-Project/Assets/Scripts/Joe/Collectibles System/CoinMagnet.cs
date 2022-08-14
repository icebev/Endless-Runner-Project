using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    public List<GameObject> caughtCoins;
    public float coinMoveSpeed;
    public AnimationCurve coinMoveSpeedCurve;
    public TileSpeedManagement tileSpeedManagement;
    // Start is called before the first frame update
    void Start()
    {
        this.caughtCoins = new List<GameObject>();
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.coinMoveSpeed = this.coinMoveSpeedCurve.Evaluate(this.tileSpeedManagement.currentTileSpeed);

        for (int i = 0; i < this.caughtCoins.Count; i++)
        {
            if (this.caughtCoins[i].activeInHierarchy == false)
            {
                this.caughtCoins.RemoveAt(i);
                i--;
            }
        }

        if (this.caughtCoins.Count > 0)
        {
            foreach (GameObject coin in this.caughtCoins)
            {
                this.MoveCoinTowardsPlayer(coin);
            }
        }
    }

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

    private void MoveCoinTowardsPlayer(GameObject coinObject)
    {
        Vector3 newCoinPosition = Vector3.MoveTowards(coinObject.transform.position, this.transform.position, this.coinMoveSpeed);
        coinObject.transform.position = newCoinPosition;
    }
}
