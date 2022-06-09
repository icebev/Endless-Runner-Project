using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        _loadingManagerScript.LoadGameScene3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
