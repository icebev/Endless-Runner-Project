using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class SprintSystem : MonoBehaviour
{
    public Animator playerAnimator;
    public CinemachineVirtualCamera playerCamera;
    public TileSpeedIncrementation tileSpeedIncrementation;
    public Transform cameraHolder;
    
    public float camZNormal;
    public float camZSprinting;
    public float camZTarget;

    public float fovNormal;
    public float fovSprinting;
    public float fovTarget;

    public float tileSpeedChange;
    public float inerpolationSpeed;


    private void Start()
    {
        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.fovTarget = this.fovNormal;
        this.camZTarget = this.camZNormal;
    }


    public void ProcessSprintInput(InputAction.CallbackContext press)
    {
        if (press.performed)
        {
            this.StartSprinting();
        }
        if (press.canceled)
        {
            this.StopSprinting();
        }
    }
    private void FixedUpdate()
    {
        // Camera fov lerp
        float currentFov = this.playerCamera.m_Lens.FieldOfView;
        currentFov = Mathf.Lerp(currentFov, this.fovTarget, this.inerpolationSpeed);
        this.playerCamera.m_Lens.FieldOfView = currentFov;

        // Camera folder position change
        float currentZPos = this.cameraHolder.localPosition.z;
        currentZPos = Mathf.Lerp(currentZPos, this.camZTarget, this.inerpolationSpeed);
        this.cameraHolder.transform.localPosition = new Vector3(0, 0, currentZPos);
    }

    public void StartSprinting()
    {
        this.tileSpeedIncrementation.currentTileSpeed += 2.0f;
        this.tileSpeedIncrementation.speedLimit += 2.0f;
        this.playerAnimator.Play("FastRun");
        this.fovTarget = this.fovSprinting;
        this.camZTarget = this.camZSprinting;

    }

    public void StopSprinting()
    {
        this.tileSpeedIncrementation.currentTileSpeed -= this.tileSpeedChange;
        this.tileSpeedIncrementation.speedLimit -= this.tileSpeedChange;
        this.playerAnimator.Play("Running");
        this.fovTarget = this.fovNormal;
        this.camZTarget = this.camZNormal;

    }
}
