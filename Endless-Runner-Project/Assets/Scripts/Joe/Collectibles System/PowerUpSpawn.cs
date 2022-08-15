using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* POWER-UP SPAWN CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Removed on-awake determination of the powerup type 
 * so that the calculation is only made if the prefab needs to spawn
 */
/// <summary>
/// Used to spawn a power-up at the spawn point based on the spawn chances
/// </summary>

public class PowerUpSpawn : MonoBehaviour
{
    [SerializeField] private PowerUpTypeConfig[] powerUpTypeConfigs;
    [SerializeField] private int chosenPowerUpIndex;
    [SerializeField] private float overallSpawnChance;

    /// <summary>
    /// Determine type of power-up this spawn point will spawn 
    /// </summary>
    private void SelectPowerUpType()
    {
        float probabilitiesTotal = 0.0f;

        foreach (PowerUpTypeConfig powerupType in this.powerUpTypeConfigs)
        {
            probabilitiesTotal += powerupType.powerUpRarityWeighting;
        }

        // Select at powerup type at random based on the rarity weightings 
        float randomSelector = Random.Range(0.0f, probabilitiesTotal);

        // We start at -1 for selected index since the while loop will always execute at least once
        // given that the random value will be greater than zero
        int selectedIndex = -1;
        while (randomSelector >= 0.0f && selectedIndex < this.powerUpTypeConfigs.Length - 1)
        {
            selectedIndex++;
            randomSelector -= this.powerUpTypeConfigs[selectedIndex].powerUpRarityWeighting;
        }

        this.chosenPowerUpIndex = selectedIndex;
    }

    private void Start()
    {
        // We only instantiate the powerup prefab to be collected
        // if the random value is less than the overall spawn chance set for this spawn point
        if (Random.Range(0.0f, 1.0f) <= this.overallSpawnChance)
        {
            this.SelectPowerUpType();
            GameObject newPowerUp = Instantiate(this.powerUpTypeConfigs[this.chosenPowerUpIndex].powerUpPrefab, this.transform.position, this.transform.rotation);
            newPowerUp.transform.parent = this.transform;
            newPowerUp.GetComponent<PowerUp>().powerUpType = this.powerUpTypeConfigs[this.chosenPowerUpIndex].powerUpType;
        }
    }
}
