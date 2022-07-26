using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileSpeedManagement : MonoBehaviour
{
    public float currentTileSpeed;
    public TextMeshProUGUI distanceTravelledText;
    public float externalSpeedMultiplier;
    public float distanceTravelled = 0;
    private float currentScore;
    private TileSpeedIncrementation tileSpeedIncrementation;
    private SprintSystem sprintSystem;

    private void Start()
    {
        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.sprintSystem = FindObjectOfType<SprintSystem>();
    }
    private void FixedUpdate()
    {
        if (GameOverEvent.isPlayerDead == false)
        {
            this.currentTileSpeed = this.tileSpeedIncrementation.calculatedTargetTileSpeed * this.externalSpeedMultiplier;
            if (this.sprintSystem.isSprinting)
            {
                this.currentTileSpeed += this.sprintSystem.tileSpeedChange;
            }
            this.distanceTravelled += this.currentTileSpeed * Time.fixedDeltaTime;
            this.currentScore = this.distanceTravelled * 10;
            this.distanceTravelledText.text = Mathf.Round(this.currentScore).ToString();
        }
        else
        {
            this.currentTileSpeed = 0.0f;
        }

    }

}
