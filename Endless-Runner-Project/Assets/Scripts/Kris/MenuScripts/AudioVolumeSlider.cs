using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeSlider : MonoBehaviour
{

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource slideSound;

    public void SlideLoudness(float volume)
    {
        this._audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        if (this.slideSound == null) return;
        if (!this.slideSound.isPlaying)
        {
            this.slideSound.Play();
        }
        else if (this.slideSound.time >= 0.4f)
        {
            this.slideSound.Play();
        }

    }
}
