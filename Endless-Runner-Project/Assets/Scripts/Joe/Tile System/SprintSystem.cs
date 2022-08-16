using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

/* SPRINT SYSTEM CLASS
 * Author(s): Joe Bevis & Kris Burgess-James
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Fixed variable name typo - inerpolationSpeed to interpolationSpeed.
 * Commenting pass
 */

/// <summary>
/// Contains functionality for smooth transitioning between regular run and sprinting player states.
/// </summary>
public class SprintSystem : MonoBehaviour
{

    [Header("Inspector Set References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private Transform cameraHolder;

    
    [Header("Inpsector Config - Camera movement")]
    [SerializeField] private float camZNormal;
    [SerializeField] private float camZSprinting;
    [SerializeField] private float camZTarget;

    [Header("Inpsector Config - Field of view change")]
    [SerializeField] private float fovNormal;
    [SerializeField] private float fovSprinting;
    [SerializeField] private float fovSpeedboost;
    [SerializeField] private float fovTarget;

    [Header("Inpsector Config - Run animation speed")]
    [SerializeField] private float runAnimSpeedNormal;
    [SerializeField] private float runAnimSpeedSprinting;
    [SerializeField] private float runAnimSpeedCurrent;
    [SerializeField] private float runAnimSpeedTarget;
    [SerializeField] private float interpolationSpeed;

    [Header("Other")]
    // Public variables
    public float tileSpeedChange;
    public bool speedBoostModeActive;
    public bool isSprinting;

    // Private variables
    private TileSpeedIncrementation tileSpeedIncrementation;
    private TileSpeedManagement tileSpeedManagement;
    private PlayerControls inputActionsForSprint;
    private InputAction sprintAction;

    private void Start()
    {
        this.inputActionsForSprint = new PlayerControls();
        this.sprintAction = this.inputActionsForSprint.PlayerCharacter.Sprint;
        this.sprintAction.Enable();

        // To make sure values exist we set them in code if they haven't been configured
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

        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.tileSpeedManagement = FindObjectOfType<TileSpeedManagement>();
        this.fovTarget = this.fovNormal;
        this.camZTarget = this.camZNormal;
        this.runAnimSpeedTarget = this.runAnimSpeedNormal;
        this.runAnimSpeedCurrent = this.runAnimSpeedNormal;
    }


    /// <summary>
    /// Process the input when a sprint input button is pressed or released to start or stop sprinting
    /// </summary>
    /// <param name="press"></param>
    public void ProcessSprintInput(InputAction.CallbackContext press)
    {
        // We only allow the player to start sprinting if they aren't slowed down externally
        // because it's too easy to recover from a collision otherwise
        if (press.performed && this.tileSpeedManagement.IsNotSlowed == true)
        {
            this.StartSprinting();
        }
        
        if (press.canceled)
        {
            this.StopSprinting();
        }
    }

    /// <summary>
    /// Allows the sprint button to be held down during a collision
    /// so that the player starts sprinting as soon as they are able to
    /// </summary>
    public void HoldingSprintButtonUpdate()
    {
        if (this.isSprinting == false && this.sprintAction.IsPressed() == true && this.tileSpeedManagement.IsNotSlowed == true)
        {
            this.StartSprinting();
        }
    }

    private void FixedUpdate()
    {
        this.HoldingSprintButtonUpdate();

        // Stop sprinting if the player is being slowed after a collision
        if (this.tileSpeedManagement.IsNotSlowed == false && this.isSprinting)
        {
            this.StopSprinting();
        }

        // Speed boost powerup override - sprint input is no longer relevant if the speed boost is taking place
        if (this.speedBoostModeActive)
        {
            if(this.tileSpeedChange != 5)
            {
                // We must start and stop sprinting to ensure that the correct value is used for the speed increas
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

        // Camera holder position change
        float currentZPos = this.cameraHolder.localPosition.z;
        currentZPos = Mathf.Lerp(currentZPos, this.camZTarget, this.interpolationSpeed);
        this.cameraHolder.transform.localPosition = new Vector3(0, 0, currentZPos);

        // Run animation speed lerp
        if (this.runAnimSpeedCurrent != this.runAnimSpeedTarget)
        {
            if (Mathf.Abs(this.runAnimSpeedCurrent - this.runAnimSpeedTarget) < 0.01f)
            {
                this.runAnimSpeedCurrent = this.runAnimSpeedTarget;
            }
            this.runAnimSpeedCurrent = Mathf.Lerp(this.runAnimSpeedCurrent, this.runAnimSpeedTarget, this.interpolationSpeed);
            // The animator will use the RunSpeed float to determine the speed of the run animation
            this.playerAnimator.SetFloat("RunSpeed", this.runAnimSpeedCurrent);
        }
    }

    /// <summary>
    /// Start the player sprinting and set the lerp targets
    /// </summary>
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

    /// <summary>
    /// Stop the player sprinting and set the lerp targets
    /// </summary>
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
