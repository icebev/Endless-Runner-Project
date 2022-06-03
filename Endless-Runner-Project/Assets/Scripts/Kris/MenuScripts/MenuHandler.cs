using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private MainButtons _MainButtons;
    [SerializeField] private GameObject _MainHandler;


    [SerializeField] private GameObject _OptionsHandler;


    [SerializeField] private GameObject _CreditsHandler;

    private enum gameMenus
    {
        Main,
        Options,
        Credits

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
                    _MainHandler.SetActive(true);

                    break;
                case (int)gameMenus.Options:
                    _OptionsHandler.SetActive(true);

                    break;
                case (int)gameMenus.Credits:
                    _CreditsHandler.SetActive(true);

                    break;
            }


            switch (this.CurrentState)
            {
                case (int)gameMenus.Main:
                    _MainHandler.SetActive(false);

                    break;
                case (int)gameMenus.Options:
                    _OptionsHandler.SetActive(false);

                    break;
                case (int)gameMenus.Credits:
                    _CreditsHandler.SetActive(false);

                    break;

            }
            this.CurrentState = MenuState;

        }

       

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
