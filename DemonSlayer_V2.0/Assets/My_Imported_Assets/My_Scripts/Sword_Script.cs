using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Script : MonoBehaviour {
        
    public GameObject sword;
    public GameObject weapon;

    public void swordVisible()
    {
        sword.SetActive(true);
        weapon.SetActive(false);
    }

    public void swordInvisible()
    {
        weapon.SetActive(true);
        sword.SetActive(false);        
    }    
}
