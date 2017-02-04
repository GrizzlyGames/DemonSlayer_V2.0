using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Script : MonoBehaviour
{
    public int cost;
    private Transform trans;
    private bool bOpenDoor;

    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        if (bOpenDoor)
        {
            if (trans.localPosition.z > -2)
            {
                trans.Translate(Vector3.forward * -Time.deltaTime);                
            }
            else
            {
                bOpenDoor = false;
            }
        }                 
    }

    public void OpenDoor()
    {
        if (Game_Manager_Script.instance.playerCredits >= cost)
        bOpenDoor = true;
        Debug.Log("Open doors called");
        Game_Manager_Script.instance.playerCredits -= cost;
    }    
}
