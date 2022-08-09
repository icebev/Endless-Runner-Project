using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestDistanceValue;
    [SerializeField] private TextMeshProUGUI coinsCollectedValue;
    [SerializeField] private TextMeshProUGUI totalDistanceValue;
    [SerializeField] private TextMeshProUGUI totalDeathsValue;
    [SerializeField] private TextMeshProUGUI totalPowerupsValue;


    private void OnEnable()
    {
        this.bestDistanceValue.text = PlayerPrefs.GetInt("LifetimeBestDistance").ToString() + "m";
        this.coinsCollectedValue.text = PlayerPrefs.GetInt("LifetimeCoinsCollected").ToString();
        this.totalDistanceValue.text = PlayerPrefs.GetInt("LifetimeTotalDistance").ToString() + "m";
        this.totalDeathsValue.text = PlayerPrefs.GetInt("LifetimeTotalDeaths").ToString();
        this.totalPowerupsValue.text = PlayerPrefs.GetInt("LifetimeTotalPowerups").ToString();
    }
}
