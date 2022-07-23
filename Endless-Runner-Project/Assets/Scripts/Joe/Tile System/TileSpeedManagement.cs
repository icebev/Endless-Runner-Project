using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileSpeedManagement : MonoBehaviour
{
    public float currentTileSpeed;
    public TextMeshProUGUI distanceTravelledText;
    public float distanceTravelled = 0;
    private float currentScore;

    private void FixedUpdate()
    {
        this.distanceTravelled += this.currentTileSpeed * Time.fixedDeltaTime;
        this.currentScore = this.distanceTravelled * 10;
        this.distanceTravelledText.text = Mathf.Round(this.currentScore).ToString();
    }

}
