using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDuckTrigger : MonoBehaviour
{
    private GameObject cam;
    private Animator duckAnimator;

    private void Start()
    {
        this.cam = GameObject.FindGameObjectWithTag("CMCam");
        this.duckAnimator = this.cam.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        this.duckAnimator.SetBool("CameraDuck", true);
    }

    private void OnTriggerExit(Collider other)
    {
        this.duckAnimator.SetBool("CameraDuck", false);



    }
}
