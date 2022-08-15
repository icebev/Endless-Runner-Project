using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* POWER-UP TYPE CONFIG STRUCT & POWER-UP TYPE ENUM
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Refactored power-up type as an enum for readability 
 * and to avoid passing in excess information about the power-up
 */
/// <summary>
/// Allows the power up type to be configured in the inpector for each power-up spawn point
/// </summary>
[System.Serializable]
public struct PowerUpTypeConfig
{
    public PowerUpType powerUpType;
    [Tooltip("The prefab that will spawn if this power-up type is chosen.")]
    public GameObject powerUpPrefab;
    [Tooltip("How likely this power-up type will be chosen relative to other types.")]
    public float powerUpRarityWeighting;
}

/// <summary>
/// The type of the power-up used to determine the effect its collection.
/// </summary>
public enum PowerUpType
{
    CoinMagnet,
    CoinMultiplier,
    MaxSpeed,
}