using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* DELAYED FACE APPEAR CLASS
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * 
 */

/// <summary>
/// Ensures that the evil fog face will only appear once the black fog background is covering most of the screen.
/// Without this, the face will sometimes appear out of thin air upon game over which looks strange.
/// </summary>
public class DelayedFaceAppear : MonoBehaviour
{
    private const int MINYPOSVALUE = -1400;
    [SerializeField] private Transform GUIFogTransform;
    [SerializeField] private GameObject FogFace;

    // Update is called once per frame
    void Update()
    {
        // Only set the face as active if it is not already active
        if (this.FogFace.activeInHierarchy == false && this.GUIFogTransform.localPosition.y > MINYPOSVALUE)
        {
            this.FogFace.SetActive(true);
        }
    }
}
