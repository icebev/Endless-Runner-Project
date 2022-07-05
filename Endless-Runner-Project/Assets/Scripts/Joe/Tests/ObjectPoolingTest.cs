using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingTest : MonoBehaviour
{
    public static ObjectPoolingTest SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public int simultaneousCoinLimit;
    public int activeCoinCount;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.pooledObjects = new List<GameObject>();
        GameObject tmp;

        for (int i = 0; i < this.amountToPool; i++)
        {
            tmp = Instantiate(this.objectToPool);
            tmp.SetActive(false);
            this.pooledObjects.Add(tmp);
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

    // Update is called once per frame
    void Update()
    {
        GameObject coin = ObjectPoolingTest.SharedInstance.GetPooledObject();
        if (coin != null && this.activeCoinCount < this.simultaneousCoinLimit)
        {
            coin.transform.position = this.transform.position;
            coin.SetActive(true);
            this.activeCoinCount++;
        }
    }
}
