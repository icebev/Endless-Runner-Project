using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/* GAME OVER EVENT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// For providing a death state and triggered event for the player character upon game over
/// </summary>
public class GameOverEvent : MonoBehaviour
{
    /// <summary>
    /// Invoked by the ChaserMechanics script to trigger death effects set up in the inspector
    /// </summary>
    public UnityEvent playerDeath;
    public static bool isPlayerDead;

    // Start is called before the first frame update
    void Start()
    {
        // We set the global state variable back to false at the start of the game when the player is alive
        GameOverEvent.isPlayerDead = false;
    }

    // Called when the playerDeath Unity event is invoked
    public void KillPlayer()
    {
        GameOverEvent.isPlayerDead = true;

        // Increase tracked total deaths
        int totalPlayerDeaths = PlayerPrefs.GetInt("LifetimeTotalDeaths");
        totalPlayerDeaths++;
        PlayerPrefs.SetInt("LifetimeTotalDeaths", totalPlayerDeaths);
    }

}
