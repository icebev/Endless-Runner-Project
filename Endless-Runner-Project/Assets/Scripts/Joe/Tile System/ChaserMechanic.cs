using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaserMechanic : MonoBehaviour
{
    public GameObject movingFog;
    public AnimationCurve fogYPositionCurve;

    public Slider chaserSlider;
    public float chaserStartDistance;
    public float chaserCurrentDistance;
    public float chaserMaxDistance;
    public GameOverEvent gameOverEvent;
    public float fogMoveSpeed;

    private SprintSystem sprintSystem;
    public float chaserCloseInRate; //0.025f
    public float sprintEscapeRate; // 0.1f

    private void Start()
    {
        this.sprintSystem = FindObjectOfType<SprintSystem>();
        this.chaserCurrentDistance = this.chaserStartDistance;
    }

    private void FixedUpdate()
    {
        if (this.sprintSystem.isSprinting)
        {
            if (this.chaserCurrentDistance < this.chaserMaxDistance)
                this.chaserCurrentDistance += this.sprintEscapeRate * Time.fixedDeltaTime;
        }
        else
        {
            if (this.chaserCurrentDistance > 0.0f)
                this.chaserCurrentDistance -= this.chaserCloseInRate * Time.fixedDeltaTime;
        }

        // Update GUI bar
        float sliderValue = 1 - (this.chaserCurrentDistance / this.chaserMaxDistance);
        Debug.Log(sliderValue);
        this.chaserSlider.value = sliderValue;

        // update chaser fog position
        Vector3 targetFogPos = new Vector3(this.movingFog.transform.localPosition.x, this.fogYPositionCurve.Evaluate(sliderValue), this.movingFog.transform.localPosition.z);
        this.movingFog.transform.localPosition = Vector3.MoveTowards(this.movingFog.transform.localPosition, targetFogPos, this.fogMoveSpeed);


        // Game over trigger
        if (sliderValue > 0.95)
        {
            this.gameOverEvent.playerDeath.Invoke();
        }
    }
}
