using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer[] _audioMixers;
    private enum audioMixers
    {
        SFX,
        Music,
    }

    private void Start()
    {
        int FirstTimeLaunch = PlayerPrefs.GetInt("FirstTimeLaunch");
        float sfxVolume;
        float musicVolume;

        switch (FirstTimeLaunch)
        {
            case 1:
                sfxVolume = PlayerPrefs.GetFloat("SfxMixerValue");
                musicVolume = PlayerPrefs.GetFloat("MusicMixerValue");
                break;
            default:
                sfxVolume = 1;
                musicVolume = 1;
                PlayerPrefs.SetInt("FirstTimeLaunch", 1);
                break;
        }
        
        this._audioMixers[(int)audioMixers.SFX].SetFloat("Volume", Mathf.Log10(sfxVolume) * 20);
        this._audioMixers[(int)audioMixers.Music].SetFloat("Volume", Mathf.Log10(musicVolume) * 20);

        Destroy(this.gameObject);
    }
}
