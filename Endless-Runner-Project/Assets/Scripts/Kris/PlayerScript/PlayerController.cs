using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* PLAYER CONTROLLER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 18/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me. 
 * 
 */
/// <summary>
/// A class for managing the store
/// </summary>

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterManager _characterManager; //Retrieves the Character Manager to do the Movements.

    public void RotateLeft(InputAction.CallbackContext press) //Unused Debug, forces the character to rotate. Is currently unbinded.
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return; //Checks to see if you have performed a "performed" (instead of "start and cancelled")
        this._characterManager.Rotate(TurnDirection.Left); //Rotates the character left.
    }

    public void RotateRight(InputAction.CallbackContext press) //Unused Debug, forces the character to rotate.Is currently unbinded.
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Rotate(TurnDirection.Right); //Rotates the character Right.
    }

    public void MoveLeft(InputAction.CallbackContext press) //Moves the character to the left.
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Move(CharacterManager.movementDirections.left); //Tells the character manager to move the player Left
    }

    public void MoveRight(InputAction.CallbackContext press) //Moves the character to the right.
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Move(CharacterManager.movementDirections.right); //Tells the character manager to move the player Right
    }

    public void Jump(InputAction.CallbackContext press) //Allows the character to Jump.
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Jump();      //Tells the character manager to make the player Jump
    }

    public void Slide(InputAction.CallbackContext press) //Allows the player to slide
    {
        if (!press.performed || GameOverEvent.isPlayerDead) return;
        this._characterManager.Slide(); //Tells the character manager to make the player Slide
    }
}
