using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckRaycastExperiment : MonoBehaviour
{
    Vector3 raycastOrigin = new Vector3(0, 0.5f, 0);
    public float raycastDistance = 0.5f;
    public float transformChangeRate = 0.5f;

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
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(this.raycastOrigin, Vector3.down, out hit, this.raycastDistance))
        {
            Debug.DrawRay(this.raycastOrigin, Vector3.down * hit.distance, Color.yellow);
            this.playerRigidbody.velocity = new Vector3();
            this.playerRigidbody.useGravity = false;
            this.transform.position += new Vector3(0, this.transformChangeRate, 0);
            Debug.Log("Did Hit");
        }
        else if (!Physics.Raycast(this.raycastOrigin, Vector3.down, out lowerHit, this.raycastDistance * 2))
        {
            Debug.DrawRay(this.raycastOrigin, Vector3.down * lowerHit.distance, Color.blue);
            
            if (this.playerRigidbody.useGravity == false)
            {
                this.playerRigidbody.useGravity = true;
            }
            Debug.Log("Did not Hit");
        }
    }
}

