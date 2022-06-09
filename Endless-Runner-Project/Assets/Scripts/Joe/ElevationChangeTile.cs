using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevationChangeTile : MonoBehaviour
{
    private GameObject player;
    public float elevationChange = 0;
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        this.player.transform.position += new Vector3(0, this.elevationChange, 0);
    }
}
