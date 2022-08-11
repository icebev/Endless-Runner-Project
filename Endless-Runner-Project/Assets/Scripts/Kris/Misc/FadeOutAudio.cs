using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FadeOutAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer _music;
    private float value = 1;

    private void OnEnable()
    {
        this.value = PlayerPrefs.GetFloat("MusicMixerValue");
    }
    private void FixedUpdate()
    {

        if (!(this.value < 0.005f)) { this.value -= 0.01f; }
        if ( this.value < 0.005f ) { this.value = 0.005f; }
        this._music.SetFloat("Volume", Mathf.Log10(this.value) * 20);

    }
}
