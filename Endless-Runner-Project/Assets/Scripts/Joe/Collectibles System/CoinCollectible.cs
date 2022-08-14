using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* COIN COLLECTIBLE CLASS
 * Author(s): Joe Bevis
 * Date last modified: 14/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Removed use of a public static value for the multiplier 
 * The multiplier is now contained within the CollectibleEventFunctions class
 */
/// <summary>
/// Attached to each coin object to add trigger volume behaviour when the player is detected to collect the coin
/// </summary>
public class CoinCollectible : MonoBehaviour
{
    [Tooltip("How much the coin is worth.")]
    [SerializeField] private int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleEventFunctions.OnCoinCollect.Invoke(this.coinValue);
            // Move the coin off screen to make it seem like it has disappeared without having to destroy it fully
            this.transform.Translate(new Vector3(0, -300, 0), Space.World);
        }
    }
}
