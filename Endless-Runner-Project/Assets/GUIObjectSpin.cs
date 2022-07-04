using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIObjectSpin : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up * 5.0f, Space.World);
    }
}
