using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* RESET HOW TO CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for  resetting the position of the "How to play"
/// </summary>

public class ResetHowTo : MonoBehaviour
{
    [SerializeField] private GameObject[] Menus; //The different "How to play" menus
    private void OnEnable() //Whenever the menu is activated
    {
        int index = 0; //Index 0 is used to set the first screen to active
        foreach (GameObject HelpScreen in Menus)  //Enables the first and disables the others
        { 
            if (index == 0) { HelpScreen.SetActive(true); }
            else { HelpScreen.SetActive(false);  }
            index += 1; //increases the index.
        }
    }
}
