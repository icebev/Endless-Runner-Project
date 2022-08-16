using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GROUND CHECK RAYCAST EXPERIMENT CLASS 
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// Used for testing the validity of using RayCasts for the player character collision detection.
/// It was useful for seeing the pros and cons of the system, and to support my fellow programmer.
/// </summary>
public class GroundCheckRaycastExperiment : MonoBehaviour
{
    Vector3 raycastOrigin = new Vector3(0, 0.5f, 0);
    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private float transformChangeRate = 0.5f;

    private Rigidbody playerRigidbody;

    private void Start()
    {
        this.playerRigidbody = this.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        this.raycastOrigin = this.transform.position + new Vector3(0, 0.5f, 0);
        RaycastHit hit;
        RaycastHit lowerHit;
        // Does the ray intersect any objects excluding the player layer - if so, then the player must be on the ground
        if (Physics.Raycast(this.raycastOrigin, Vector3.down, out hit, this.raycastDistance))
        {
            Debug.DrawRay(this.raycastOrigin, Vector3.down * hit.distance, Color.yellow);

            // We don't want the player to fall through the ground so we set the velocity to zero and disable gravity
            this.playerRigidbody.velocity = new Vector3();
            this.playerRigidbody.useGravity = false;

            // Pushes the player up while the raycast is hitting the ground, so that the player can go up ramps.
            this.transform.position += new Vector3(0, this.transformChangeRate, 0);
            //Debug.Log("Did Hit");
        }
        // A second longer raycast going down was required for checking if the player is on or near the ground or not, 
        // without this the player would rapidly flicker up and down because of the transform change 
        else if (!Physics.Raycast(this.raycastOrigin, Vector3.down, out lowerHit, this.raycastDistance * 2))
        {
            Debug.DrawRay(this.raycastOrigin, Vector3.down * lowerHit.distance, Color.blue);
            
            // If the player is far enough above the ground so that the longer RayCast also doesn't reach the ground, we must enable gravity again
            if (this.playerRigidbody.useGravity == false)
            {
                this.playerRigidbody.useGravity = true;
            }
            //Debug.Log("Did not Hit");
        }
    }
}

