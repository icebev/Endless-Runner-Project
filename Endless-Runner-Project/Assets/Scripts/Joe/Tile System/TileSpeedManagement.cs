using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/* TILE SPEED MANAGEMENT CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * Properties created to protect frequently accessed and important fields
 */
/// <summary>
/// Handles tile treadmill speed and also calculates and stores the total distance travelled
/// </summary>
public class TileSpeedManagement : MonoBehaviour
{
    // Private variables
    private float _currentTileSpeed;
    private float _externalSpeedMultiplier;
    private float distanceTravelled = 0;

    private TileSpeedIncrementation tileSpeedIncrementation;
    private SprintSystem sprintSystem;

    [Header("Inspector Set References")] 
    [SerializeField] private TextMeshProUGUI distanceTravelledText;
    [SerializeField] private TextMeshProUGUI finalDistanceText;

    #region Properties
    /// <summary>
    /// For altering the overall tile speed outside of the Tile Speed Management class
    /// </summary>
    public float ExternalSpeedMultiplier
    {
        get { return this._externalSpeedMultiplier; }
        set { this._externalSpeedMultiplier = value; }
    }

    /// <summary>
    /// The currently applied speed of the tile treadmill
    /// </summary>
    public float CurrentTileSpeed 
    { 
        get { return this._currentTileSpeed; }
        set { this._currentTileSpeed = value; }
    }

    /// <summary>
    /// Property that indicates whether the tiles are currently being slowed externally
    /// </summary>
    public bool IsNotSlowed
    {
        get
        {
            if (this.ExternalSpeedMultiplier == 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    private void Start()
    {
        this.tileSpeedIncrementation = FindObjectOfType<TileSpeedIncrementation>();
        this.sprintSystem = FindObjectOfType<SprintSystem>();
    }

    private void FixedUpdate()
    {
        // We should only have a tile speed greater than zero if the player is still alive
        if (GameOverEvent.isPlayerDead == false)
        {
            this.CurrentTileSpeed = this.tileSpeedIncrementation.calculatedTargetTileSpeed * this.ExternalSpeedMultiplier;
            
            // We apply the bonus speed for sprinting if applicable
            if (this.sprintSystem.isSprinting)
            {
                this.CurrentTileSpeed += this.sprintSystem.tileSpeedChange;
            }

            // Cumulative addition to total distance travelled 
            this.distanceTravelled += this.CurrentTileSpeed * Time.fixedDeltaTime;
            this.distanceTravelledText.text = Mathf.Round(this.distanceTravelled).ToString() + "m";
        }
        else
        {
            this.CurrentTileSpeed = 0.0f;
            // We need to update the game over screen distance if the player is dead
            this.finalDistanceText.text = "Distance: " + Mathf.Round(this.distanceTravelled).ToString() + "m";
        }
    }

    /// <summary>
    /// Called upon game over or mid run quit to record distance progress and set records
    /// </summary>
    public void RunEndDistanceUpdate()
    {
        // Add to total distance travelled PlayerPref to record stat
        int lifeTimeTotalDistance = PlayerPrefs.GetInt("LifetimeTotalDistance");
        lifeTimeTotalDistance += Mathf.RoundToInt(this.distanceTravelled);
        PlayerPrefs.SetInt("LifetimeTotalDistance", lifeTimeTotalDistance);

        // Overwrite best distance if the current distance is greater than the saved best
        int lifeTimeBestDistance = PlayerPrefs.GetInt("LifetimeBestDistance");
        if (this.distanceTravelled > lifeTimeBestDistance)
        {
            PlayerPrefs.SetInt("LifetimeBestDistance", Mathf.RoundToInt(this.distanceTravelled));
        }
    }

}
