using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* MOVING WALL BACKGROUND CLASS 
 * Author(s): Joe Bevis
 * Date last modified: 09/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Now adds in the x direction when the wall goes past the min X value instead of setting the value 
 * so that no gaps are made due to the quantized nature of movement in the update loop.  
 */
/// <summary>
/// A class for moving the wall pieces in the background of the main menu scene to create an endless animation effect.
/// </summary>

public class MovingWallBackground : MonoBehaviour
{
    [SerializeField] private float minimumXValue;
    [SerializeField] private float startingXValue;
    [SerializeField] private float wallMoveSpeed;

    private void Update()
    {
        // We move each of the child objects - these will be the wall pieces.
        foreach (Transform child in this.transform)
        {
            Vector3 displacementVector = new Vector3(-this.wallMoveSpeed * Time.deltaTime, 0, 0);
            child.position += displacementVector;

            if (child.position.x <= this.minimumXValue)
            {
                // Calculate how much in the x direction the object must be translated to reach the required starting position
                // now that it has gone past the minimum x value.
                float moveToStartDistance = this.startingXValue - this.minimumXValue;
                child.position += new Vector3(moveToStartDistance, 0, 0);
            }
        }
    }

}
