using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsScript : MonoBehaviour
{

    [SerializeField] private AudioClip[] footstepsSoundArray;
    [SerializeField] private AudioSource footstepsLeftSource;
    [SerializeField] private AudioSource footstepsRightSource;

    private void PlayLeftStepSound()
    {
        AudioClip clip = this.GetRandomFootstepSound();
        this.footstepsLeftSource.PlayOneShot(clip);
    }

    private void PlayRightStepSound()
    {
        AudioClip clip = this.GetRandomFootstepSound();
        this.footstepsRightSource.PlayOneShot(clip);
    }

    private AudioClip GetRandomFootstepSound()
    {
        int indexSelected = Random.Range(0, this.footstepsSoundArray.Length);
        return this.footstepsSoundArray[indexSelected];
    }
}
