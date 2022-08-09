using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameOverEvent : MonoBehaviour
{
    public UnityEvent playerDeath;
    public static bool isPlayerDead;


    // Start is called before the first frame update
    void Start()
    {
        GameOverEvent.isPlayerDead = false;
    }

    public void KillPlayer()
    {
        GameOverEvent.isPlayerDead = true;
        int totalPlayerDeaths = PlayerPrefs.GetInt("LifetimeTotalDeaths");
        totalPlayerDeaths++;
        PlayerPrefs.SetInt("LifetimeTotalDeaths", totalPlayerDeaths);
    }

}
