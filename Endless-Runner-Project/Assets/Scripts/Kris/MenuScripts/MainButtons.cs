using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainButtons : MonoBehaviour
{

    [SerializeField] private GameObject[] Buttons;


    public void DisableButtons()
    {
        for (int i = 0; i <= this.Buttons.Length; i++)
        {
            Button TempButton = this.Buttons[i].GetComponent<Button>();
            

        }
    }
    public void EnableButtons()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
