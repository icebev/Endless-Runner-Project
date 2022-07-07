using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollectable : MonoBehaviour
{
    public int coinValue = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectableEventFunctions.OnCoinCollect.Invoke(this.coinValue);
            this.transform.Translate(new Vector3(0, -200, 0), Space.World);
        }
    }
}
