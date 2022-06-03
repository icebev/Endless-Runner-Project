using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileManager : MonoBehaviour
{
    public GameObject tilePrefab;

    public int tileSpawnCount;
    public float squareTileDimension;

    private Transform finalTile;

    public UnityEvent createNewTile;

    private float MaxTileDistance
    {
        get { return this.tileSpawnCount * this.squareTileDimension; }
    }

    private void Start()
    {
        for (int z = this.tileSpawnCount; z >= 0; z--)
        {
            Vector3 tilePos = this.tilePrefab.transform.position + new Vector3(0, 0, this.MaxTileDistance - (z * this.squareTileDimension));
            GameObject newTile = Instantiate(this.tilePrefab, tilePos, this.tilePrefab.transform.rotation);
            
            if (z == this.tileSpawnCount)
            {
                this.finalTile = newTile.transform;
                print(this.finalTile.position+"Final1");
            }
        }
    }

    public void SpawnAdditionalTile()
    {
        Vector3 newSpawnPos = this.finalTile.position + new Vector3(0, 0, this.squareTileDimension);
        GameObject newTile = Instantiate(this.tilePrefab, newSpawnPos, this.tilePrefab.transform.rotation);
        print(this.finalTile.position + "Final2");
        this.finalTile = newTile.transform;
        print(this.finalTile.position + "Final3");
    }

}
