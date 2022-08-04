using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CHARACTER ANIMATION MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 01/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for managing animations
/// </summary>

public class CharacterAnimManager : MonoBehaviour
{
    private animationStates animationPrevious;
    private CharacterManager.playerStates previousAnimationState = CharacterManager.playerStates.grounded;
    private Animator charAnimator;
    private CharacterManager characterManager;


    private void Start()
    {
        this.characterManager = this.gameObject.GetComponent<CharacterManager>();
        this.charAnimator = this.characterManager.GetAnimator();
    }

    public enum animationStates
    {
        Slide,
        Fall,
        Jump,
        Run,
    }

    public void ManageAnimation(CharacterManager.playerStates playerState)
    {
        if (this.charAnimator.GetCurrentAnimatorStateInfo(0).IsName("Running") && playerState == CharacterManager.playerStates.grounded && this.charAnimator.GetBool("Run")) //To fix a bug where animation can get stuck at "Falling Idle" when grounded
        {
            this.charAnimator.SetTrigger("Run");

        }
        if (this.previousAnimationState == playerState) return; //Check whether or not the animation has already been set to play.

        this.previousAnimationState = playerState;
        switch (playerState)                                //These play the animations based on the character's States.
        {
            case CharacterManager.playerStates.grounded:
                this.charAnimator.SetTrigger("Run");
                break;
            case CharacterManager.playerStates.crouching:
                this.charAnimator.SetTrigger("Slide");
                break;
            case CharacterManager.playerStates.quickfall:
                this.charAnimator.SetTrigger("Fall");
                break;
            case CharacterManager.playerStates.jumping:
                this.charAnimator.SetTrigger("Jump");
                break;
        }
    }

    public void UpdateVelocity(float velocity)
    {
        this.charAnimator.SetFloat("yVelo", velocity);
    }
}
