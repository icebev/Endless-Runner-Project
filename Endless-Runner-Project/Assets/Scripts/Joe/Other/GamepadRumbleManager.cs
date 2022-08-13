using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* GAMEPAD RUMBLE MANAGER CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Refactored the code in the update loop to avoid unncessecary calls of the StopRumble() method.
 */

/// <summary>
/// A class for adding gamepad rumble as an option for consequential feedback
/// </summary>
public class GamepadRumbleManager : MonoBehaviour
{
    // Global disableRumble variable can be used by any other script in situations where a controller rumble could break,
    // for example on pause where timescale 0 might make the gamepad vibrate indefinitely.
    public static bool disableRumble = false;

    [SerializeField] private PlayerInput playerInput;
    private Gamepad gamepad;
    private float remainingRumbleTime;
    private bool rumbling;

    private void Update()
    {
        if (this.remainingRumbleTime > 0.0f)
        {
            if (GamepadRumbleManager.disableRumble == true)
            {
                this.StopRumble();
                return;
            }
            else
            {
                this.remainingRumbleTime -= Time.deltaTime;
            }
        }
        else if (this.rumbling)
        {
            this.StopRumble();
        }

    }

    /// <summary>
    /// Set the gamepad to rumble at a constant intensity for a set period.
    /// </summary>
    /// <param name="lowFreqSpeed">Intensity (0 to 1) of the low frequency motor</param>
    /// <param name="highFreqSpeed">Intensity (0 to 1) of the high frequency motor</param>
    /// <param name="duration"> How long the gamepad will vibrate for</param>
    public void BeginConstantRumble(float lowFreqSpeed, float highFreqSpeed, float duration)
    {
        if (this.gamepad == null)
        {
            this.gamepad = this.GetGamepad();
        }

        this.gamepad?.SetMotorSpeeds(lowFreqSpeed, highFreqSpeed);
        this.remainingRumbleTime = duration;
        this.rumbling = true;
    }

    public void StopRumble()
    {
        if (this.gamepad == null)
        {
            this.gamepad = this.GetGamepad();
        }

        this.gamepad?.SetMotorSpeeds(0, 0);
        this.remainingRumbleTime = 0;
        this.rumbling = false;
    }

    /* BEGINNING OF CITED CODE
     * Title: UnityInputSystem_ControlerRumble (Rumbler.cs)
     * Author: Srfigie 
     * Date Published: 11/06/2020
     * Date Accessed: 12/08/2022
     * URL: https://github.com/Srfigie/UnityInputSystem_ControlerRumble/blob/master/Assets/Scripts/Rumbler.cs
     * Usage: Used for implementation of controller rumble system to create rumble effect upon obstacle collision.
     */

    private Gamepad GetGamepad()
    {
        Gamepad gamepad = null;
        foreach (var g in Gamepad.all)
        {
            foreach (var d in this.playerInput.devices)
            {
                if (d.deviceId == g.deviceId)
                {
                    gamepad = g;
                    break;
                }
            }
            if (gamepad != null)
            {
                break;
            }
        }
        return gamepad;
    }

    // END OF CITED CODE
}
