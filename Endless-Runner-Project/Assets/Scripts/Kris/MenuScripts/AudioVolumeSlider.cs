using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource slideSound;
    [SerializeField] private Slider slider;
    [SerializeField] private bool isSfxSlider;
    private float storedVolume;

    private void OnEnable()
    {
        float CurrentVolume;
        this._audioMixer.GetFloat("Volume", out CurrentVolume);
        this.slider.value = Mathf.Pow(10,(CurrentVolume / 20));
    }

    public void SlideLoudness(float volume)
    {

        this.storedVolume = volume;
        this._audioMixer.SetFloat("Volume", Mathf.Log10(this.storedVolume) * 20);

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
    }

    public void MouseUpEvent()
    {
        switch (this.isSfxSlider)
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
