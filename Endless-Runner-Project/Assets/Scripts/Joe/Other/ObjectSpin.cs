using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    [SerializeField] private float spinSpeed;
    [SerializeField] private Vector3 spinAxis;
    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.Rotate(this.spinAxis * this.spinSpeed * Time.fixedDeltaTime, Space.World);
    }
}
