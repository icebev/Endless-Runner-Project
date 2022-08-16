using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* COIN MOVEMENT TEST CLASS 
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */

/// <summary>
/// Used for testing object pooling for coins by releasing the pooled coin 
/// once it reaches a certain z position threshold
/// </summary>
public class CoinMovementTest : MonoBehaviour
{
    private Rigidbody coinRigidbody;

    private void Awake()
    {
        this.coinRigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Move the coin by giving it a velocity
        this.coinRigidbody.velocity = new Vector3(0, 0, Random.Range(2.0f, 5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z >= 10.0f)
        {
            this.gameObject.SetActive(false);
            ObjectPoolingTest.SharedInstance.activeCoinCount--;
        }
    }
}
