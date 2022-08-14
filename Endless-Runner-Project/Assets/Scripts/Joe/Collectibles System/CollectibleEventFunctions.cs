using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

/* COLLECTIBLE EVENT FUNCTIONS CLASS
 * Author(s): Joe Bevis
 * Date last modified: 14/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Variable protection levels pass
 * Commenting pass
 */
/// <summary>
/// A class for implementing the results of the player picking up a coin or powerup during their run.
/// </summary>
public class CollectibleEventFunctions : MonoBehaviour
{

    // Public static UnityEvents can be invoked from any other script
    // without needing a specific reference, which avoids slowing down execution for situations
    // where a lot of the same objects are spawning and all need access to the event.
    public static UnityEvent<int> OnCoinCollect;
    public static UnityEvent<PowerUpType> OnPowerUpCollect;

    [Header("Inspector References - Collection Special Effects")]
    [SerializeField] private AudioSource coinCollectSound;
    [SerializeField] private AudioSource powerUpCollectSound;
    [SerializeField] private ParticleSystem powerUpCollectParticles;
    [SerializeField] private ParticleSystem coinCollectParticles;
    [SerializeField] private GameObject boostShield;
    [SerializeField] private GameObject coinMagnetField;

    [Header("Inspector References - GUI")]
    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private TextMeshProUGUI gameOverRunCoinCountText;
    [SerializeField] private TextMeshProUGUI gameOverTotalCoinText;
    [SerializeField] private GameObject coinMagnetIndicator;
    [SerializeField] private GameObject coinMultiplierIndicator;
    [SerializeField] private GameObject boostIndicator;

    // Coin collect sound randomizer constant values
    private const float MINCOINPITCH = 0.9f;
    private const float MAXCOINPITCH = 1.1f;
    private const float MINCOINVOL = 0.04f;
    private const float MAXCOINVOL = 0.06f;

    // Mid-run tracking variables
    private int coinsCollected = 0;
    private int coinValueMultiplier = 1;
    private int powerUpsCollected = 0;
    // Private power-up state tracking variables
    private float remainingMagnetTime;
    private float remainingMultiplierTime;
    private float remainingBoostTime;
    private bool magnetActive;
    private bool multiplierActive;
    private bool boostActive;

    private void Awake()
    {
        CollectibleEventFunctions.OnCoinCollect = new UnityEvent<int>();
        CollectibleEventFunctions.OnPowerUpCollect = new UnityEvent<PowerUpType>();
        CollectibleEventFunctions.OnCoinCollect.AddListener(IncrementCoinCount);
        CollectibleEventFunctions.OnPowerUpCollect.AddListener(TriggerPowerUp);
    }

    /// <summary>
    /// Called upon game over or when the run is quit - adds coins to the player's bank and records stats progress
    /// </summary>
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

        // Update Game over screen GUI text to show the correct value of coins collected
        this.gameOverRunCoinCountText.text = this.coinsCollected.ToString();
        this.gameOverTotalCoinText.text = PlayerPrefs.GetInt("TotalPlayerCoins").ToString();

        int lifeTimeTotalCoins = PlayerPrefs.GetInt("LifetimeCoinsCollected");
        lifeTimeTotalCoins += this.coinsCollected;
        PlayerPrefs.SetInt("LifetimeCoinsCollected", lifeTimeTotalCoins);

        // Also update total powerups collected here for convenience and prevent having to update on every collect
        int lifeTimeTotalPowerups = PlayerPrefs.GetInt("LifetimeTotalPowerups");
        lifeTimeTotalPowerups += this.powerUpsCollected;
        PlayerPrefs.SetInt("LifetimeTotalPowerups", lifeTimeTotalPowerups);


    }

    /// <summary>
    /// Called to add coins to the mid-run total collected coins
    /// </summary>
    /// <param name="coinVal">Value of coins to add to the total</param>
    public void IncrementCoinCount(int coinVal)
    {
        this.coinCollectSound.pitch = UnityEngine.Random.Range(MINCOINPITCH, MAXCOINPITCH);
        this.coinCollectSound.volume = UnityEngine.Random.Range(MINCOINVOL, MAXCOINVOL);
        this.coinCollectSound.PlayOneShot(this.coinCollectSound.clip);
        this.coinCollectParticles.Play();
        this.coinsCollected += coinVal * this.coinValueMultiplier;
        this.coinCountText.text = this.coinsCollected.ToString();
    }

    /// <summary>
    /// Called when a power-up is collected to trigger a power-up effect
    /// </summary>
    /// <param name="powerUpRef">The power up type used to determine which effect to activate</param>
    public void TriggerPowerUp(PowerUpType powerUpRef)
    {
        this.powerUpCollectParticles.Play();
        this.powerUpCollectSound.Play();
        this.powerUpsCollected++;

        // Magnet activation
        if (powerUpRef.powerUpName == "CoinMagnet")
        {
            float activeTime = 3 + (2 * PlayerPrefs.GetInt("GameUpgradeMagnet"));
            this.ActivateMagnet(activeTime);
        }

        // Multiplier activation
        if (powerUpRef.powerUpName == "CoinMultiplier")
        {
            float activeTime = 3 + (2 * PlayerPrefs.GetInt("GameUpgradeCoin"));
            this.ActivateMultiplier(activeTime);
        }

        // Speed boost activation
        if (powerUpRef.powerUpName == "MaxSpeed")
        {
            float activeTime = 2 + (2 * PlayerPrefs.GetInt("GameUpgradeBoost"));
            this.ActivateSpeedBoost(activeTime);
        }
    }

    #region Power-Up Activation and Deactivation Methods

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
        this.coinValueMultiplier = 2;
        this.coinMultiplierIndicator.SetActive(true);
        this.multiplierActive = true;
    }

    public void DeactivateMultiplier()
    {
        this.coinValueMultiplier = 1;
        this.coinMultiplierIndicator.SetActive(false);
        this.multiplierActive = false;
    }

    public void ActivateSpeedBoost(float activeDuration)
    {
        this.remainingBoostTime = activeDuration;
        this.boostIndicator.SetActive(true);
        this.boostShield.SetActive(true);
        FindObjectOfType<SprintSystem>().speedBoostModeActive = true;
        this.boostActive = true;
    }

    public void DeactivateSpeedBoost()
    {
        SprintSystem sprintSystem = FindObjectOfType<SprintSystem>();
        sprintSystem.speedBoostModeActive = false;
        this.boostIndicator.SetActive(false);
        this.boostShield.SetActive(false);
        sprintSystem.StopSprinting();
        sprintSystem.tileSpeedChange = 2;
        this.boostActive = false;
    }

    #endregion


    // We use the update loop to check whether the power-up has expired
    // and its effects need to be deactivated
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