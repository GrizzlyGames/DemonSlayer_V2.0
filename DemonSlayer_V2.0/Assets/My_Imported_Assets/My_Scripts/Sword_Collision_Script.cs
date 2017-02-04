using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Collision_Script : MonoBehaviour {

    public int damage = 100;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<TakeDamage_Script>() != null)
        {
            col.gameObject.GetComponent<TakeDamage_Script>().Damage(damage);
        }
    }
}
