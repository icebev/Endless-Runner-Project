using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OPTIONS CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for the options menu.
/// </summary>
public class Options : MonoBehaviour
{
    //Used by a button.
    public void ResetPrefs()
    {
        //The music value is carried over because people want to reset their save and not lose audio chance.
        float sfxVolume = PlayerPrefs.GetFloat("SfxMixerValue");   
        float musicVolume = PlayerPrefs.GetFloat("MusicMixerValue");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("FirstTimeLaunch", 1); //First time launch is used by volume to be set to max. This shouldn't be reset
        PlayerPrefs.SetFloat("SfxMixerValue", sfxVolume);
        PlayerPrefs.SetFloat("MusicMixerValue", musicVolume);
    }
}
