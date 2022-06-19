using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CornerTileBehaviour : MonoBehaviour
{
    public TurnDirection turnDirection;
    private TileManager tileManager;
    private CharacterManager characterManager;
    private bool hasRotated = false;
    private float turnDist = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.characterManager = FindObjectOfType<CharacterManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromOrigin = Vector3.Distance(this.transform.position, new Vector3(0, 4.5f, 0));
        if (distanceFromOrigin < this.turnDist)
        {
            if (this.hasRotated == false)
            {
                this.characterManager.Rotate(this.turnDirection);
                this.characterManager.SetLanePos(0);

                this.hasRotated = true;
                this.tileManager.runDirection = this.tileManager.spawnDirection;
                foreach (Transform child in this.tileManager.tilesContainer.transform)
                {
                    child.GetComponent<TileMovement>().CorrectOffset();
                }
            }
        }
    }
}
