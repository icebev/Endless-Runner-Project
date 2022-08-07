using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollisionConsequences : MonoBehaviour
{

    private float slowDownDuration;
    private float slowDownAnimationTime;
    private AnimationCurve chosenSlowDownCurve;

    public Animator playerAnimator;

    private TileSpeedManagement tileSpeedManagement;
    private TileSpeedIncrementation speedIncrementation;
    private ChaserMechanic chaserMechanic;

    public ConfigurablePlayerSlowDown[] playerSlowDowns;
    public ParticleSystem collisionParticles;
    public AudioSource collisionImpactAudio;
    public AudioSource collisionGruntAudio;

    [System.Serializable]
    public struct ConfigurablePlayerSlowDown
    {
        public AnimationCurve slowDownCurve;
        public float chaserCatchUpAmount;
        public float specificSlowDownDuration;

    }

    void Start()
    {
        this.slowDownAnimationTime = this.slowDownDuration;

        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.speedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.chaserMechanic = FindObjectOfType<ChaserMechanic>();

    }

    public void StumbleSlowDown(int slowDownIndex)
    {
        this.playerAnimator.Play("Stumble");
        this.playerAnimator.ResetTrigger("Run");
        this.playerAnimator.ResetTrigger("Slide");
        this.playerAnimator.ResetTrigger("Jump");
        this.collisionParticles.Play();
        this.collisionImpactAudio.Play();
        this.collisionGruntAudio.Play();

        ConfigurablePlayerSlowDown configurablePlayerSlow = this.playerSlowDowns[slowDownIndex];
        this.slowDownAnimationTime = 0.0f;
        this.slowDownDuration = configurablePlayerSlow.specificSlowDownDuration;
        this.chosenSlowDownCurve = configurablePlayerSlow.slowDownCurve;
        this.chaserMechanic.chaserCurrentDistance -= configurablePlayerSlow.chaserCatchUpAmount;
    }


    private void FixedUpdate()
    {
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
