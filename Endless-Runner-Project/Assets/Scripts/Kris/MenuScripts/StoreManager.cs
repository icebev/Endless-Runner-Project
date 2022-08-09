using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StoreManager : MonoBehaviour
{
    private int coins;
    [SerializeField] private float subsequentPurchaseAudioPitchIncrease;
    [SerializeField] private TextMeshProUGUI[] coinsText;
    [SerializeField] private TextMeshProUGUI[] UpgradesText;
    [SerializeField] private TextMeshProUGUI[] PurchaseText;
    private string[] PurchaseTextPreset = new string[] { "Coin +", "Magnet +", "Boost +" };
    private string[] PlayerPrefStrings = new string[] { "GameUpgradeCoin", "GameUpgradeMagnet", "GameUpgradeBoost" };
    [SerializeField] private BlockManager[] _blockManagers;

    [SerializeField] private AudioSource PurchaseAudio;
    [SerializeField] private AudioClip[] PuchaseAudioClips;
    private int[] upgrades = new int[] { 0, 0, 0 };
    private int baseCost = 200;
    private int[] finalCost = new int[] { 0, 0, 0 };
    private int[] extraSeconds = new int[] { 0, 0, 0 };
    public enum GameUpgrades
    {
        Coin,
        Magnet,
        Boost,
    }

    private void OnEnable()
    {

        this.coins = PlayerPrefs.GetInt("TotalPlayerCoins");
        this.coins = 100000;
        this.upgrades[(int)GameUpgrades.Coin] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Coin]);
        this.upgrades[(int)GameUpgrades.Magnet] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Magnet]);
        this.upgrades[(int)GameUpgrades.Boost] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Boost]);
        print(this.upgrades[(int)GameUpgrades.Coin]);
        
        MakeBlocksRed();

        UpdatePrices(GameUpgrades.Coin);
        UpdatePrices(GameUpgrades.Magnet);
        UpdatePrices(GameUpgrades.Boost);

        UpdateSeconds(GameUpgrades.Coin);
        UpdateSeconds(GameUpgrades.Magnet);
        UpdateSeconds(GameUpgrades.Boost);

        UpdateGUI();
    }


    private void UpdatePrices(GameUpgrades whichUpgrade)
    {
        this.finalCost[(int)whichUpgrade] = this.baseCost;
        if (this.upgrades[(int)whichUpgrade] != 0) 
        {

                this.finalCost[(int)whichUpgrade] = this.baseCost *  Mathf.RoundToInt( Mathf.Pow(2, this.upgrades[(int)whichUpgrade]));
            
        }

        if(this.finalCost[(int)whichUpgrade] >= 3200)
        {
            this.PurchaseText[(int)whichUpgrade].text = "MAX";
        }
        else
        {
            this.PurchaseText[(int)whichUpgrade].text = "" + this.finalCost[(int)whichUpgrade];
        }


    }
    private void UpdateSeconds(GameUpgrades whichUpgrade)
    {
        this.extraSeconds[(int)whichUpgrade] = (this.upgrades[(int)whichUpgrade]) * 2;
        this.UpgradesText[(int)whichUpgrade].text = PurchaseTextPreset[(int)whichUpgrade] + this.extraSeconds[(int)whichUpgrade] + "s";
    }

    private void UpdateGUI()
    {
        foreach (TextMeshProUGUI text in coinsText)
        {
            text.text = ("x" + this.coins);
        }
        this._blockManagers[(int)GameUpgrades.Coin].UpdateBlocks(this.upgrades[(int)GameUpgrades.Coin]);
        this._blockManagers[(int)GameUpgrades.Magnet].UpdateBlocks(this.upgrades[(int)GameUpgrades.Magnet]);
        this._blockManagers[(int)GameUpgrades.Boost].UpdateBlocks(this.upgrades[(int)GameUpgrades.Boost]);

    }
    private void BuyUpgrade(GameUpgrades whichUpgrade)
    {
        if (this.coins < this.finalCost[(int)whichUpgrade] || this.finalCost[(int)whichUpgrade] == 3200)
        {
            this.PurchaseAudio.pitch = 1.0f;
            this.PurchaseAudio.clip = this.PuchaseAudioClips[1];
            this.PurchaseAudio.Play();
            return;
        }
        this.PurchaseAudio.pitch = 1.0f + (this.subsequentPurchaseAudioPitchIncrease * this.upgrades[(int)whichUpgrade]);
        this.PurchaseAudio.clip = this.PuchaseAudioClips[0];
        this.PurchaseAudio.Play();
        this.coins -= this.finalCost[(int)whichUpgrade];
        this.upgrades[(int)whichUpgrade] += 1;
        PlayerPrefs.SetInt("TotalPlayerCoins", this.coins);
        PlayerPrefs.SetInt(this.PlayerPrefStrings[(int)whichUpgrade], this.upgrades[(int)whichUpgrade]);
        UpdatePrices(whichUpgrade);
        UpdateSeconds(whichUpgrade);
        UpdateGUI();
        //PurchaseAudio.Play();

    }

   public void BuyCoin()
    {
        this.BuyUpgrade(GameUpgrades.Coin);
    }
    public void BuyMagnet()
    {
        this.BuyUpgrade(GameUpgrades.Magnet);
    }
    public void BuyBoost()
    {
        this.BuyUpgrade(GameUpgrades.Boost);
    }

    public void MakeBlocksRed()
    {
        this._blockManagers[(int)GameUpgrades.Coin].RedifyBlocks();
        this._blockManagers[(int)GameUpgrades.Magnet].RedifyBlocks();
        this._blockManagers[(int)GameUpgrades.Boost].RedifyBlocks();
    }

    private void FixedUpdate()
    {
        print(this.upgrades[(int)GameUpgrades.Coin]);
        
    }

}
