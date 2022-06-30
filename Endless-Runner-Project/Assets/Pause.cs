using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    private GameStateControls gameStateControls;
    private InputAction pauseAction;

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
            this.PauseGame();
        }
        else
        {
            this.UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
