using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* COIN POOL CLASS
 * Author(s): Joe Bevis
 * Date last modified: 15/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 * 
 */
/// <summary>
/// Object pooling for the coin collectibles - used to reduce
/// constant creating and destroying of the coin prefab to improve perfomance.
/// </summary>

public class CoinPool : MonoBehaviour
{
    /* RESEARCH SOURCE REFERENCE
     * Title: Introduction to Object Pooling
     * Author: Unity Technologies
     * Date Published: 18/05/2022
     * Date Accessed: 04/07/2022
     * URL: https://learn.unity.com/tutorial/introduction-to-object-pooling
     * Usage: Learned how to implement basic object pooling using this tutorial.
     */

    // Public static reference for easy access
    public static CoinPool SharedInstance;

    [Header("Pool Config")]
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.pooledObjects = new List<GameObject>();
        
        GameObject newCoin;

        // Generate a pool of objects on start equal to the amount to pool and set them inactive
        for (int i = 0; i < this.amountToPool; i++)
        {
            newCoin = Instantiate(this.objectToPool);
            newCoin.transform.parent = this.transform;
            newCoin.SetActive(false);
            this.pooledObjects.Add(newCoin);
        }
    }

    /// <summary>
    /// Retrieve a pooled coin that is ready to be used (is inactive) from the pool
    /// </summary>
    /// <returns>A pooled coin object</returns>
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < this.amountToPool; i++)
        {
            if (this.pooledObjects[i].activeInHierarchy == false)
            {
                return this.pooledObjects[i];
            }
        }
        return null;
    }
}
