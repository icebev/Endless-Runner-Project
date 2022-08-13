using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScipt : MonoBehaviour
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


    public void TogglePause(InputAction.CallbackContext obj)
    {
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
        GamepadRumbleManager.disableRumble = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        GamepadRumbleManager.disableRumble = false;
        Time.timeScale = 1f;
    }
}
