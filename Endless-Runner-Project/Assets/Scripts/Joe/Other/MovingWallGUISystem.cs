using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MovingWallGUISystem : MonoBehaviour
{
    public float minXValue;
    public float startingXValue;
    public float moveSpeed;

    private void Update()
    {
        foreach (Transform child in this.transform)
        {
            if (child.position.x <= this.minXValue)
            {
                child.position = new Vector3(this.startingXValue, child.position.y, child.position.z);
            }

            Vector3 displacementVector = new Vector3(-this.moveSpeed * Time.deltaTime, 0, 0);

            child.position += displacementVector;
        }
        
    }

}
