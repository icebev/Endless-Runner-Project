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
    

    public TextMeshProUGUI coinCountText;

    public GameObject coinMagnetField;
    public GameObject coinMagnetIndicator;

    public GameObject coinMultiplierIndicator;

    private void Awake()
    {
        CollectableEventFunctions.OnCoinCollect = new UnityEvent<int>();
        CollectableEventFunctions.OnPowerUpCollect = new UnityEvent<PowerUpType>();
        CollectableEventFunctions.OnCoinCollect.AddListener(IncrementCoinCount);
        CollectableEventFunctions.OnPowerUpCollect.AddListener(TriggerPowerUp);

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
        if (powerUpRef.powerUpName == "CoinMagnet")
        {
            this.coinMagnetField.SetActive(true);
            this.coinMagnetIndicator.SetActive(true);

            StartCoroutine(DeactivateCoinMagnet(powerUpRef.powerUpDuration));

        }

        if (powerUpRef.powerUpName == "CoinMultiplier")
        {
            CoinCollectable.coinValueMultiplier = 2;
            this.coinMultiplierIndicator.SetActive(true);

            StartCoroutine(DeactivateCoinMultiplier(powerUpRef.powerUpDuration));

        }
    }

    public IEnumerator DeactivateCoinMagnet(float delay)
    {
        yield return new WaitForSeconds(delay);

        this.coinMagnetField.SetActive(false);
        this.coinMagnetIndicator.SetActive(false);
    }

    public IEnumerator DeactivateCoinMultiplier(float delay)
    {
        yield return new WaitForSeconds(delay);

        CoinCollectable.coinValueMultiplier = 1;
        this.coinMultiplierIndicator.SetActive(false);
    }
}
