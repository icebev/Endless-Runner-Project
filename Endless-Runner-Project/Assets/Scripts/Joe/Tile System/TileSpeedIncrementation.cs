using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpeedIncrementation : MonoBehaviour
{
    [Header("Tile Speed Incrementation Configuration")]
    [Tooltip("The mode of speed incrementation.")]
    public SpeedIncrementMode incrementMode;

    // Speed limit fields
    [Tooltip("Should there be a limit to the tile movement speed?")]
    public bool useSpeedLimit = false;

    [Tooltip("The maximum speed the tiles can move towards the player excluding sprint bonus.")]
    public float speedLimit;

    [Tooltip("The starting speed of tile movement opposing the run direction.")]
    [SerializeField] private float startingTileSpeed = 6f;
    
    // Linear mode fields
    [Tooltip("Amount by which the speed increases over 10 seconds of gameplay.")]
    [SerializeField] private float linearIncrementFactor = 0f;

    // Interval mode fields
    [Tooltip("Time between each speed increase or target speed increase.")]
    [SerializeField] private float intervalTime = 0f;
    [Tooltip("Amount by which the speed or target speed increases after the interval.")]
    [SerializeField] private float intervalIncreaseFactor = 0f;

    // Private fields
    [SerializeField] private float _calculatedTargetTileSpeed;
    private float timeUntilInterval = 0.0f;
    private float intervalTargetSpeed;

    // Property used to protect the calculated target tile speed when accessing it in other scripts
    /// <summary>
    /// The absolute value of the tile speed, incremented within FixedUpdate with no speed modifiers applied
    /// </summary>
    public float CalculatedTargetTileSpeed
    {
        get { return this._calculatedTargetTileSpeed; }
        private set { this._calculatedTargetTileSpeed = value; }
    }

    private void Start()
    {
        this.intervalTargetSpeed = this.startingTileSpeed;
        this.timeUntilInterval = this.intervalTime;
        this.CalculatedTargetTileSpeed = this.startingTileSpeed;
    }

    private void FixedUpdate()
    {
        this.IncrementTargetTileSpeed();
    }

    /// <summary>
    /// Increments the target tile speed based on the configuration of the incrementation system
    /// </summary>
    private void IncrementTargetTileSpeed()
{
        if (this.CalculatedTargetTileSpeed < this.speedLimit || this.useSpeedLimit == false)
        {
            switch (this.incrementMode)
            {
                // Linear mode - constant incrementation over time by a set rate
                case SpeedIncrementMode.linear:
                    {
                        this.CalculatedTargetTileSpeed += Time.fixedDeltaTime * 0.1f * this.linearIncrementFactor;
                        break;
                    }
                // Intervals mode - at set intervals the players speed increases by a set increase amount
                case SpeedIncrementMode.intervals:
                    {
                        this.timeUntilInterval -= Time.fixedDeltaTime;
                        if (this.timeUntilInterval <= 0.0f)
                        {
                            this.timeUntilInterval += this.intervalTime;
                            this.CalculatedTargetTileSpeed += this.intervalIncreaseFactor;
                        }
                        break;
                    }
                // Linear Mid Interval mode - constant incrementation up to a variable cap which is increased at intervals
                case SpeedIncrementMode.linearMidInterval:
                    {
                        this.timeUntilInterval -= Time.fixedDeltaTime;
                        if (this.timeUntilInterval <= 0.0f)
                        {
                            this.timeUntilInterval += this.intervalTime;
                            this.intervalTargetSpeed += this.intervalIncreaseFactor;
                        }

                        // Linear incrementation up to the cap of intervalTargetSpeed
                        if (this.CalculatedTargetTileSpeed <= this.intervalTargetSpeed)
                        {
                            this.CalculatedTargetTileSpeed += Time.fixedDeltaTime * 0.1f * this.linearIncrementFactor;
                        }
                        break;
                    }
            }
        }
        // If the current Calculated Target Tile speed exceeds the set speed limit,
        // we must set it to the speed limit
        else if (this.CalculatedTargetTileSpeed > this.speedLimit)
        {
            this.CalculatedTargetTileSpeed = this.speedLimit;
        }

    }
}
