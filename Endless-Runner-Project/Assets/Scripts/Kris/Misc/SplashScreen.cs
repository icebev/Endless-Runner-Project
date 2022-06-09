using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(splashPushOver());
    }

    private IEnumerator splashPushOver()
    {
        GameObject _loadingManagerObject = GameObject.FindGameObjectWithTag("LoadManager");
        LoadingManager _loadingManagerScript = _loadingManagerObject.GetComponent<LoadingManager>();
        yield return new WaitForSeconds(3);
        _loadingManagerScript.LoadGameScene1(2, true, 0);
        yield return null;
    }


}
