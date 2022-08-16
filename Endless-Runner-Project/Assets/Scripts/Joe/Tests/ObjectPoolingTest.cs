using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OBJECT POOLING TEST CLASS 
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// Used while learning object pooling in a separate scene to ensure 
/// that the technique was understood properly before implementing it fully.
/// </summary>
public class ObjectPoolingTest : MonoBehaviour
{
    /* RESEARCH SOURCE REFERENCE
     * Title: Introduction to Object Pooling
     * Author: Unity Technologies
     * Date Published: 18/05/2022
     * Date Accessed: 04/07/2022
     * URL: https://learn.unity.com/tutorial/introduction-to-object-pooling
     * Usage: Learned how to implement basic object pooling using this tutorial.
     */

    public static ObjectPoolingTest SharedInstance;
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private int simultaneousCoinLimit;
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

        // Pool instantiation
        for (int i = 0; i < this.amountToPool; i++)
        {
            tmp = Instantiate(this.objectToPool);
            tmp.SetActive(false);
            this.pooledObjects.Add(tmp);
        }
    }

    // Pooled object retrieval
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
        // Testing the object pool by taking coin GameObjects from it
        // and then making them move with CoinMovementTest 
        GameObject coin = ObjectPoolingTest.SharedInstance.GetPooledObject();
        if (coin != null && this.activeCoinCount < this.simultaneousCoinLimit)
        {
            coin.transform.position = this.transform.position;
            coin.SetActive(true);
            this.activeCoinCount++;
        }
    }
}
