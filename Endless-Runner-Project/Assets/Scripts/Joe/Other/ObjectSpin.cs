using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OBJECT SPIN CLASS 
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// Spins the GameObject by a set speed around a set axis over time
/// </summary>
public class ObjectSpin : MonoBehaviour
{
    [Tooltip("How fast the object will spin. Can be negative.")]
    [SerializeField] private float spinSpeed;
    [Tooltip("Axis around which the object will spin.")]
    [SerializeField] private Vector3 spinAxis;

    void FixedUpdate()
    {
        this.transform.Rotate(this.spinAxis * this.spinSpeed * Time.fixedDeltaTime, Space.World);
    }
}
