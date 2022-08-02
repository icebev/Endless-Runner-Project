using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PowerUpType
{
    public string powerUpName;
    public float powerUpDuration;
    public UnityAction eventFunction;
    public GameObject powerUpPrefab;
    public float powerUpRarityWeighting;
   
}

public class PowerUpSpawn : MonoBehaviour
{
    public PowerUpType[] powerUpTypes;
    public int chosenPowerUpIndex;
    public float overallSpawnChance;

    private void Awake()
    {
        float probabilitiesTotal = 0.0f;

        foreach (PowerUpType powerupType in this.powerUpTypes)
        {
            probabilitiesTotal += powerupType.powerUpRarityWeighting;
        }

        float randomSelector = Random.Range(0.0f, probabilitiesTotal);

        int selectedIndex = -1;
        while (randomSelector >= 0.0f && selectedIndex < this.powerUpTypes.Length - 1)
        {
            selectedIndex++;
            randomSelector -= this.powerUpTypes[selectedIndex].powerUpRarityWeighting;
        }

        this.chosenPowerUpIndex = selectedIndex;
    }
    private void Start()
    {

        if (Random.Range(0.0f, 1.0f) <= this.overallSpawnChance)
        {
            GameObject newPowerUp = Instantiate(this.powerUpTypes[this.chosenPowerUpIndex].powerUpPrefab, this.transform.position, this.transform.rotation);
            newPowerUp.transform.parent = this.transform;
            newPowerUp.GetComponent<PowerUp>().powerUpType = this.powerUpTypes[this.chosenPowerUpIndex];
        }
    }
}
