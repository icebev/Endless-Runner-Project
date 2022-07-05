using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PowerUpType
{
    public string powerUpName;
    public UnityAction eventFunction;
    public GameObject powerUpPrefab;
    public float powerUpRarityWeighting;
}

public class PowerUpSpawn : MonoBehaviour
{
    public PowerUpType[] powerUpTypes;
    public int chosenPowerUpIndex;

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
        GameObject newPowerUp = Instantiate(this.powerUpTypes[this.chosenPowerUpIndex].powerUpPrefab, this.transform.position, this.transform.rotation);
        newPowerUp.transform.parent = this.transform;
        newPowerUp.GetComponent<PowerUp>().powerUpType = this.powerUpTypes[this.chosenPowerUpIndex];
    }
}
