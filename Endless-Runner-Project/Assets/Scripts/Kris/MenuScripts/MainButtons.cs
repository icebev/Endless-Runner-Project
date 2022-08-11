using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainButtons : MonoBehaviour
{
    [SerializeField] private GameObject fadeObj;
    [SerializeField] private MenuHandler _menuHandler;

    [SerializeField] private Button[] _menuButtons;
    [SerializeField] private Button[] _optionsButtons;
    [SerializeField] private Button[] _creditsButtons;
    [SerializeField] private Button[] _quitButtons;
    [SerializeField] private Button[] _storeButtons;
    [SerializeField] private Button[] _statsButtons;
    //[SerializeField] private Button[] _;



    public void ToggleButtons(MenuHandler.gameMenus WhichMenu, bool ButtonEnable)
    {

        Button[] _GameButtons = this._menuButtons;

        switch (WhichMenu)
        {
            case MenuHandler.gameMenus.Main:
                _GameButtons = this._menuButtons;
                break;
            case MenuHandler.gameMenus.Credits:
                _GameButtons = this._creditsButtons;
                break;
            case MenuHandler.gameMenus.Options:
                _GameButtons = this._optionsButtons;
                break;
            case MenuHandler.gameMenus.Quit:
                _GameButtons = this._quitButtons;
                break;
            case MenuHandler.gameMenus.Store:
                _GameButtons = this._storeButtons;
                break;
            case MenuHandler.gameMenus.Stats:
                _GameButtons = this._statsButtons;
                break;

        }

        foreach (Button _button in _GameButtons)
        {
            _button.interactable = ButtonEnable;
        }
    }

    public void PlayGame()
    {
        ToggleButtons(MenuHandler.gameMenus.Main, false);
        this.fadeObj.SetActive(true);
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene1(3, true, 0);
    }

    public void SettingsMenu()
    {
        _menuHandler.ChangeMenuState(MenuHandler.gameMenus.Options);
    }
    public void CreditsMenu()
    {
        _menuHandler.ChangeMenuState(MenuHandler.gameMenus.Credits);
    }


    public void QuitGame()
    {
        _menuHandler.ChangeMenuState(MenuHandler.gameMenus.Quit);
    }

    public void GoBack()
    {

        _menuHandler.ReturnMenuState();
    }

    public void Store()
    {
        _menuHandler.ChangeMenuState(MenuHandler.gameMenus.Store);
    }

    public void Stats()
    {
        _menuHandler.ChangeMenuState(MenuHandler.gameMenus.Stats);
    }

    public void ExitGame()
    {
        Application.Quit();

    }
     
}
