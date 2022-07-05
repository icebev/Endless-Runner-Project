using System;
using UnityEngine;

/* TILE DIFFICULTY ENUM
 * Author(s): Joe Bevis
 * Date last modified: 03/07/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Added the 'filler' difficulty
 * 
 */
/// <summary>
/// The difficulty of a tile - will be assignable on the tile scriptable object and used for fair sequencing
/// </summary>
public enum TileDifficulty
{
    Filler,
    VeryEasy,
    Easy,
    Medium,
    Hard
}
