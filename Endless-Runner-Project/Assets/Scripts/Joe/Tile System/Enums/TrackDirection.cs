using System;
using UnityEngine;

/* TRACK DIRECTION ENUM
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// The four orthoganal directions of travel. Used by the Tile Manager to 
/// keep track of the current run direction and direction that tiles are spawning in.
/// </summary>
public enum TrackDirection
{
    positiveZ,
    negativeX,
    negativeZ,
    positiveX
}
