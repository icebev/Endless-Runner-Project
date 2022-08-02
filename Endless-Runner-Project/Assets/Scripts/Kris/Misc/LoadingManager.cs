using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private int targetScene = 2; //The scene the game will try to load.
    private int targetAnimationTrans = 0; //What transition animation will the game try to play
    private Slider loadingBar;
    private bool currentlyLoading = false;

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
            DontDestroyOnLoad(this.gameObject);
        }
        

    }

    //Takes in the target scene, whether or not it loads directly to the next scene or uses loading, and
    //what transition the scenes will make.
    public void LoadGameScene1(int sceneID, bool useLoadingScene, int animationTransID)
    {
        if (this.currentlyLoading == true) return;
        this.currentlyLoading = true;
        this.targetScene = sceneID;
        this.targetAnimationTrans = animationTransID;

        switch (useLoadingScene)
        {
            case false:
                StartCoroutine(LoadGameScene4(this.targetScene));
                break;

            case true:
                StartCoroutine(LoadGameScene2(1));
                break;

        }
    }

    private IEnumerator LoadGameScene2(int Scene) //This part loads the next scene with a delay and Async.
    {
        //-----Insert transition Animation Here-----//
        print("loading");

        GameObject Transition = GameObject.FindGameObjectWithTag("Transitions");
        if (Transition != null)
        {
            Transform FadeObject = Transition.transform.Find("Fade");
            Animator FadeAnim = FadeObject.GetComponent<Animator>();
            FadeAnim.Play("FadeOut");
        }
        yield return new WaitForSeconds(2);
        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(Scene); //Loads the Scene without freezing the game.
        while (!asyncLoading.isDone)
        {
            //Insert code to relay the % of loading
            if(this.loadingBar != null)
            {
                this.loadingBar.value = 0.1f + asyncLoading.progress;
            }

            print(asyncLoading.progress);
            yield return null;
        }
        this.currentlyLoading = false;
    }

    public void LoadGameScene3() //This is used by the script in the loading level. Loads the target scene.
    {
        GameObject loadingBarObject = GameObject.Find("Loading Bar");
        this.loadingBar = loadingBarObject.GetComponent<Slider>();
        this.loadingBar.value = 0;
        StartCoroutine(LoadGameScene2(this.targetScene));

    }

    public IEnumerator LoadGameScene4(int Scene) //This loads a scene by skipping the Loading Screen.
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(Scene);
        yield return null;
    }

}
