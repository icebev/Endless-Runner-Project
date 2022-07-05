using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.pooledObjects = new List<GameObject>();
        
        GameObject newCoin;

        for (int i = 0; i < this.amountToPool; i++)
        {
            newCoin = Instantiate(this.objectToPool);
            newCoin.transform.parent = this.transform;
            newCoin.SetActive(false);
            this.pooledObjects.Add(newCoin);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < this.amountToPool; i++)
        {
            if (!this.pooledObjects[i].activeInHierarchy)
            {
                return this.pooledObjects[i];
            }
        }
        return null;
    }
}
