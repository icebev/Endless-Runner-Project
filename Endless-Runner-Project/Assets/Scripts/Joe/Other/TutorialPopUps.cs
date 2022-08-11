using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialPopUps : MonoBehaviour
{

    private PlayerControls tutorialInputActions;
    private InputAction jump;
    private InputAction slide;
    private InputAction moveLeft;
    private InputAction moveRight;
    private InputAction sprint;

    [SerializeField] private GameObject[] popUps;
    [SerializeField] private AudioSource popUpSound;
    [SerializeField] private AudioClip[] popUpSoundClips;
    [SerializeField] private int currentPopUpNumber;
    [SerializeField] private PauseScipt pauseScript;

    [SerializeField] private float timeUntilFirstPause;
    [SerializeField] private float timeBetweenPopups;

    private bool waitingForNextPopup;
    private bool popUpActive;
    private float timeUntilNextPopUp;

    // Start is called before the first frame update
    void Start()
    {
        this.tutorialInputActions = new PlayerControls();
        this.jump = this.tutorialInputActions.PlayerCharacter.Jump;
        this.slide = this.tutorialInputActions.PlayerCharacter.Slide;
        this.moveLeft = this.tutorialInputActions.PlayerCharacter.MoveLeft;
        this.moveRight = this.tutorialInputActions.PlayerCharacter.MoveRight;
        this.sprint = this.tutorialInputActions.PlayerCharacter.Sprint;

        if (PlayerPrefs.GetInt("TutorialComplete") == 1)
        {
            this.currentPopUpNumber = 4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        this.timeUntilFirstPause -= Time.deltaTime;
        this.timeUntilNextPopUp -= Time.deltaTime;

        switch (this.currentPopUpNumber)
        {
            case 0:
                {
                    if (this.timeUntilFirstPause <= 0.0f && this.popUpActive == false)
                    {
                        this.DisplayPopUp(this.currentPopUpNumber);
                        this.jump.Enable();

                    }

                    if (this.jump.IsPressed() && this.popUpActive == true)
                    {
                        this.HidePopUp(this.currentPopUpNumber);
                    }
                    break;
                }
            case 1:
                {
                    if (this.timeUntilNextPopUp <= 0.0f && this.waitingForNextPopup == true)
                    {
                        this.DisplayPopUp(this.currentPopUpNumber);
                        this.moveLeft.Enable();
                        this.moveRight.Enable();
                    }

                    if (this.moveLeft.IsPressed() || this.moveRight.IsPressed())
                    {
                        this.HidePopUp(this.currentPopUpNumber);
                    }
                    break;
                }
            case 2:
                {
                    if (this.timeUntilNextPopUp <= 0.0f && this.waitingForNextPopup == true)
                    {
                        this.DisplayPopUp(this.currentPopUpNumber);
                        this.slide.Enable();
                    }

                    if (this.slide.IsPressed())
                    {
                        this.HidePopUp(this.currentPopUpNumber);
                    }
                    break;
                }
            case 3:
                {
                    if (this.timeUntilNextPopUp <= 0.0f && this.waitingForNextPopup == true)
                    {
                        this.DisplayPopUp(this.currentPopUpNumber);
                        this.sprint.Enable();
                    }

                    if (this.sprint.IsPressed())
                    {
                        this.HidePopUp(this.currentPopUpNumber);
                        PlayerPrefs.SetInt("TutorialComplete", 1);
                    }
                    break;
                }
            case 4:
                this.SelfDestruct();
                break;
        }
    }

    private void DisplayPopUp(int popUpNumber)
    {
        if (popUpNumber % 2 == 0)
        {
            this.popUpSound.PlayOneShot(this.popUpSoundClips[0]);
        }
        else
        {
            this.popUpSound.PlayOneShot(this.popUpSoundClips[1]);
        }
        this.popUps[popUpNumber].SetActive(true);
        this.pauseScript.PauseGame();
        this.waitingForNextPopup = false;
        this.popUpActive = true;
    }

    private void HidePopUp(int popUpNumber)
    {
        this.pauseScript.UnpauseGame();
        this.popUps[popUpNumber].SetActive(false);
        this.currentPopUpNumber++;
        this.timeUntilNextPopUp = this.timeBetweenPopups;
        this.waitingForNextPopup = true;
        this.popUpActive = false;
    }

    private void SelfDestruct()
    {
        this.jump.Disable();
        this.slide.Disable();
        this.moveLeft.Disable();
        this.moveRight.Disable();
        this.sprint.Disable();
        Destroy(this.gameObject);
    }
}
