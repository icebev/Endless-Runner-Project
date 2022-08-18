using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER ANIMATION MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 18/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for managing animations
/// </summary>

public class CharacterAnimManager : MonoBehaviour
{
    private animationStates animationPrevious; //Old previous state, kept incase it breaks something.
    private CharacterManager.playerStates previousAnimationState = CharacterManager.playerStates.grounded; //Sets the previous playerstate for animation to grounded.
    private Animator charAnimator; //The character's animator rig.
    private CharacterManager characterManager; //The character manager used to get the animator.

    private void Start()
    {
        this.characterManager = this.gameObject.GetComponent<CharacterManager>(); //Retrieving the character manager.
        this.charAnimator = this.characterManager.GetAnimator(); //grabbing the animator stored in the manager.
    }

    public enum animationStates //The different animation states.
    {
        Slide,
        Fall, //Unused "Fall" in the final due to complications
        Jump,
        Run,
    }

    public void ManageAnimation(CharacterManager.playerStates playerState) //Manages animation, takes in the playerstate
    {
        //Check whether or not the animation has already been set to play. (So that the code doesn't continuously run, for performance)
        if (this.previousAnimationState == playerState) return; 

        this.previousAnimationState = playerState; //Sets the current playerstate to previous.

        this.charAnimator.ResetTrigger("Run"); //Reset the triggers for Run, Slide, Fall, And Jump to fix an animation bug where the wrong animation gets stuck.
        this.charAnimator.ResetTrigger("Slide");
        this.charAnimator.ResetTrigger("Fall");
        this.charAnimator.ResetTrigger("Jump");

        switch (playerState)                                //These play the animations based on the character's States.
        {
            case CharacterManager.playerStates.grounded: //Grounded turns on Run trigger.
                this.charAnimator.SetTrigger("Run");
                break;
            case CharacterManager.playerStates.crouching: //Crouching turns on Slide trigger.
                this.charAnimator.SetTrigger("Slide");
                break;
            case CharacterManager.playerStates.quickfall: //Unused quickfall. This is here as legacy.
                this.charAnimator.SetTrigger("Fall");
                break;
            case CharacterManager.playerStates.jumping: //Jumping turns on the jump trigger.
                this.charAnimator.SetTrigger("Jump");
                break;
        }
    }

    public void UpdateVelocity(float velocity) //Updates the Velocity. This is so the character can play the Roll animation when falling fast enough.
    {
        this.charAnimator.SetFloat("yVelo", velocity);
    }
}
