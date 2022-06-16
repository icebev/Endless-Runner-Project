using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CornerTileBehaviour : MonoBehaviour
{
    public TurnDirection turnDirection;
    private TileManager tileManager;
    private bool hasRotated = false;
    private float turnDist = 0.1f;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromOrigin = Vector3.Distance(this.transform.position, new Vector3(0, -0.25f + player.transform.position.y, 0));
        if (distanceFromOrigin < this.turnDist)
        {
            if (this.hasRotated == false)
            {
                if (this.turnDirection == TurnDirection.Left)
                {
                    this.player.transform.eulerAngles = new Vector3(
                        this.player.transform.eulerAngles.x,
                        this.player.transform.eulerAngles.y - 90,
                        this.player.transform.eulerAngles.z
                    );

                }
                else if (this.turnDirection == TurnDirection.Right)
                {
                    this.player.transform.eulerAngles = new Vector3(
                        this.player.transform.eulerAngles.x,
                        this.player.transform.eulerAngles.y + 90,
                        this.player.transform.eulerAngles.z
                    );
                }
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
