using System;
using UnityEngine;

/* COLLISION CONSEQUENCE TYPE ENUM
 * Author(s): Joe Bevis
 * Date last modified: 13/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * 
 * 
 */
/// <summary>
/// The type of obstacle collision consequence which is used by ObstacleCollisionConsequences 
/// to determine how much the player is slowed down and caught up to by the chaser.
/// </summary>
public enum CollisionConsequenceType
{
    RegularTrip,
    SprintingTrip,
    LethalCollision
}