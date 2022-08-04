using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Animator blocksAnim;
    [SerializeField] private GameObject[] _menus;
    [SerializeField] private Animator[] _menusAnimator;
    [SerializeField] private MainButtons _buttonManager;
    private gameMenus _menuState = gameMenus.Main;
    private gameMenus _previousMenu;

    private void Start()
    {
        this.blocksAnim.gameObject.SetActive(true);
    }

    public enum gameMenus
    {
        Main,
        Options,
        Credits,
        Quit,
        Store,
    }

    public void ReturnMenuState()
    {
        ChangeMenuState(this._previousMenu);
    }

    public void ChangeMenuState(gameMenus targetMenu)
    {

        if (this._menuState == targetMenu) return;
        StartCoroutine(transitionMenu(targetMenu));
        /*
        this._previousMenu = this._menuState;
        this._menuState = targetMenu;
        this._buttonManager.ToggleButtons(this._previousMenu, false);
        this._menus[(int)this._previousMenu].SetActive(false);  //Change this for animation
        this._buttonManager.ToggleButtons(this._menuState, true);
        this._menus[(int)this._menuState].SetActive(true); //Change this for animation
        */

    }

    IEnumerator transitionMenu(gameMenus targetMenu)
    {
        this._previousMenu = this._menuState;
        this._menuState = targetMenu;
        this._buttonManager.ToggleButtons(this._previousMenu, false);
        this.blocksAnim.Play("BlocksOut");
        yield return new WaitForSeconds(0.75f);
        this._menus[(int)this._previousMenu].SetActive(false);
        this._menus[(int)this._menuState].SetActive(true);
        this._buttonManager.ToggleButtons(this._menuState, false);
        this.blocksAnim.Play("BlocksIn");
        yield return new WaitForSeconds(0.75f);
        this._buttonManager.ToggleButtons(this._menuState, true);
        yield return null;
    }

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
}