using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CAMERA DUCK TRIGGER CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * This script was never needed in the final build of the project,
 * it has been left in as evidence of the concept being considered.
 */
/// <summary>
/// A class that enables the camera to go low when the player is going under obstacles
/// to avoid camera clipping
/// </summary>
public class CameraDuckTrigger : MonoBehaviour
{
    private GameObject cam;
    private Animator duckAnimator;

    private void Start()
    {
        this.cam = GameObject.FindGameObjectWithTag("CMCam");
        this.duckAnimator = this.cam.GetComponent<Animator>();
    }

    // Make the camera go low while inside this trigger volume - attached to a tile.
    private void OnTriggerStay(Collider other)
    {
        if (this.duckAnimator.GetBool("CameraDuck") == false )
        {
            this.duckAnimator.SetBool("CameraDuck", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        this.duckAnimator.SetBool("CameraDuck", false);
    }
}
