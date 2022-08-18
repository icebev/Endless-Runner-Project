using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* LOADING MANAGER CLASS
 * Author(s): Kris Burgess-James
 * Date last modified: 18/08/2022
 *******************************************************************************
 * 
 */
/// <summary>
/// A class for loading scenes through a manager.
/// </summary>


public class LoadingManager : MonoBehaviour
{
    private int targetScene = 2; //The scene the game will try to load.
    private int targetAnimationTrans = 0; //Legacy Animation Target
    private Slider loadingBar; //The loading bar slider
    private bool currentlyLoading = false;  //Used to see if the game is still loading
    private Animator FadeAnim; //The fade in and out animation the animator uses
    private bool transition = false; //Whether or not Scene transitions should be used

    private enum SceneAnimationTypes //Old legacy Enum that I don't want to change incase it breaks something.
    {
        swipe,
        fade,
        uhhhh,
    }


    private void Awake() //Awake runs the code regardless if it is active or not.
    {
        //This searches for the loading manager, and checks to see if there is more than one.
        GameObject loadManagerObject = this.gameObject; 
        GameObject[] _loadManagerLimit = GameObject.FindGameObjectsWithTag("LoadManager");
        
        if (_loadManagerLimit.Length > 1)
        { 
            Destroy(loadManagerObject); //When there is an extra one, it deletes it.
        }
        else
        {
            DontDestroyOnLoad(this.gameObject); //Makes sure that the current loading manager doesn't get deleted.
        }
    }

    //Takes in a Target Scene ID, whether or not the loading scene should be used, and what type of animation should be used.
    public void LoadGameScene1(int sceneID, bool useLoadingScene, int animationTransID)
    {
        if (this.currentlyLoading == true) return; //Stops the loading manager from loading a scene multiple times at once.
        this.currentlyLoading = true;

        this.targetScene = sceneID; //Sets the target scene and animation transition to the value passed in.
        this.targetAnimationTrans = animationTransID;

        switch (useLoadingScene)
        {
            case false:
                StartCoroutine(LoadGameScene4(this.targetScene)); //Loads the scene directly, doing a fade scene transition.
                break;

            case true:
                StartCoroutine(LoadGameScene4(1)); //Loads scene "1" which is the Loading Scene.
                break;
        }
    }

    private IEnumerator LoadGameScene2(int Scene) //Part 2: LoadGameScene2 is what happens within the loading scene.
    {

        FindAnimator(); //Finds the animator to play the animation, and waits 2 seconds for the animation to finish.
        yield return new WaitForSeconds(2);
        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(Scene); //Loads the Scene without freezing the game, using Async.
        asyncLoading.allowSceneActivation = false; //Stops the game from loading when done.
        
        //Keeps looping the loading until it's done, affects the loading bar.
        while (!asyncLoading.isDone)
        {    
            if (this.loadingBar != null)
            {
                this.loadingBar.value = 0.1f + asyncLoading.progress; //if a loading bar is found, it will put the current load progress into it.
            }
            if (asyncLoading.progress >= 0.9f) //Scenes are loaded at 0.9f, just not activated, which is why this check is here.
            {
                if (this.transition) this.FadeAnim.Play("FadeOut"); //Plays the fade-out animation if transitions is enabled.
                yield return new WaitForSeconds(1.5f); //Waits for the animation to finish.
                asyncLoading.allowSceneActivation = true; //Enables the new scene and deloads the old.
                this.currentlyLoading = false; //Allows the loading manager to load scenes again.
            }
            yield return null;
        }
        yield return null;
    }

    public void LoadGameScene3() //This is used by the Loading Helper to trigger the Coroutine in the loading screen.
    {
        FindAnimator(); //Finds the animator
        if(this.transition) this.FadeAnim.Play("FadeIn"); //Plays fade in animation
        GameObject loadingBarObject = GameObject.Find("Loading Bar"); //Finds the loading bar GameObject in the loading scene
        this.loadingBar = loadingBarObject.GetComponent<Slider>(); //Sets the loading bar to Slider
        this.loadingBar.value = 0;                                 //Resets the default value to 0
        StartCoroutine(LoadGameScene2(this.targetScene));      //Starts the main coroutine.

    }

    public IEnumerator LoadGameScene4(int Scene) //This is used to load a specific scene, with a fade transition. It is used to load scenes without the Loading scene.
    {
        FindAnimator(); //finds the animator
        if (this.transition) this.FadeAnim.Play("FadeOut"); //Plays the animation
        yield return new WaitForSeconds(2); //Waits for the animator to be over
        SceneManager.LoadScene(Scene); //Loads the scene.
        yield return null;
    }

    private void FindAnimator() //Used to find the animator in the scene that does the transitions.
    {
        GameObject Transition = GameObject.FindGameObjectWithTag("Transitions"); //Locates the transition GameObject
        if (Transition != null)
        {
            Transform FadeObject = Transition.transform.Find("Fade"); //Locates and Plays the fade animation
            this.FadeAnim = FadeObject.GetComponent<Animator>();
            this.transition = true;
        }
        else
        {
            this.transition = false; //if it can't be found, it's set to false which is a error/crash prevention.
        }
    }
}
