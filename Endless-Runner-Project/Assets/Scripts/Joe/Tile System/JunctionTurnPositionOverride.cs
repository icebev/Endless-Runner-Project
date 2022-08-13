using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* JUNCTION TURN POSITION OVERRIDE CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Improved protection level of variables that don't need to be accessed from outside the script
 * 
 */

/// <summary>
/// Defines how the player character should act when they reach a corner of a junction tile.
/// </summary>
public class JunctionTurnPositionOverride : MonoBehaviour
{
    [SerializeField] private GameObject playerGameobject;
    [SerializeField] private Transform leftTurnPoint;
    [SerializeField] private Transform rightTurnPoint;
    [Tooltip("An animation curve to calculate how long the player should be positioned at the turn point based on the tile movement speed.")]
    [SerializeField] private AnimationCurve speedToTurnTimeCurve;
    // The disappearing pieces give the illusion that there have been tiles around the corner the entire time as the player is turning.
    [Tooltip("The pieces that are no longer visible after the turn has taken place.")]
    [SerializeField] private GameObject disappearingPieces;

    private TileSpeedIncrementation tileSpeedIncrementation;
    private CharacterManager characterManager;

    private bool leftTurning;
    private bool rightTurning;

    [SerializeField] private CinemachineVirtualCamera closeCam;
    [SerializeField] private CinemachineVirtualCamera normalCam;

    

    public void ActivateLeftTurn()
    {
        this.leftTurning = true;
        this.disappearingPieces.SetActive(false);
        float turnTime = this.speedToTurnTimeCurve.Evaluate(this.tileSpeedIncrementation.calculatedTargetTileSpeed);
        StartCoroutine(DelayedTurnToggleOff("Left", turnTime));
    }
    public void ActivateRightTurn()
    {
        this.disappearingPieces.SetActive(false);
        this.rightTurning = true;
        float turnTime = this.speedToTurnTimeCurve.Evaluate(this.tileSpeedIncrementation.calculatedTargetTileSpeed);
        StartCoroutine(DelayedTurnToggleOff("Right", turnTime));
    }


    public IEnumerator DelayedTurnToggleOff(string turn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (turn == "Left")
        {
            this.leftTurning = false;
        }

        if (turn == "Right")
        {
            this.rightTurning = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        this.playerGameobject = GameObject.FindGameObjectWithTag("PlayerCharacterContainer");
        this.normalCam = GameObject.Find("NormalCam").GetComponent<CinemachineVirtualCamera>();
        this.closeCam = GameObject.Find("CloseCam").GetComponent<CinemachineVirtualCamera>();
        this.characterManager = FindObjectOfType<CharacterManager>();
        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
    }

    // We use late update to change the player character's position while they are turning
    // to be the exact point on the corner of the turn within the junction corridor,
    // set by the left and right turn point transforms
    private void LateUpdate()
    {
        if (this.leftTurning)
        {
            this.playerGameobject.transform.position = this.leftTurnPoint.position;
        }

        if (this.rightTurning)
        {
            this.playerGameobject.transform.position = this.rightTurnPoint.position;
        }
    }

    // Trigger volume upon entering a junction - switch to the close up camera
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.closeCam.Priority = 20;
            this.characterManager.LockLaneSwitching(true);
        }
    }

    // Upon leaving a junction - switch to the regular camera

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.closeCam.Priority = 0;
            this.characterManager.LockLaneSwitching(false);

        }
    }
}
