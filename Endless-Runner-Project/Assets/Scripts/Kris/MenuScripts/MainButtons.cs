using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainButtons : MonoBehaviour
{

    [SerializeField] private MenuHandler _menuHandler;
    [SerializeField] private Button[] Buttons;


    public void DisableButtons()
    {
        foreach(Button _button in this.Buttons)
        {
            _button.interactable = false;
        }

       
    }
    public void EnableButtons()
    {
        foreach (Button _button in this.Buttons)
        {
            _button.interactable = true;
        }
    }

    public void PlayGame()
    {
        _menuHandler.SetMenuState((int)MenuHandler.gameMenus.Credits);
    }

    public void SettingsMenu()
    {
        _menuHandler.SetMenuState((int)MenuHandler.gameMenus.Main);
    }

    public void QuitGame()
    {
        _menuHandler.SetMenuState((int)MenuHandler.gameMenus.Main);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
