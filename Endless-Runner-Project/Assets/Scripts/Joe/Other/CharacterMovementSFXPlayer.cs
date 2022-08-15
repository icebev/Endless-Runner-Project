using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER MOVEMENT SFX PLAYER CLASS
 * Author(s): Joe Bevis
 * Date last modified: 14/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// For playing the correct sound effects when the character moves
/// </summary>
public class CharacterMovementSFXPlayer : MonoBehaviour
{
    [Header("Inspector Set References")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource jumpGaspSound;
    [SerializeField] private AudioSource slideSound;
    [SerializeField] private AudioSource laneChangeSound;

    [SerializeField] private AudioClip[] jumpSounds;
    [SerializeField] private AudioClip[] jumpGaspSounds;

    [Tooltip("Alters the pitch of the lane change whoosh and slide sounds each time played")]
    [SerializeField] private float whooshPitchVariance;


    private AudioClip PickRandomClipFromArray(AudioClip[] clipArray)
    {
        int randomIndex = Random.Range(0, clipArray.Length);

        return clipArray[randomIndex];
    }


    public void PlayJumpSound()
    {
        AudioClip selectedJumpClip = this.PickRandomClipFromArray(this.jumpSounds);
        AudioClip selectedGaspClip = this.PickRandomClipFromArray(this.jumpGaspSounds);

        this.jumpSound.PlayOneShot(selectedJumpClip);
        this.jumpGaspSound.PlayOneShot(selectedGaspClip);
    }

    public void PlaySlideSound()
    {
        float newPitch = Random.Range(1.0f - this.whooshPitchVariance, 1.0f + this.whooshPitchVariance);
        this.slideSound.pitch = newPitch;
        this.slideSound.PlayOneShot(this.slideSound.clip);
    }

    public void PlayLaneChangeSound()
    {
        float newPitch = Random.Range(1.0f - this.whooshPitchVariance, 1.0f + this.whooshPitchVariance);
        this.laneChangeSound.pitch = newPitch;
        this.laneChangeSound.PlayOneShot(this.laneChangeSound.clip);
    }
}
