using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/* SET AUDIO CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for setting the audio to player-set audio.
/// </summary>
/// 
public class SetAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer[] _audioMixers; //AudioMixers such as SFX and Music
    private enum audioMixers //Enum for different audio mixers
    {
        SFX,
        Music,
    }
    private void Start()
    {
        int FirstTimeLaunch = PlayerPrefs.GetInt("FirstTimeLaunch"); //Used to see if the game hasn't been launched before
        float sfxVolume; //Volumes for SFX and Music
        float musicVolume;

        switch (FirstTimeLaunch)
        {
            case 1:
                sfxVolume = PlayerPrefs.GetFloat("SfxMixerValue"); //Retrieving the user's values if they have launched the game before.
                musicVolume = PlayerPrefs.GetFloat("MusicMixerValue");
                break;
            default:
                sfxVolume = 1; //Setting audio settings for if the user hasn't launched the game before.
                musicVolume = 1;
                PlayerPrefs.SetInt("FirstTimeLaunch", 1);
                break;
        }
        
        //Setting the audio of the mixers from what the player had selected
        this._audioMixers[(int)audioMixers.SFX].SetFloat("Volume", Mathf.Log10(sfxVolume) * 20);
        this._audioMixers[(int)audioMixers.Music].SetFloat("Volume", Mathf.Log10(musicVolume) * 20);

        //Deleting the gameobject cause it is not needed.
        Destroy(this.gameObject);
    }
}
