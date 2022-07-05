using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class CollectableEventFunctions : MonoBehaviour
{
    public static UnityEvent<int> OnCoinCollect;

    public static UnityEvent<PowerUpType> OnPowerUpCollect;

    public int coinsCollected = 0;

    public TextMeshProUGUI coinCountText;

    private void Awake()
    {
        CollectableEventFunctions.OnCoinCollect = new UnityEvent<int>();
        CollectableEventFunctions.OnPowerUpCollect = new UnityEvent<PowerUpType>();
        CollectableEventFunctions.OnCoinCollect.AddListener(IncrementCoinCount);
        CollectableEventFunctions.OnPowerUpCollect.AddListener(LogPowerUpType);

    }

    public void IncrementCoinCount(int coinVal)
    {
        this.coinsCollected += coinVal;
        this.coinCountText.text = this.coinsCollected.ToString();
    }

    public void LogPowerUpType(PowerUpType powerUpRef)
    {
        Debug.Log(powerUpRef.powerUpName);
    }
}
