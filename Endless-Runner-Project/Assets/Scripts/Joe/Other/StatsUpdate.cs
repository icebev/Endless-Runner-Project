using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* STATS UPDATE CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// Updates the lifetime stats GUI panel values based 
/// on saved PlayerPref values when the stats GameObject screen is enabled
/// </summary>
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
