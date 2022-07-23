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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        this.loadingManager.LoadGameScene1(3, true, 0);
    }

    public void ReturnToMenu()
    {
        this.loadingManager.LoadGameScene1(2, true, 0);
    }
}
