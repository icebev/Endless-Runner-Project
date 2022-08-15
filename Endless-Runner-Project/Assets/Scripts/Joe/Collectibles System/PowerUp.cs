using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* POWER UP CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// Enables the powerup to be collected upon trigger enter by the player
/// </summary>
public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invoke the effect of collecting the function using the public static Unity Event
            CollectibleEventFunctions.OnPowerUpCollect.Invoke(this.powerUpType);
            Destroy(this.gameObject);
        }
    }
}
