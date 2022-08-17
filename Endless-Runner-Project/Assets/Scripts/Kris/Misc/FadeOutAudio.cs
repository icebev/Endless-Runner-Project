using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/* FADE OUT AUDIO CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 *
 */
/// <summary>
/// A class for fading out the music audio;
/// </summary>
public class FadeOutAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer _music; //Music Audiomixer that needs to fade out.
    private float value = 1; //How loud the music is.

    private void OnEnable()
    {
        this.value = PlayerPrefs.GetFloat("MusicMixerValue"); //Gets the current value of the AudioMixer.
    }
    private void FixedUpdate()
    {

        if (!(this.value < 0.005f)) { this.value -= 0.01f; }
        if ( this.value < 0.005f ) { this.value = 0.005f; } //Reduces the value of the Auiomixer before applying it to the actual mixer, allowing for a fade-outy sound.
        this._music.SetFloat("Volume", Mathf.Log10(this.value) * 20);

    }
}
