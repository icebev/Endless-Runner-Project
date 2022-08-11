using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public void ResetPrefs()
    {
        float sfxVolume = PlayerPrefs.GetFloat("SfxMixerValue");
        float musicVolume = PlayerPrefs.GetFloat("MusicMixerValue");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("FirstTimeLaunch", 1);
        PlayerPrefs.SetFloat("SfxMixerValue", sfxVolume);
        PlayerPrefs.SetFloat("MusicMixerValue", musicVolume);
    }
}
