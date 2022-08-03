using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterStart : MonoBehaviour
{

    public float timeUntilDestroy;

    // Update is called once per frame
    void Update()
    {
        this.timeUntilDestroy -= Time.deltaTime;

        if(this.timeUntilDestroy <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
