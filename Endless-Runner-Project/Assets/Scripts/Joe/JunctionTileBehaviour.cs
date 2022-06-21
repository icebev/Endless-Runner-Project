using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionTileBehaviour : MonoBehaviour
{

    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;
    private float turnDist = 0.1f;

    [SerializeField] private bool hasLeftTurn;
    [SerializeField] private bool hasRightTurn;
    [SerializeField] private bool hasCenterCorridor;

    [System.Obsolete]

    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.characterManager = FindObjectOfType<CharacterManager>();
    }
    private void FixedUpdate()
    {
        float distanceFromOrigin = Vector3.Distance(this.transform.position, new Vector3(0, 4.5f, 0));
        if (distanceFromOrigin < this.turnDist)
        {
            if (this.hasRotated == false && this.characterManager.GetPlayerLaneTarget() == 2 && this.hasRightTurn)
            {
                this.characterManager.Rotate(TurnDirection.Right);
                this.tileManager.TrackSpawnRightTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;


                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, 90);
                    tileObject.GetComponent<TileMovement>().CorrectOffset();
                }
                //foreach (Transform child in this.tileManager.tilesContainer.transform)
                //{
                //    child.GetComponent<TileMovement>().CorrectOffset();
                //}

            }
            else if (this.hasRotated == false && this.characterManager.GetPlayerLaneTarget() == -2 && this.hasLeftTurn)
            {
                this.characterManager.Rotate(TurnDirection.Left);
                this.tileManager.TrackSpawnLeftTurn();
                this.tileManager.runDirection = this.tileManager.spawnDirection;


                foreach (GameObject tileObject in this.tileManager.junctionSpawnedTilesList)
                {
                    tileObject.transform.RotateAround(this.transform.position, Vector3.up, -90);
                    tileObject.GetComponent<TileMovement>().CorrectOffset();

                }
            }

            this.tileManager.spawningAfterJunction = false;
            this.characterManager.SetLanePos(0);
            this.hasRotated = true;
            this.tileManager.junctionSpawnedTilesList.Clear();
            this.tileManager.currentJunctionTile = null;
        }
    }
}
