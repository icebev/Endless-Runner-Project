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
 * Replaced hard coded indicator threshold value with a configurable variable
 * Gave the ChaserCurrentDistance property a getter and setter that ensures correct usage
 * Fixed an issue where the player didn't die because the ChaserCurrentDistance setter prevented negative values
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
    [SerializeField] private float _chaserCurrentDistance;
    [SerializeField] private float sprintWarningThreshold = 0.7f;
    [SerializeField] private float fogMoveSpeed;

    [Tooltip("How quickly the chaser catches up the the player when not sprinting.")]
    [SerializeField] private float chaserCloseInRate; //0.025f
    [Tooltip("How quickly the player gets away from the chaser when sprinting.")]
    [SerializeField] private float sprintEscapeRate; // 0.1f
    
    private SprintSystem sprintSystem;

    /// <summary>
    /// Chaser current distance property is restricted to be less than the max chaser distance
    /// The property is accessed when the player collides with objects, so is public.
    /// </summary>
    public float ChaserCurrentDistance 
    {
        get { return this._chaserCurrentDistance; } 
        set 
        { 
            if (value <= this.chaserMaxDistance)
            {
                this._chaserCurrentDistance = value;
            }
            else
            {
                this._chaserCurrentDistance = this.chaserMaxDistance;
            }
        } 
    }

    private void Start()
    {
        this.sprintSystem = FindObjectOfType<SprintSystem>();
        this.ChaserCurrentDistance = this.chaserStartDistance;
    }

    private void FixedUpdate()
    {
        if (GameOverEvent.isPlayerDead == false)
        {
            // If the player is still alive we change the chaser distance based on whether the player is sprinting or not. 
            if (this.sprintSystem.isSprinting)
            {
                if (this.ChaserCurrentDistance < this.chaserMaxDistance)
                    this.ChaserCurrentDistance += this.sprintEscapeRate * Time.fixedDeltaTime;
            }
            else
            {
                if (this.ChaserCurrentDistance > 0.0f)
                    this.ChaserCurrentDistance -= this.chaserCloseInRate * Time.fixedDeltaTime;
            }

        }

        // Update the chaser GUI slider bar
        // To make the bar full when the chaser is close to the player, we subtract from 1.
        float sliderValue = 1 - (this.ChaserCurrentDistance / this.chaserMaxDistance);
        this.chaserSlider.value = sliderValue;

        // Determine if the sprint warning animation should be playing
        // We must signal to the player that sprinting is required as the slider reaches the max value
        if (sliderValue > this.sprintWarningThreshold)
        {
            this.sprintIndicatorAnimator.SetBool("IndicateSprint", true);
        }
        else
        {
            this.sprintIndicatorAnimator.SetBool("IndicateSprint", false);
        }

        // Game over trigger when slidervalue reaches the max and the player is still alive
        if (sliderValue > 0.99f && GameOverEvent.isPlayerDead == false && GUIFunctionality.ReturningToMenu == false)
        {
            this.gameOverEvent.playerDeath.Invoke();
            this.ChaserCurrentDistance = 0;
        }

        // Update chaser GUI fog position using the position animation curve
        // to make the fog encompass the screen more as the slidervalue increases
        Vector3 targetFogPos = new Vector3(this.movingFog.transform.localPosition.x, this.fogYPositionCurve.Evaluate(sliderValue), this.movingFog.transform.localPosition.z);
        this.movingFog.transform.localPosition = Vector3.MoveTowards(this.movingFog.transform.localPosition, targetFogPos, this.fogMoveSpeed);

    }
}
