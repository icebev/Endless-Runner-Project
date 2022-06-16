using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpeedIncrementation : MonoBehaviour
{
    public SpeedIncrementMode incrementMode;
    public float currentTileSpeed;
    public bool useSpeedLimit = false;
    public float speedLimit;

    private float targetSpeed;

    [Tooltip("The starting speed of tile movement opposing the run direction.")]
    [SerializeField] private float startingTileSpeed = 6f;
    
    // Linear fields
    [Tooltip("Amount by which the speed increases over 10 seconds of gameplay.")]
    [SerializeField] private float linearIncrementFactor = 0f;

    // Interval 
    [Tooltip("Time between each speed increase or target speed increase.")]
    [SerializeField] private float intervalTime = 0f;
    [Tooltip("Amount by which the speed or target speed increases after the interval.")]
    [SerializeField] private float intervalIncreaseFactor = 0f;

    private float timeUntilInterval = 0.0f;

    private void Start()
    {
        this.targetSpeed = this.startingTileSpeed;
        this.timeUntilInterval = this.intervalTime;
        this.currentTileSpeed = this.startingTileSpeed;
    }

    private void FixedUpdate()
    {
        if (this.currentTileSpeed < this.speedLimit || this.useSpeedLimit == false)
        {
            switch (this.incrementMode)
            {
                case SpeedIncrementMode.linear:
                    {
                        this.currentTileSpeed += Time.fixedDeltaTime * 0.1f * this.linearIncrementFactor;
                        break;
                    }
                case SpeedIncrementMode.intervals:
                    {
                        if (this.timeUntilInterval <= 0.0f)
                        {
                            this.timeUntilInterval = this.intervalTime;
                            this.currentTileSpeed += this.intervalIncreaseFactor;
                        }
                        this.timeUntilInterval -= Time.fixedDeltaTime;
                        break;
                    }
                case SpeedIncrementMode.linearMidInterval:
                    {
                        if (this.timeUntilInterval <= 0.0f)
                        {
                            this.timeUntilInterval = this.intervalTime;
                            this.targetSpeed += this.intervalIncreaseFactor;
                        }
                        this.timeUntilInterval -= Time.fixedDeltaTime;

                        if (this.currentTileSpeed <= this.targetSpeed)
                        {
                            this.currentTileSpeed += Time.fixedDeltaTime * 0.1f * this.linearIncrementFactor;
                        }
                        break;
                    }
            }
        }
        else if (this.currentTileSpeed > this.speedLimit)
        {
            this.currentTileSpeed = this.speedLimit;
        }
    }
}
