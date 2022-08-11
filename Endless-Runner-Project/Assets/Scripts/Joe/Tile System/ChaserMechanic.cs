using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* CHASER MECHANIC CLASS
 * Author(s): Joe Bevis
 * Date last modified: 11/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * 
 */

/// <summary>
/// Implements the fog chaser mechanic, moves the GUI fog, and triggers game over.
/// </summary>
public class ChaserMechanic : MonoBehaviour
{
    // Inspector set references
    [Header("Private References")]
    [SerializeField] private Animator sprintIndicatorAnimator;
    [SerializeField] private GameObject movingFog;
    [SerializeField] private Slider chaserSlider;
    [SerializeField] private GameOverEvent gameOverEvent;

    [Header("Configurable Values")]
    [Tooltip("A curve for the y position of the GUI fog as the slider value changes.")]
    [SerializeField] private AnimationCurve fogYPositionCurve;
    [SerializeField] private float chaserStartDistance;
    [SerializeField] private float chaserMaxDistance;
    [SerializeField] private float fogMoveSpeed;

    [Tooltip("How quickly the chaser catches up the the player when not sprinting.")]
    [SerializeField] private float chaserCloseInRate; //0.025f
    [Tooltip("How quickly the player gets away from the chaser when sprinting.")]
    [SerializeField] private float sprintEscapeRate; // 0.1f
    private SprintSystem sprintSystem;

    public float chaserCurrentDistance;

    private void Start()
    {
        this.sprintSystem = FindObjectOfType<SprintSystem>();
        this.chaserCurrentDistance = this.chaserStartDistance;
    }

    private void FixedUpdate()
    {
        if (GameOverEvent.isPlayerDead == false)
        {
            // We change the chaser distance sbsed on whether the player is sprinting or not,  
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


        }

        // Update GUI bar
        float sliderValue = 1 - (this.chaserCurrentDistance / this.chaserMaxDistance);
        this.chaserSlider.value = sliderValue;

        if (sliderValue > 0.7)
        {
            this.sprintIndicatorAnimator.SetBool("IndicateSprint", true);
        }
        else
        {
            this.sprintIndicatorAnimator.SetBool("IndicateSprint", false);
        }
        // Game over trigger
        if (sliderValue > 0.99 && GameOverEvent.isPlayerDead == false)
        {
            this.gameOverEvent.playerDeath.Invoke();
            this.chaserCurrentDistance = 0;
        }
        // update chaser fog position
        Vector3 targetFogPos = new Vector3(this.movingFog.transform.localPosition.x, this.fogYPositionCurve.Evaluate(sliderValue), this.movingFog.transform.localPosition.z);
        this.movingFog.transform.localPosition = Vector3.MoveTowards(this.movingFog.transform.localPosition, targetFogPos, this.fogMoveSpeed);



    }
}
