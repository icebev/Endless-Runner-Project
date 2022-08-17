using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/* AUDIO VOLUME SLIDER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * Sound doesn't play constantly when sliding for SFX slider
 */
/// <summary>
/// A class for changing the AudioMixers via the sliders in the Options Menu.
/// </summary>

public class AudioVolumeSlider : MonoBehaviour
{

    [SerializeField] private AudioMixer _audioMixer; //To adjust the SFX Or Music mixers
    [SerializeField] private AudioSource slideSound; //The sound to play for the SFX slider
    [SerializeField] private Slider slider; //The Audio-sliders for SFX or Music mixers
    [SerializeField] private bool isSfxSlider; //Bool to see if it is a SFX slider (to play audio after or not)
    private float storedVolume; //To store the current volume into a variable

    ////// The Menu system works by enabling the game objects. The current volume is retrieved
    ////// when you open the Menu and sets the sliders visually to what your current volume is.
    private void OnEnable() 
    {
        float CurrentVolume; 
        this._audioMixer.GetFloat("Volume", out CurrentVolume);
        this.slider.value = Mathf.Pow(10,(CurrentVolume / 20)); //To calculate from Log10 to a 0-1 value
    }
    //////


    ////// The sliders use this function, by passing in the value and set into the audioMixer.
    public void SlideLoudness(float volume)
    {
        this.storedVolume = volume;
        this._audioMixer.SetFloat("Volume", Mathf.Log10(this.storedVolume) * 20);

        ////// This is the Legacy SFX slide audio, where it'll repeat a sound every so often.
        ////// It was changed to a more favourable option.
        /*
        if (this.slideSound == null) return;

        if (!this.slideSound.isPlaying)
        {
            this.slideSound.Play();
        }
        else if (this.slideSound.time >= 0.4f)
        {
            this.slideSound.Play();
        }
        */
        //////
    }

    ////// This function is a mouse-up when you let go of the slider
    ////// (Note: Couldn't use unity's built in mouse-up) 
    ////// The function sets the value of the mixers as a PlayerPref, so that the volume can be re-applied in 
    ////// different scenes.
    public void MouseUpEvent()
    {
        switch (this.isSfxSlider) //Used to play the sound and set the volume mixer pref
        {
            case true:
                PlayerPrefs.SetFloat("SfxMixerValue", this.storedVolume);
                this.slideSound.Play();
                break;

            case false:
                PlayerPrefs.SetFloat("MusicMixerValue", this.storedVolume);
                break;
        }
    }
}
