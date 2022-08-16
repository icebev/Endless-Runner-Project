using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GUI FUNCTIONALITY CLASS
 * Author(s): Joe Bevis
 * Date last modified: 11/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Removed private loadingManager reference
 * Commenting pass
 * 
 */
/// <summary>
/// Contains the functions used by the mid-run GUI to change scenes. 
/// Uses the loading manager for smooth transitions and to show the loading screen. 
/// </summary>
public class GUIFunctionality : MonoBehaviour
{
    public static bool ReturningToMenu = false;

    private void Start()
    {
        GUIFunctionality.ReturningToMenu = false;
    }

    /// <summary>
    /// Reload the current scene to 'restart' the run
    /// </summary>
    public void Retry()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene1(3, true, 0);
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void ReturnToMenu()
    {
        // We use this global variable to ensure the player cannot die while returning to menu which could cause coin duplication glitches.
        GUIFunctionality.ReturningToMenu = true;
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene1(2, true, 0);
    }
}
