using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* AUDIO VOLUME SLIDER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * 
 */
/// <summary>
/// A class for 
/// </summary>

public class ResetHowTo : MonoBehaviour
{
    [SerializeField] private GameObject[] Menus;
    private void OnEnable()
    {
        int index = 0;
        foreach (GameObject HelpScreen in Menus) 
        { 
            if (index == 0) { HelpScreen.SetActive(true); }
            else { HelpScreen.SetActive(false);  }
            index += 1;
        }
    }
}
