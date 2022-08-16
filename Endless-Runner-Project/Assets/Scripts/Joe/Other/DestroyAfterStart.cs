using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* DESTROY AFTER START CLASS
 * Author(s): Joe Bevis
 * Date last modified: 16/08/2022
 *******************************************************************************
 * CHANGE NOTES:
 * Commenting pass
 */
/// <summary>
/// For destroying GameObjects that are no longer required 
/// after a set duration from the start in order to improve optimization
/// </summary>
public class DestroyAfterStart : MonoBehaviour
{
    [Tooltip("How long the GameObject will last in the scene.")]
    [SerializeField] private float timeUntilDestroy;

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
