using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameOverEvent : MonoBehaviour
{
    public UnityEvent playerDeath;
    public static bool isPlayerDead;
    public GameObject gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        GameOverEvent.isPlayerDead = false;
    }

    public void KillPlayer()
    {
        GameOverEvent.isPlayerDead = true;
        this.gameOverText.SetActive(true);
    }

}
