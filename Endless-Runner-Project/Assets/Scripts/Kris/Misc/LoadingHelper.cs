using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* LOADING HELPER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * CHANGE NOTES: 
 * Made it so that the loading text does dots.
 */
/// <summary>
/// A class for helping the Loading Manager get to the next stage of loading the next scene. It also does stuff in the loading scene.
/// </summary>
public class LoadingHelper : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI loadingText; //text object that shows "Loading"
    private string[] loadingTextIndex = new string[] {"Loading","Loading.", "Loading..", "Loading..."}; //Text index that shows loading text
    private int loadingTextTime = 0; //Incorrect name for loading text index.

    void Start()
    {
        //Retrieves the loading manager object to get the loading manager script.
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();

        //Runs the next part of the loading manager.
        _loadingManagerScript.LoadGameScene3();

        //Updates the text.
        StartCoroutine(this.LoadingTextUpdate());
    }

    private IEnumerator LoadingTextUpdate() //Used to update the text.
    {
        yield return new WaitForSeconds(0.5f); //Waits 0.5 seconds to update the text.
        this.loadingTextTime += 1; //Adds to the index
        if(this.loadingTextTime > 3) { this.loadingTextTime = 0; } //Resets to 0 if above index (3)
        this.loadingText.text = this.loadingTextIndex[this.loadingTextTime]; //Sets the text to the text inex.
        StartCoroutine(this.LoadingTextUpdate()); //recalls the loading text update to update the text.
        yield return null;
    }
}
