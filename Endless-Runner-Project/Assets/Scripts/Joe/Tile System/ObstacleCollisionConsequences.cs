using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OBSTACLE COLLISION CONSEQUENCES CLASS
 * Author(s): Joe Bevis
 * Date last modified: 11/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 *
 */
public class ObstacleCollisionConsequences : MonoBehaviour
{
    // Private variables
    private float slowDownDuration;
    /// <summary>
    /// A variable used to keep track of current slow down animation curve progress
    /// </summary>
    private float slowDownAnimationTime;
    private AnimationCurve chosenSlowDownCurve;
    private TileSpeedManagement tileSpeedManagement;
    private ChaserMechanic chaserMechanic;

    [Header("Inspector Set References")]
    [SerializeField] private ConfigurablePlayerSlowDown[] playerSlowDowns;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem collisionParticles;
    [SerializeField] private AudioSource collisionImpactAudio;
    [SerializeField] private AudioSource collisionGruntAudio;


    /// <summary>
    /// A struct for setting up a configurable consequence for hitting an obstacle.
    /// </summary>
    [System.Serializable]
    public struct ConfigurablePlayerSlowDown
    {
        [Tooltip("A curve of player speed to time out of total slow down duration.")]
        public AnimationCurve slowDownCurve;
        [Tooltip("How much the chaser catches up to the player.")]
        public float chaserCatchUpAmount;
        public float specificSlowDownDuration;
    }

    void Start()
    {
        this.slowDownAnimationTime = this.slowDownDuration;
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.chaserMechanic = FindObjectOfType<ChaserMechanic>();
    }

    public void StumbleSlowDown(int slowDownIndex)
    {
        // Trigger the correct animation and effects
        this.playerAnimator.Play("Stumble");
        this.playerAnimator.ResetTrigger("Run");
        this.playerAnimator.ResetTrigger("Slide");
        this.playerAnimator.ResetTrigger("Jump");
        this.collisionParticles.Play();
        this.collisionImpactAudio.Play();
        this.collisionGruntAudio.Play();

        // Get the consequence of the slow down based on the slowDownIndex value passed in
        ConfigurablePlayerSlowDown configurablePlayerSlow = this.playerSlowDowns[slowDownIndex];
        this.slowDownAnimationTime = 0.0f;
        this.slowDownDuration = configurablePlayerSlow.specificSlowDownDuration;
        this.chosenSlowDownCurve = configurablePlayerSlow.slowDownCurve;
        this.chaserMechanic.ChaserCurrentDistance -= configurablePlayerSlow.chaserCatchUpAmount;
    }


    private void FixedUpdate()
    {
        // 
        if (this.slowDownAnimationTime < this.slowDownDuration)
        {
            float speedMultiplier = this.chosenSlowDownCurve.Evaluate(this.slowDownAnimationTime / this.slowDownDuration);
            this.tileSpeedManagement.externalSpeedMultiplier = speedMultiplier;
            this.slowDownAnimationTime += Time.fixedDeltaTime;
        }
        else
        {
            this.tileSpeedManagement.externalSpeedMultiplier = 1.0f;
        }
    }

}
