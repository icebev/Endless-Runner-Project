using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainButtons : MonoBehaviour
{

    [SerializeField] private MenuHandler _menuHandler;

    [SerializeField] private Button[] _menuButtons;
    [SerializeField] private Button[] _optionsButtons;
    [SerializeField] private Button[] _creditsButtons;
    [SerializeField] private Button[] _quitButtons;
    //[SerializeField] private Button[] _;



    public void ToggleButtons(int WhichMenu, bool ButtonEnable)
    {

        Button[] _GameButtons = this._menuButtons;

        switch (WhichMenu)
        {
            case (int)MenuHandler.gameMenus.Main:
                _GameButtons = this._menuButtons;
                break;
            case (int)MenuHandler.gameMenus.Credits:
                _GameButtons = this._creditsButtons;
                break;
            case (int)MenuHandler.gameMenus.Options:
                _GameButtons = this._optionsButtons;
                break;
            case (int)MenuHandler.gameMenus.Quit:
                _GameButtons = this._quitButtons;
                break;

        }

        foreach(Button _button in _GameButtons)
        {
            switch (ButtonEnable)
            {
                case true:
                    _button.interactable = true;
                    break;
                case false:
                    _button.interactable = false;
                    break;

            }
            
        }

       
    }

    public void PlayGame()
    {
        _menuHandler.ChangeMenuState((int)MenuHandler.gameMenus.Credits);
    }

    public void SettingsMenu()
    {
        _menuHandler.ChangeMenuState((int)MenuHandler.gameMenus.Options);
    }
    public void CreditsMenu()
    {
        _menuHandler.ChangeMenuState((int)MenuHandler.gameMenus.Credits);
    }


    public void QuitGame()
    {
        _menuHandler.ChangeMenuState((int)MenuHandler.gameMenus.Quit);
    }

    public void GoBack()
    {

        _menuHandler.ReturnMenuState();
    }


    public void ExitGame()
    {
        Application.Quit();

    }
     
}
