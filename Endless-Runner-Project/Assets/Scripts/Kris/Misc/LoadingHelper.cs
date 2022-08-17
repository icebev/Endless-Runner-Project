using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* AUDIO VOLUME SLIDER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 17/08/2022
 *******************************************************************************
 * Important Note: Some changes have been made using Joe's computer / account by me.
 * CHANGE NOTES: 
 * 
 */
/// <summary>
/// A class for 
/// </summary>
public class LoadingHelper : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI loadingText;
    private string[] loadingTextIndex = new string[] {"Loading","Loading.", "Loading..", "Loading..."};
    private int loadingTextTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene3();
        StartCoroutine(this.LoadingTextUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadingTextUpdate()
    {
        yield return new WaitForSeconds(0.5f);
        this.loadingTextTime += 1;
        if(this.loadingTextTime > 3) { this.loadingTextTime = 0; }
        this.loadingText.text = this.loadingTextIndex[this.loadingTextTime];
        StartCoroutine(this.LoadingTextUpdate());
        yield return null;
    }
}
