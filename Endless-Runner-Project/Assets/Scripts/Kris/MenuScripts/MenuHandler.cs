using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* MENU HANDLER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * Restarted the code from the beginning due to coming up with more sophisticated solution
 * Added Store enum
 * Added Stats enum
 * Added How To enum
 */
/// <summary>
/// A class for handling the Menu
/// </summary>

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private Animator blocksAnim;    //This is the block transition animation that occurs on the main menu.
    [SerializeField] private GameObject[] _menus;    //This contains each menu for the main menu.
    [SerializeField] private Animator[] _menusAnimator; //Unused Legacy Menu Animator (Would have animated the menu)
    [SerializeField] private MainButtons _buttonManager; //Button manager to perform actions like disabling current buttons.
    private gameMenus _menuState = gameMenus.Main; //The different menu states
    private gameMenus _previousMenu; // Stores the previous menu state to go back.

    private void Start()
    {
        this.blocksAnim.gameObject.SetActive(true); //Sets the blocks animation gameobject to active, since it is part of a prefab used everywhere.
    }

    public enum gameMenus //The different menus within the game. Names are self-explanatory.
    {
        Main,
        Options,
        Credits,
        Quit,
        Store,
        Stats,
        HowTo,
    }

    public void ReturnMenuState() //Returns the menu to the previous menu by changin it.
    {
        ChangeMenuState(this._previousMenu);
    }

    public void ChangeMenuState(gameMenus targetMenu) //Chances the menu state to the target menu that the button manager requested.
    {
        if (this._menuState == targetMenu) return;
        StartCoroutine(transitionMenu(targetMenu));
    }

    IEnumerator transitionMenu(gameMenus targetMenu) //IEnumerator for doing timed Menu transition.
    {
        this._previousMenu = this._menuState; //Sets the previous menu state
        this._menuState = targetMenu;   //Sets menu state to target
        this._buttonManager.ToggleButtons(this._previousMenu, false); //Toggles the buttons for the current state false.
        this.blocksAnim.Play("BlocksOut"); //Plays transition
        yield return new WaitForSeconds(1f); //Waits for the transition to finish (1 second)
        this._menus[(int)this._previousMenu].SetActive(false); //Deactivates the current menu
        this._menus[(int)this._menuState].SetActive(true); //Activates the new menu
        this._buttonManager.ToggleButtons(this._menuState, false); //Disables the buttons of new menu until the transition in finishes
        this.blocksAnim.Play("BlocksIn");
        yield return new WaitForSeconds(1f);
        this._buttonManager.ToggleButtons(this._menuState, true); //Enables the buttons of the new menu
        yield return null;//Ends the IEnumerator.
    }
}

///////////// Below here is OLD legacy code. It was an inefficient way to code the menu.
///Left it here incase you'd like to see how I improved from this old system. Sorry for no comments below.

    /*

    [SerializeField] private GameObject _MainHandler;
    private MainButtons _MainButtons;

    [SerializeField] private GameObject _OptionsHandler;


    [SerializeField] private GameObject _CreditsHandler;


    void Start()
    {
        this._MainButtons = this._MainHandler.GetComponent<MainButtons>();
    }

    public enum gameMenus
    {
        Main,
        Options,
        Credits,
        Quit

    }

    private int CurrentState = (int)gameMenus.Main;

    public int GetMenuState()
    {
        return this.CurrentState;
        

    }

    public void SetMenuState(int MenuState)
    {
        if (this.CurrentState != MenuState && MenuState <= System.Enum.GetValues(typeof(gameMenus)).Length && MenuState >= 0)
        {
            switch (MenuState)
            {
                case (int)gameMenus.Main:
                    this._MainHandler.SetActive(true);
                    this._MainButtons.EnableButtons();
                    break;
                case (int)gameMenus.Options:
                    this._OptionsHandler.SetActive(true);

                    break;
                case (int)gameMenus.Credits:
                    this._CreditsHandler.SetActive(true);

                    break;


                case (int)gameMenus.Quit:
                    Application.Quit();
                    break;
            }


            switch (this.CurrentState)
            {
                case (int)gameMenus.Main:
                    this._MainHandler.SetActive(false);
                    this._MainButtons.DisableButtons();

                    break;
                case (int)gameMenus.Options:
                    this._OptionsHandler.SetActive(false);

                    break;
                case (int)gameMenus.Credits:
                    this._CreditsHandler.SetActive(false);

                    break;

            }
            this.CurrentState = MenuState;

        }

       

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

    */
