using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* MAIN BUTTONS CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * Added store
 * Added stats
 * Added how to
 * Created "AllButtons" to store the arrays of the other buttons. It made the code smaller and more managable.
 */
/// <summary>
/// A class for managing the buttons in the Main Menu.
/// </summary>

public class MainButtons : MonoBehaviour
{
    [SerializeField] private GameObject fadeObj; //Fade Object used to Fade Out after pressing "Play"
    [SerializeField] private MenuHandler _menuHandler; //The menu handler that works with the buttons.

    ////// These are all the different buttons that are located in the Main Menu.
    ////// For instance, the _menuButtons would contain the buttons to go to Store, Quit, Options, Stats, Etc.
    ////// Variables are self explanatory
    [SerializeField] private Button[] _menuButtons;
    [SerializeField] private Button[] _optionsButtons;
    [SerializeField] private Button[] _creditsButtons;
    [SerializeField] private Button[] _quitButtons;
    [SerializeField] private Button[] _storeButtons;
    [SerializeField] private Button[] _statsButtons;
    [SerializeField] private Button[] _howToButtons;
    private Button[][] AllButtons;
    //////

    private void Start()
    {
        //Groups all the buttons into "All Buttons" which is used by toggle buttons.
        //Essentially this ensures that the buttons can easily be Interactive or Not at ease.
        this.AllButtons = new Button[][] 
        { 
            this._menuButtons,  this._optionsButtons, this._creditsButtons, this._quitButtons, 
            this._storeButtons, this._statsButtons, this._howToButtons
        };
    }

    //Recieves the current menu and toggles whether the button should be enabled or disabled.
    public void ToggleButtons(MenuHandler.gameMenus WhichMenu, bool ButtonEnable)
    {
        Button[] _GameButtons = this.AllButtons[(int)WhichMenu]; //Grabs the specific menu buttons and sets it.
        foreach (Button _button in _GameButtons) _button.interactable = ButtonEnable;
    }

    //The button behaviour for when you press play.
    public void PlayGame()
    {
        ToggleButtons(MenuHandler.gameMenus.Main, false); //Toggles buttons off cause the Menu Manager isn't used here.
        this.fadeObj.SetActive(true);                     //Turns on the fade-out.

        ////// This searches for the Loading Manager GameObject using the tag, then uses it to retrieve the loading manager script.
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        //////
        // Tells the loading manager to prepare to load the Runner ("Main") Scene, with the loading screen and fade animation.
        _loadingManagerScript.LoadGameScene1(3, true, 0);
        
    }

    //The button behaviour for when you press Settings
    public void SettingsMenu()
    {
        //Changes the menu state to settings.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.Options);
    }

    //The button behaviour for when you press Credits
    public void CreditsMenu()
    {
        //Changes the menu state to the Credits screen.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.Credits);
    }

    //The button behaviour for when you press Quit
    public void QuitGame()
    {
        //Changes the menu state to Quit state, which essentially quits the game.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.Quit);
    }

    //The button behaviour for when you press Go Back
    public void GoBack()
    {
        //Changes the menu state the previous menu.
        this._menuHandler.ReturnMenuState();
    }

    //The button behaviour for when you press Store
    public void Store()
    {
        //Changes the menu state to Store screen.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.Store);
    }

    //The button behaviour for when you press Stats
    public void Stats()
    {
        //Changes the menu state to Statistics menu.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.Stats);
    }

    public void HowTo()
    {
        //Changes the menu state to How To screen.
        this._menuHandler.ChangeMenuState(MenuHandler.gameMenus.HowTo);
    }

    //The button behaviour for when you press Exit game.
    public void ExitGame()
    {
        Application.Quit();
    }
}
