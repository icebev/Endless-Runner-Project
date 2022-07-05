using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMovementTest : MonoBehaviour
{
    private Rigidbody coinRigidbody;
    // Start is called before the first frame update

    private void Awake()
    {
        this.coinRigidbody = this.GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
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
