using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OBSTACLE COLLISION CONSEQUENCES CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Added gamepad rumble config to the configurable struct
 *
 */

/// <summary>
/// A class that allows the consequences of each type of collision to be configured in the inspector.
/// </summary>
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
    private GamepadRumbleManager gamepadRumbleManager;

    [SerializeField] private ConfigurablePlayerSlowDown[] playerSlowDowns;
    [Header("Inspector Set References")]
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
        [Tooltip("How much the chaser catches up to the player upon collision.")]
        public float chaserCatchUpAmount;
        public float specificSlowDownDuration;

        // Gamepad rumble upon collision setup
        [Header("Rumble Settings")]
        public float gamepadRumbleIntensity;
        public float gamepadRumbleDuration;
    }

    void Start()
    {
        this.slowDownAnimationTime = this.slowDownDuration;
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.chaserMechanic = FindObjectOfType<ChaserMechanic>();
        this.gamepadRumbleManager = FindObjectOfType<GamepadRumbleManager>();
    }

    /// <summary>
    /// Overload that takes in a CollisionConsequenceType instead of an int.
    /// </summary>
    /// <param name="consequenceType"></param>
    public void StumbleSlowDown(CollisionConsequenceType consequenceType)
    {
        int consequenceIndex = (int)consequenceType;
        this.StumbleSlowDown(consequenceIndex);
    }

    public void StumbleSlowDown(int slowDownIndex)
    {
        // Trigger the correct animation, sfx, and particle effects
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

        // We store the struct values locally for use in fixed update where we no longer have a reference to the struct
        this.slowDownDuration = configurablePlayerSlow.specificSlowDownDuration;
        this.chosenSlowDownCurve = configurablePlayerSlow.slowDownCurve;
        this.chaserMechanic.ChaserCurrentDistance -= configurablePlayerSlow.chaserCatchUpAmount;

        // Set the gamepad to rumble
        this.gamepadRumbleManager.BeginConstantRumble(configurablePlayerSlow.gamepadRumbleIntensity, configurablePlayerSlow.gamepadRumbleIntensity, configurablePlayerSlow.gamepadRumbleDuration);
    }


    private void FixedUpdate()
    {
        // (If the slow down consequence is not complete)
        if (this.slowDownAnimationTime < this.slowDownDuration)
        {
            float speedMultiplier = this.chosenSlowDownCurve.Evaluate(this.slowDownAnimationTime / this.slowDownDuration);
            this.tileSpeedManagement.ExternalSpeedMultiplier = speedMultiplier;
            this.slowDownAnimationTime += Time.fixedDeltaTime;
        }
        else
        {
            // We must return the exteral speed multiplier to 1 once the slow down consequence has finished.
            this.tileSpeedManagement.ExternalSpeedMultiplier = 1.0f;
        }
    }

}
