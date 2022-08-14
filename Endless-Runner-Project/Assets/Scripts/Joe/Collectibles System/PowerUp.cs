using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleEventFunctions.OnPowerUpCollect.Invoke(this.powerUpType);
            Destroy(this.gameObject);
        }
    }
}
