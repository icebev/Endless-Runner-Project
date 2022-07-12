using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileSpeedManagement : MonoBehaviour
{
    private TileSpeedIncrementation speedIncrementation;
    private SprintSystem sprintSystem;
    public float currentTileSpeed;
    public TextMeshProUGUI distanceTravelledText;
    public float distanceTravelled = 0;

    public AnimationCurve slowDownCurve;
    public AnimationCurve fogYPositionCurve;
    public float slowDownDuration;
    private float slowDownAnimationTime;
    private float currentScore;

    public GameObject movingFog;
    public Slider chaserSlider;
    public float chaserStartDistance;
    public float chaserCurrentDistance;
    public float chaserMaxDistance;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.slowDownAnimationTime = this.slowDownDuration;
        this.speedIncrementation = this.GetComponent<TileSpeedIncrementation>();
        this.sprintSystem = this.GetComponent<SprintSystem>();

    }

    public void StumbleSlowDown()
    {
        this.slowDownAnimationTime = 0.0f;
        this.playerAnimator.Play("Stumble");
        this.chaserCurrentDistance -= 0.1f;
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

        if (this.sprintSystem.isSprinting)
        {
            if (this.chaserCurrentDistance < this.chaserMaxDistance)
                this.chaserCurrentDistance += 0.1f * Time.fixedDeltaTime;
        }
        else
        {
            if (this.chaserCurrentDistance > 0.0f)
                this.chaserCurrentDistance -= 0.025f * Time.fixedDeltaTime;

        }

        float sliderValue = 1 - (this.chaserCurrentDistance / this.chaserMaxDistance);
        Vector3 targetFogPos = new Vector3(this.movingFog.transform.position.x, this.fogYPositionCurve.Evaluate(sliderValue), this.movingFog.transform.position.z);
        this.movingFog.transform.localPosition = Vector3.MoveTowards(this.movingFog.transform.localPosition, targetFogPos, 5);
        this.chaserSlider.value = sliderValue;
    }

}
