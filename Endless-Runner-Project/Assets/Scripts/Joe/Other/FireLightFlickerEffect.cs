using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightFlickerEffect : MonoBehaviour
{
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

        this.transform.localPosition += this.moveVelocity * Time.fixedDeltaTime;

        if (Vector3.Distance(this.startingPos, this.transform.localPosition) > this.lightMoveAmount)
        {
            this.transform.localPosition = this.startingPos;
            this.SetRandomLightMoveSpeed();
        }
    }

    private void SetRandomLightMoveSpeed()
    {
        this.moveVelocity = this.lightMoveDirection * Random.Range(1.0f, 5.0f) * this.flickerSpeed;

    }
}
