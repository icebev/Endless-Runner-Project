using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FIRE LIGHT FLICKER EFFECT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// Moves the light source in a way that makes it seem like the fire is flickering naturally.
/// </summary>
public class FireLightFlickerEffect : MonoBehaviour
{
    private const float RANDOMIZERMIN = 1.0f;
    private const float RANDOMIZERMAX = 5.0f;
    private Vector3 startingPos;
    private Vector3 moveVelocity;

    [SerializeField] private float flickerSpeed;
    [SerializeField] private float lightMoveAmount;
    [SerializeField] private Vector3 lightMoveDirection;

    private void Start()
    {
        this.startingPos = this.transform.localPosition;
        this.SetRandomLightMoveSpeed();
    }

    private void FixedUpdate()
    {
        // Move the light source
        this.transform.localPosition += this.moveVelocity * Time.fixedDeltaTime;

        // Reset back to the start position once the light moves further than the set move amount
        if (Vector3.Distance(this.startingPos, this.transform.localPosition) > this.lightMoveAmount)
        {
            this.transform.localPosition = this.startingPos;
            this.SetRandomLightMoveSpeed();
        }
    }

    /// <summary>
    /// Sets the velocity that the light will move to be a random value based on the flicker speed
    /// </summary>
    private void SetRandomLightMoveSpeed()
    {
        this.moveVelocity = this.lightMoveDirection * Random.Range(RANDOMIZERMIN, RANDOMIZERMAX) * this.flickerSpeed;
    }
}
