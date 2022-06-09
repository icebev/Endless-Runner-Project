using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private int targetScene = 2; //The scene the game will try to load.
    private int targetAnimationTrans = 0; //What transition animation will the game try to play

    private enum SceneAnimationTypes //Insert *actual* animation types if there is time.
    {
        swipe,
        fade,
        uhhhh,
    }


    private void Awake() //Awake runs the code regardless if it is active or not.
    {
        //Since loadManager is required in every scene, this destroys any that are not required.
        GameObject loadManagerObject = this.gameObject; 
        GameObject[] _loadManagerLimit = GameObject.FindGameObjectsWithTag("LoadManager");
        if (_loadManagerLimit.Length > 1)
        {
            Destroy(loadManagerObject);
        }
        else
        {
            DontDestroyOnLoad(loadManagerObject);
        }
        

    }

    //Takes in the target scene, whether or not it loads directly to the next scene or uses loading, and
    //what transition the scenes will make.
    public void LoadGameScene1(int sceneID, bool useLoadingScene, int animationTransID)
    {
        this.targetScene = sceneID;
        this.targetAnimationTrans = animationTransID;

        switch (useLoadingScene)
        {
            case false:
                StartCoroutine(LoadGameScene2(this.targetScene));
                break;

            case true:
                StartCoroutine(LoadGameScene2(2));
                break;

        }
    }

    private IEnumerator LoadGameScene2(int Scene) //This part loads the next scene with a delay and Async.
    {
        //-----Insert transition Animation Here-----//
        print("loading");
        yield return new WaitForSeconds(2);
        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(Scene); //Loads the Scene without freezing the game.
        while (!asyncLoading.isDone)
        {
            //Insert code to relay the % of loading
            yield return null;
        }
        
    }

    public void LoadGameScene3() //This is used by the script in the loading level. Loads the target scene.
    {

        StartCoroutine(LoadGameScene2(this.targetScene));

    }

}
