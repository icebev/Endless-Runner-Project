using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileSpeedManagement : MonoBehaviour
{
    private TileSpeedIncrementation speedIncrementation;
    public float currentTileSpeed;
    public TextMeshProUGUI distanceTravelledText;
    public float distanceTravelled = 0;

    public AnimationCurve slowDownCurve;
    public float slowDownDuration;
    private float slowDownAnimationTime;
    private float currentScore;

    // Start is called before the first frame update
    void Start()
    {
        this.slowDownAnimationTime = this.slowDownDuration;
        this.speedIncrementation = this.GetComponent<TileSpeedIncrementation>();
        
    }

    public void StumbleSlowDown()
    {
        this.slowDownAnimationTime = 0.0f;
    }
    private void FixedUpdate()
    {
        if (this.slowDownAnimationTime < this.slowDownDuration)
        {
            float speedMultiplier = this.slowDownCurve.Evaluate(this.slowDownAnimationTime / this.slowDownDuration);

            this.currentTileSpeed = this.speedIncrementation.calculatedTargetTileSpeed * speedMultiplier;

            this.slowDownAnimationTime += Time.fixedDeltaTime;
        }
        else
        {
            this.currentTileSpeed = this.speedIncrementation.calculatedTargetTileSpeed;

        }

        this.distanceTravelled += this.currentTileSpeed * Time.fixedDeltaTime;
        this.currentScore = this.distanceTravelled * 10;
        this.distanceTravelledText.text = Mathf.Round(this.currentScore).ToString();
    }

}
