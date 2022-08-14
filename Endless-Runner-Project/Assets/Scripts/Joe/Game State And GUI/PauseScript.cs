using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/* PAUSE SCRIPT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 14/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Corrected class name typo
 * Commenting pass
 */
/// <summary>
/// A class for pausing the game mid run by setting the timescale to zero.
/// </summary>
public class PauseScript : MonoBehaviour
{
    private GameStateControls gameStateControls;
    private InputAction pauseAction;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    private void OnEnable()
    {
        this.gameStateControls = new GameStateControls();
        this.pauseAction = this.gameStateControls.StateControlActions.Pause;
        this.pauseAction.Enable();
        this.pauseAction.performed += this.TogglePause;
    }

    // When there is a pause input such as escape or start on the controller, toggles pause.
    public void TogglePause(InputAction.CallbackContext obj)
    {
        // Only toggle if the player is still alive
        if (GameOverEvent.isPlayerDead == false) return;

        if (Time.timeScale != 0)
        {
            this.pauseButton.onClick.Invoke();
        }
        else
        {
            this.resumeButton.onClick.Invoke();
        }
    }

    public void PauseGame()
    {
        // We must disable the gamepad rumble while the game is paused
        // since the gampad rumble uses time to determine how long it should rumble for.
        GamepadRumbleManager.disableRumble = true;
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        GamepadRumbleManager.disableRumble = false;
        Time.timeScale = 1.0f;
    }
}
