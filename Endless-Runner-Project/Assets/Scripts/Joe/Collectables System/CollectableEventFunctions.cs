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
    public AudioSource coinCollectSound;
    public ParticleSystem powerUpCollectParticles;
    public ParticleSystem coinCollectParticles;
    public float remainingMagnetTime;
    public float remainingMultiplierTime;
    public float remainingBoostTime;
    public bool magnetActive;
    public bool multiplierActive;
    public bool boostActive;


    public TextMeshProUGUI coinCountText;

    public TextMeshProUGUI gameOverRunCoinCountText;
    public TextMeshProUGUI gameOverTotalCoinText;


    public GameObject coinMagnetField;
    public GameObject coinMagnetIndicator;

    public GameObject coinMultiplierIndicator;
    public GameObject boostIndicator;

    private void Awake()
    {
        CollectableEventFunctions.OnCoinCollect = new UnityEvent<int>();
        CollectableEventFunctions.OnPowerUpCollect = new UnityEvent<PowerUpType>();
        CollectableEventFunctions.OnCoinCollect.AddListener(IncrementCoinCount);
        CollectableEventFunctions.OnPowerUpCollect.AddListener(TriggerPowerUp);
    }

    public void RunEndCoinUpdate()
    {
        if (PlayerPrefs.GetInt("TotalPlayerCoins") == 0)
        {
            PlayerPrefs.SetInt("TotalPlayerCoins", this.coinsCollected);
        }
        else
        {
            int currentPlayerCoins = PlayerPrefs.GetInt("TotalPlayerCoins");
            PlayerPrefs.SetInt("TotalPlayerCoins", currentPlayerCoins + this.coinsCollected);
        }

        this.gameOverRunCoinCountText.text = this.coinsCollected.ToString();
        this.gameOverTotalCoinText.text = PlayerPrefs.GetInt("TotalPlayerCoins").ToString();

    }

    public void IncrementCoinCount(int coinVal)
    {
        this.coinCollectSound.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        this.coinCollectSound.volume = UnityEngine.Random.Range(0.04f, 0.06f);
        this.coinCollectSound.PlayOneShot(this.coinCollectSound.clip);
        this.coinCollectParticles.Play();
        this.coinsCollected += coinVal;
        this.coinCountText.text = this.coinsCollected.ToString();
    }

    public void TriggerPowerUp(PowerUpType powerUpRef)
    {
        this.powerUpCollectParticles.Play();

        // Magnet activation
        if (powerUpRef.powerUpName == "CoinMagnet")
        {
            this.ActivateMagnet(powerUpRef.powerUpDuration);
        }

        // Multiplier activation
        if (powerUpRef.powerUpName == "CoinMultiplier")
        {
            this.ActivateMultiplier(powerUpRef.powerUpDuration);
        }

        // Speed boost activation
        if (powerUpRef.powerUpName == "MaxSpeed")
        {
            this.ActivateSpeedBoost(powerUpRef.powerUpDuration);
        }

    }

    public void ActivateMagnet(float activeDuration)
    {
        this.remainingMagnetTime = activeDuration;
        this.coinMagnetField.SetActive(true);
        this.coinMagnetIndicator.SetActive(true);
        this.magnetActive = true;
    }

    public void DeactivateMagnet()
    {
        this.coinMagnetField.SetActive(false);
        this.coinMagnetIndicator.SetActive(false);
        this.magnetActive = false;
    }

    public void ActivateMultiplier(float activeDuration)
    {
        this.remainingMultiplierTime = activeDuration;
        CoinCollectable.coinValueMultiplier = 2;
        this.coinMultiplierIndicator.SetActive(true);
        this.multiplierActive = true;
    }

    public void DeactivateMultiplier()
    {
        CoinCollectable.coinValueMultiplier = 1;
        this.coinMultiplierIndicator.SetActive(false);
        this.multiplierActive = false;
    }

    public void ActivateSpeedBoost(float activeDuration)
    {
        this.remainingBoostTime = activeDuration;
        this.boostIndicator.SetActive(true);
        FindObjectOfType<SprintSystem>().speedBoostModeActive = true;
        this.boostActive = true;
    }

    public void DeactivateSpeedBoost()
    {
        SprintSystem sprintSystem = FindObjectOfType<SprintSystem>();
        sprintSystem.speedBoostModeActive = false;
        this.boostIndicator.SetActive(false);
        sprintSystem.StopSprinting();
        sprintSystem.tileSpeedChange = 2;
        this.boostActive = false;

    }

    private void Update()
    {
        // Magnet powerup
        if (this.remainingMagnetTime >= 0)
        {
            this.remainingMagnetTime -= Time.deltaTime;
        }
        else if (this.magnetActive == true)
        {
            this.DeactivateMagnet();
        }

        // Multiplier powerup
        if (this.remainingMultiplierTime >= 0)
        {
            this.remainingMultiplierTime -= Time.deltaTime;
        }
        else if (this.multiplierActive == true)
        {
            this.DeactivateMultiplier();
        }

        // Boost powerup
        if (this.remainingBoostTime >= 0)
        {
            this.remainingBoostTime -= Time.deltaTime;

        }
        else if (this.boostActive == true)
        {
            this.DeactivateSpeedBoost();
 

        }

    }
}
