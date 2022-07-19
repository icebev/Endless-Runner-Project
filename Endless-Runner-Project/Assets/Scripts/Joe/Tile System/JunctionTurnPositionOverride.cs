using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionTurnPositionOverride : MonoBehaviour
{
    public GameObject playerGameobject;
    public Transform leftTurnPoint;
    public Transform rightTurnPoint;
    public bool leftTurning;
    public bool rightTurning;
    public float turnTime;

    private CharacterManager characterManager;
    public CinemachineVirtualCamera closeCam;
    public CinemachineVirtualCamera normalCam;


    public void ActivateLeftTurn()
    {
        this.leftTurning = true;
        StartCoroutine(DelayedTurnToggleOff("Left", this.turnTime));
    }
    public void ActivateRightTurn()
    {
        this.rightTurning = true;
        StartCoroutine(DelayedTurnToggleOff("Right", this.turnTime));
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
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.closeCam.Priority = 20;
            this.characterManager.LockLaneSwitching(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.closeCam.Priority = 0;
            this.characterManager.LockLaneSwitching(false);

        }
    }
}
