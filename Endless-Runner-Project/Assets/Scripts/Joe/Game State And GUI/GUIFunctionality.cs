using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIFunctionality : MonoBehaviour
{

    private LoadingManager loadingManager;
    // Start is called before the first frame update
    void Start()
    {
        this.loadingManager = FindObjectOfType<LoadingManager>();
    }


    public void Retry()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene1(3, true, 0);
    }

    public void ReturnToMenu()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene1(2, true, 0);
    }
}
