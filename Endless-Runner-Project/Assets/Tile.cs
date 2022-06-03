using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float despawnZPosition;
    private TileManager tileManager;
    public float moveSpeed;

    private void Start()
    {
        this.tileManager = FindObjectOfType<TileManager>();

    }

    /*
    private void Update()
    {
        this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed * Time.deltaTime);
        if (this.transform.position.z < -10)
        {
            this.tileManager.SpawnAdditionalTile();
            Destroy(this.gameObject);
        }
        
    }
    */

    private void FixedUpdate()
    {
        if (this.transform.position.z < -10)
        {
            this.tileManager.SpawnAdditionalTile();
            Destroy(this.gameObject);
        }
        this.transform.position = this.transform.position - new Vector3(0, 0, this.moveSpeed/25);
        
    }

}
