using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FOOTSTEPS SCRIPT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// Used to play footstep audio in sync with the player's running animation to add ambience and realism to the game.
/// </summary>
public class FootstepsScript : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepsSoundArray;
    [SerializeField] private AudioSource footstepsLeftSource;
    [SerializeField] private AudioSource footstepsRightSource;


    /* RESEARCH SOURCE REFERENCE
     * Title: Adding Footstep Sounds to your Unity game with Animation Events
     * Author: Jason Weimann
     * Date Published: 18/10/2017
     * Date Accessed: 27/07/2022
     * URL: https://www.youtube.com/watch?v=Bnm8mzxnwP8
     * Usage: Discovered how to call functions as part of an animation event.
     */

    /// <summary>
    /// Plays footstep audio on the left-hand audio source
    /// </summary>
    private void PlayLeftStepSound()
    {
        AudioClip clip = this.GetRandomFootstepSound();
        this.footstepsLeftSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Plays footstep audio on the right-hand audio source
    /// </summary>
    private void PlayRightStepSound()
    {
        AudioClip clip = this.GetRandomFootstepSound();
        this.footstepsRightSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Pick a random footstep sound clip from the bank of clips
    /// </summary>
    /// <returns>A single footstep sound effect audio clip</returns>
    private AudioClip GetRandomFootstepSound()
    {
        int indexSelected = Random.Range(0, this.footstepsSoundArray.Length);
        return this.footstepsSoundArray[indexSelected];
    }
}
