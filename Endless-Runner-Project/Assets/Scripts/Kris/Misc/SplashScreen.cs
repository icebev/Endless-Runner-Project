using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    /* SPLASH SCREEN CLASS
    * Author(s): Kris Burgess-James
    * Date last modified: 17/08/2022
    *******************************************************************************
    * 
    */
    /// <summary>
    /// A class for Managing splash screen
    /// </summary>
    void Start()
    {
        StartCoroutine(splashPushOver()); //Starts the coroutine for the splash screen.
    }

    private IEnumerator splashPushOver() //Coroutine
    {
        //It finds the loading manager object and retrieves the script.
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        yield return new WaitForSeconds(3); //Waits until splashscreen is over.
        _loadingManagerScript.LoadGameScene1(2, true, 0); //tells the loading manager to load the main menu.
        yield return null; //Returns null
    }
}
