using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockManager : MonoBehaviour
{

    [SerializeField] private Image[] Blocks;

    public void RedifyBlocks()
    {
        this.Blocks[0].color = Color.red;
        this.Blocks[1].color = Color.red;
        this.Blocks[2].color = Color.red;
        this.Blocks[3].color = Color.red;
    }


    public void UpdateBlocks(int CurrentUpgrades)
    {
        if (CurrentUpgrades == 0) return;
        for (int x = 0; x < CurrentUpgrades; x++)
        {
            this.Blocks[x].color = Color.green;
        }
    }
}
