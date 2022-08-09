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
    [SerializeField] private TextMeshProUGUI finalDistanceText;

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
            this.currentScore = this.distanceTravelled;
            this.distanceTravelledText.text = Mathf.Round(this.currentScore).ToString() + "m";
        }
        else
        {
            this.currentTileSpeed = 0.0f;
            this.finalDistanceText.text = "Distance: " + Mathf.Round(this.distanceTravelled).ToString() + "m";
        }

    }

    public void RunEndDistanceUpdate()
    {
        int lifeTimeTotalDistance = PlayerPrefs.GetInt("LifetimeTotalDistance");
        lifeTimeTotalDistance += Mathf.RoundToInt(this.distanceTravelled);
        PlayerPrefs.SetInt("LifetimeTotalDistance", lifeTimeTotalDistance);

        int lifeTimeBestDistance = PlayerPrefs.GetInt("LifetimeBestDistance");
        if (this.distanceTravelled > lifeTimeBestDistance)
        {
            PlayerPrefs.SetInt("LifetimeBestDistance", Mathf.RoundToInt(this.distanceTravelled));
        }
    }

}
