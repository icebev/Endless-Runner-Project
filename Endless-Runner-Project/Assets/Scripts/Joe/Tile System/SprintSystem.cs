using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/* SPRINT SYSTEM CLASS
 * Author(s): Joe Bevis & Kris Burgess-James
 * Date last modified: 30/06/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Fixed variable name typo - inerpolationSpeed to interpolationSpeed.
 * 
 */

/// <summary>
/// Contains functionality for smooth transitioning between regular run and sprinting player states.
/// </summary>
public class SprintSystem : MonoBehaviour
{
    public Animator playerAnimator;
    public CinemachineVirtualCamera playerCamera;
    private TileSpeedIncrementation tileSpeedIncrementation;
    public Transform cameraHolder;
    public TileSpeedManagement tileSpeedManagement;
    public bool isSprinting;
    public float sprintCooldown;

    public PlayerControls inputActionsForSprint;
    public InputAction sprintAction;
    
    public float camZNormal;
    public float camZSprinting;
    public float camZTarget;

    public float fovNormal;
    public float fovSprinting;
    public float fovSpeedboost;
    public float fovTarget;

    [SerializeField] private float runAnimSpeedNormal;
    [SerializeField] private float runAnimSpeedSprinting;
    [SerializeField] private float runAnimSpeedCurrent;
    [SerializeField] private float runAnimSpeedTarget;

    public float tileSpeedChange;
    public float interpolationSpeed;

    public bool speedBoostModeActive;


    private void Start()
    {
        // QUICK FIX FOR JOE. (i know this would break in your scene, so I made this to fix itself)
        this.inputActionsForSprint = new PlayerControls();
        this.sprintAction = this.inputActionsForSprint.PlayerCharacter.Sprint;
        this.sprintAction.Enable();

        if(this.runAnimSpeedNormal == 0) 
        { 
            this.runAnimSpeedNormal = 1.5f; 
        }
        if (this.runAnimSpeedSprinting == 0)
        {
            this.runAnimSpeedSprinting = 2.5f;
        }
        if (this.interpolationSpeed == 0)
        {
            this.interpolationSpeed = 0.05f;
        }

        // END OF QUICK FIX

        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.fovTarget = this.fovNormal;
        this.camZTarget = this.camZNormal;
        this.runAnimSpeedTarget = this.runAnimSpeedNormal;
        this.runAnimSpeedCurrent = this.runAnimSpeedNormal;
    }


    public void ProcessSprintInput(InputAction.CallbackContext press)
    {
        if (press.performed && this.tileSpeedManagement.externalSpeedMultiplier == 1)
        {
            this.StartSprinting();
        }
        
        if (press.canceled)
        {
            this.StopSprinting();
        }
    }

    public void HoldingSprintButtonUpdate()
    {
        if (this.isSprinting == false && this.sprintAction.IsPressed() == true && this.tileSpeedManagement.externalSpeedMultiplier == 1)
        {
            this.StartSprinting();
        }
    }

    private void FixedUpdate()
    {
        this.HoldingSprintButtonUpdate();

        if (this.tileSpeedManagement.externalSpeedMultiplier < 1 && this.isSprinting)
        {
            this.StopSprinting();
        }

        if (this.speedBoostModeActive)
        {
            if(this.tileSpeedChange != 5)
            {
                this.StopSprinting();
                this.tileSpeedChange = 5;
                this.StartSprinting();
                
            }

            if (this.isSprinting == false)
            {
                this.StartSprinting();
            }
        }

        // Camera fov lerp
        float currentFov = this.playerCamera.m_Lens.FieldOfView;
        currentFov = Mathf.Lerp(currentFov, this.fovTarget, this.interpolationSpeed);
        this.playerCamera.m_Lens.FieldOfView = currentFov;

        // Camera folder position change
        float currentZPos = this.cameraHolder.localPosition.z;
        currentZPos = Mathf.Lerp(currentZPos, this.camZTarget, this.interpolationSpeed);
        this.cameraHolder.transform.localPosition = new Vector3(0, 0, currentZPos);

        // Run Animation speed lerp
        if (this.runAnimSpeedCurrent != this.runAnimSpeedTarget)
        {
            if (Mathf.Abs(this.runAnimSpeedCurrent - this.runAnimSpeedTarget) < 0.01f)
            {
                this.runAnimSpeedCurrent = this.runAnimSpeedTarget;
            }
            this.runAnimSpeedCurrent = Mathf.Lerp(this.runAnimSpeedCurrent, this.runAnimSpeedTarget, this.interpolationSpeed);
            this.playerAnimator.SetFloat("RunSpeed", this.runAnimSpeedCurrent);
        }
    }

    public void StartSprinting()
    {
        if (this.isSprinting == false)
        {
            this.isSprinting = true;
            this.tileSpeedIncrementation.speedLimit += this.tileSpeedChange;
            this.fovTarget = this.speedBoostModeActive ? this.fovSpeedboost : this.fovSprinting;
            this.camZTarget = this.camZSprinting;
            this.runAnimSpeedTarget = this.runAnimSpeedSprinting;
        }
    }

    public void StopSprinting()
    {

        if (this.isSprinting)
        {
            this.isSprinting = false;
            this.tileSpeedIncrementation.speedLimit -= this.tileSpeedChange;
            this.fovTarget = this.fovNormal;
            this.camZTarget = this.camZNormal;
            this.runAnimSpeedTarget = this.runAnimSpeedNormal;
        }
    }
}
