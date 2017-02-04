using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Alien_Oger_Script : MonoBehaviour {

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    Chase_Player chasePlayerScript;

	// Use this for initialization
	void Start () {
        chasePlayerScript = GetComponent<Chase_Player>();
        anim = GetComponent<Animator>();
	}

    void FixedUpdate()
    {
        // Debug.Log("Nav Speed: " + Vector3.Project(navMeshAgent.desiredVelocity, transform.forward).magnitude);
        float movementSpeed = Vector3.Project(navMeshAgent.desiredVelocity, transform.forward).magnitude;

        anim.SetFloat("speed", movementSpeed);        
    }

    public void AlienHit()
    {
        int rndNum = Random.Range(1, 3);

        switch (rndNum){
            case 1:
                anim.SetTrigger("hit1");
                break;
            default:
                anim.SetTrigger("hit2");
                break;
        }            
    }
    public void AlienKilled()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        navMeshAgent.speed = 0;
        anim.SetTrigger("death");
    }
}
