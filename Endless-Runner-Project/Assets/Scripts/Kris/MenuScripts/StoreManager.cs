using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/* STORE MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * 
 */
/// <summary>
/// A class for managing the store
/// </summary>
public class StoreManager : MonoBehaviour
{
    private int coins; //Number of coins on display that the player has.
    [SerializeField] private float subsequentPurchaseAudioPitchIncrease; //Subsequence purchase audio that increases in pitch
    [SerializeField] private TextMeshProUGUI[] coinsText; //Text that shows how much coins the user has (Array cause of the false shadow)
    [SerializeField] private TextMeshProUGUI[] UpgradesText; //Text to show extra upgrade that will be recieved (+2 seconds)
    [SerializeField] private TextMeshProUGUI[] PurchaseText; //Text to show how much it will cost to purchase
    private string[] PurchaseTextPreset = new string[] { "Coin +", "Magnet +", "Boost +" }; //Old legacy code to show type of purchase.
    private string[] PlayerPrefStrings = new string[] { "GameUpgradeCoin", "GameUpgradeMagnet", "GameUpgradeBoost" }; //The strings for player prefs
    [SerializeField] private BlockManager[] _blockManagers; //The block manager to manage the purchase blocks within the game

    [SerializeField] private AudioSource PurchaseAudio; //Purchase Game Audio
    [SerializeField] private AudioClip[] PuchaseAudioClips; //Audio clips for purchase complete, and purchase fail.
    private int[] upgrades = new int[] { 0, 0, 0 }; //Coin, Magnet, and Boost current upgrades
    private int baseCost = 200; //Base cost for everything.
    private int[] finalCost = new int[] { 0, 0, 0 }; //Calculated cost from upgrade (each upgrade increases the base price a lot)
    private int[] extraSeconds = new int[] { 0, 0, 0 }; //The extra seconds that the upgrades have.
    public enum GameUpgrades //Enums for different type of upgrades in the game.
    {
        Coin,
        Magnet,
        Boost,
    }

    private void OnEnable()
    {

        this.coins = PlayerPrefs.GetInt("TotalPlayerCoins"); //Getting the total player coins
        this.upgrades[(int)GameUpgrades.Coin] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Coin]); //Retrieving the Coin Upgrades from playerprefs
        this.upgrades[(int)GameUpgrades.Magnet] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Magnet]);
        this.upgrades[(int)GameUpgrades.Boost] = PlayerPrefs.GetInt(PlayerPrefStrings[(int)GameUpgrades.Boost]);        
        MakeBlocksRed(); //Sets the purchase blocks to fix resetting progress

        //Updates the prices for each of the upgrades depending on what you have unlocked.
        UpdatePrices(GameUpgrades.Coin); 
        UpdatePrices(GameUpgrades.Magnet);
        UpdatePrices(GameUpgrades.Boost);

        //Updates the seconds shown on the upgrades that have been purchased
        UpdateSeconds(GameUpgrades.Coin);
        UpdateSeconds(GameUpgrades.Magnet);
        UpdateSeconds(GameUpgrades.Boost);

        //Updates the GUI of the game.
        UpdateGUI();
    }


    private void UpdatePrices(GameUpgrades whichUpgrade)
    {
        this.finalCost[(int)whichUpgrade] = this.baseCost; //Sets the final cost to base cost to do the calculation.
        if (this.upgrades[(int)whichUpgrade] != 0) //Checks to see if there is a upgrade to increase the cost of future purchases.
        {
                //Final cost calculation from the base point to final cost
                this.finalCost[(int)whichUpgrade] = this.baseCost *  Mathf.RoundToInt( Mathf.Pow(2, this.upgrades[(int)whichUpgrade]));
            
        }

        if(this.finalCost[(int)whichUpgrade] >= 3200) //Final cost which changes the text to "MAX" to show you can't buy anymore.
        {
            this.PurchaseText[(int)whichUpgrade].text = "MAX";
        }
        else
        {
            this.PurchaseText[(int)whichUpgrade].text = "" + this.finalCost[(int)whichUpgrade]; //Shows the final cost.
        }
    }

    private void UpdateSeconds(GameUpgrades whichUpgrade) //Updates the seconds dependant on how many upgrades are purchased.
    {
        this.extraSeconds[(int)whichUpgrade] = (this.upgrades[(int)whichUpgrade]) * 2; //Per upgrade, you get 2 extra seconds. So its extraSecs = upgrade * 2.
        if (this.extraSeconds[(int)whichUpgrade] == 0) //If you have no upgrade
        {
            this.UpgradesText[(int)whichUpgrade].text = "None"; //The text is set to None.
        }
        else
        {
            //If you do have upgrades, it will show on the extra seconds gained.
            this.UpgradesText[(int)whichUpgrade].text = "+" + this.extraSeconds[(int)whichUpgrade] + " Seconds";
        }
    }

    private void UpdateGUI()
    {
        foreach (TextMeshProUGUI text in coinsText)
        {
            text.text = ("x" + this.coins); //Updating the Coins to say how many coins there are (it's in a for loop cause theres 2: Shadow + regular)
        }

        //This Updates the blocks on the bottom to be Green depending on how many upgrades have been unlocked.
        this._blockManagers[(int)GameUpgrades.Coin].UpdateBlocks(this.upgrades[(int)GameUpgrades.Coin]); 
        this._blockManagers[(int)GameUpgrades.Magnet].UpdateBlocks(this.upgrades[(int)GameUpgrades.Magnet]);
        this._blockManagers[(int)GameUpgrades.Boost].UpdateBlocks(this.upgrades[(int)GameUpgrades.Boost]);

    }
    //Buy upgrade function
    private void BuyUpgrade(GameUpgrades whichUpgrade)
    {
        //If you havent got enough money or the cost is set to maximum, it will play a decline sound and nothing happens.
        if (this.coins < this.finalCost[(int)whichUpgrade] || this.finalCost[(int)whichUpgrade] == 3200)
        {
            this.PurchaseAudio.pitch = 1.0f;
            this.PurchaseAudio.clip = this.PuchaseAudioClips[1];
            this.PurchaseAudio.Play();
            return;
        }

        //If you do have enough money, you can purchase. The pitch increases per upgrade, before playing the audio.
        this.PurchaseAudio.pitch = 1.0f + (this.subsequentPurchaseAudioPitchIncrease * this.upgrades[(int)whichUpgrade]);
        this.PurchaseAudio.clip = this.PuchaseAudioClips[0];
        this.PurchaseAudio.Play();
        this.coins -= this.finalCost[(int)whichUpgrade]; //Minus the player's coins.
        this.upgrades[(int)whichUpgrade] += 1; //Add the upgrade
        PlayerPrefs.SetInt("TotalPlayerCoins", this.coins); //Set the new value of the coins
        PlayerPrefs.SetInt(this.PlayerPrefStrings[(int)whichUpgrade], this.upgrades[(int)whichUpgrade]); //Update the upgrades in playerprefs
        UpdatePrices(whichUpgrade); //Updates the prices of the upgrade you bought
        UpdateSeconds(whichUpgrade); //Updates the seconds of the upgrade you bought
        UpdateGUI(); //Updates the GUI to show new data
    }     

   public void BuyCoin() //Buy coin function
    {
        this.BuyUpgrade(GameUpgrades.Coin);//Buys the coin
    }
    public void BuyMagnet() //Buy magnet function
    {
        this.BuyUpgrade(GameUpgrades.Magnet); //Buys the magnet
    }
    public void BuyBoost() //Buys boost function
    {
        this.BuyUpgrade(GameUpgrades.Boost); //Buys the boost
    }

    public void MakeBlocksRed() //Turns the blocks red.
    {
        this._blockManagers[(int)GameUpgrades.Coin].RedifyBlocks(); //Calls the redify blocks within the respected upgrades script
        this._blockManagers[(int)GameUpgrades.Magnet].RedifyBlocks();
        this._blockManagers[(int)GameUpgrades.Boost].RedifyBlocks();
    }
}
