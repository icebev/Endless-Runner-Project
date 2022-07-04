using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CollectableEventFunctions : MonoBehaviour
{
    public static UnityEvent OnCoinCollect;

    public int coinsCollected = 0;

    public TextMeshProUGUI coinCountText;

    private void Awake()
    {
        CollectableEventFunctions.OnCoinCollect = new UnityEvent();
        CollectableEventFunctions.OnCoinCollect.AddListener(IncrementCoinCount);
    }

    public void IncrementCoinCount()
    {
        this.coinsCollected += 1;
        this.coinCountText.text = this.coinsCollected.ToString();
    }
}
