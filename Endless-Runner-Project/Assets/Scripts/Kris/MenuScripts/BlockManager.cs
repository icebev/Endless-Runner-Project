using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* BLOCK MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * No Changes.
 */
/// <summary>
/// A class for reverting the non-purchased blocks to red, and updating the purchased blocks to green.
/// </summary>

public class BlockManager : MonoBehaviour
{
    [SerializeField] private Image[] Blocks; //Retrieving the 4 blocks' images

    public void RedifyBlocks() //Setting all the block's colours to red
    {
        this.Blocks[0].color = Color.red;
        this.Blocks[1].color = Color.red;
        this.Blocks[2].color = Color.red;
        this.Blocks[3].color = Color.red;
    }

    public void UpdateBlocks(int CurrentUpgrades) //Loops through and Updates the purchased blocks to Green
    {
        if (CurrentUpgrades == 0) return;
        for (int x = 0; x < CurrentUpgrades; x++)
        {
            this.Blocks[x].color = Color.green;
        }
    }
}
