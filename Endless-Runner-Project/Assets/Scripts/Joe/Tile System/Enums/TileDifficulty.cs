using System;
using UnityEngine;

/* TILE DIFFICULTY ENUM
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Added the 'filler' difficulty
 * VeryEasy is no longer used by the tile sequencer - now removed to avoid incorrect usage.
 */
/// <summary>
/// The difficulty of a tile - will be assignable on the tile scriptable object and used for fair sequencing.
/// </summary>
public enum TileDifficulty
{
    Filler,
    Easy,
    Medium,
    Hard
}
