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
            CollectableEventFunctions.OnPowerUpCollect.Invoke(this.powerUpType);
        }
    }
}
