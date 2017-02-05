using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage_Script : MonoBehaviour {

    public int health = 50;
    public int creditsEarned = 50;
    public bool killed = false;

    public void Damage(int damage)
    {
        if (!killed)
        {
            Game_Manager_Script.instance.playerCredits += creditsEarned * damage;
            Player_UI_Controller_Script.instance.UpdateCreditsText();
            health -= damage;
            Debug.Log("H: " + health + "D: " + damage);

            if (health <= 0)
            {
                if (Game_Manager_Script.instance.currentNumEnemies <= 0)
                    Game_Manager_Script.instance.SpawnDropShip();                                

                StartCoroutine(DeathDelay());
            }
        }        
    }
        
    IEnumerator DeathDelay()
    {
        killed = true;
        Game_Manager_Script.instance.currentNumEnemies--;
        if (Game_Manager_Script.instance.currentNumEnemies < 1)
            Game_Manager_Script.instance.SpawnDropShip();
        yield return new WaitForSeconds(6);
        Destroy(gameObject); 
    }
}
