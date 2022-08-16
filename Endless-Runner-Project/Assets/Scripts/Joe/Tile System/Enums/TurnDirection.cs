using System;
using UnityEngine;

/* TURN DIRECTION ENUM
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// The direction that the player turns on the tile. 
/// Used by corner tiles to determine the outcome of taking the corner.
/// </summary>
public enum TurnDirection
{
    Left,
    Right
}
