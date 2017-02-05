using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_Script : MonoBehaviour
{
    public bool alive = true;
    public bool canWalk = true;
    public int health = 100;
    public int maxHealth = 100;
    public float rayLength = 50;
    public int damage = 15;
    public Camera cam;

    private Vector3 previousPosition;
    private Animator anim;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        previousPosition = transform.position;
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Attack
        float distance = Vector3.Distance(transform.position, Player_Script.instance.PlayerPosition());     // Get distence from player // Debug.Log(distance);
        anim.SetFloat("targetDistence", distance);
        if (distance < 5)
            if (!bAttacking)
                StartCoroutine(AttackDelay());
        #endregion
        #region Determine-Actor-Speed
        float curSpeed;
        Vector3 curMove = transform.position - previousPosition;
        curSpeed = curMove.magnitude / Time.deltaTime;      // Debug.Log("curSpeed: " + curSpeed);
        anim.SetFloat("speed", curSpeed);
        previousPosition = transform.position;
        #endregion      // Set actor movement animation state
        #region Chase-Player
        if (alive && canWalk)  // check if actor is alive
            navMeshAgent.destination = Player_Script.instance.PlayerPosition();     // Chase player
        else
            navMeshAgent.velocity = Vector3.zero;
        #endregion
        #region RayCast
        Vector3 lineOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));        // Create a vector at the center of our camera's viewport
        Debug.DrawRay(lineOrigin, cam.transform.forward * rayLength, Color.red);        // Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green        
        #endregion
    }

    public void TakeDamage(int dmg)    // To be called from player raycast
    {
        if (health > 0)
        {
            health -= dmg;  // Take damage        
            Debug.Log(transform.name + " took " + dmg + " dmg. Remaing health: " + health);
            canWalk = false;
            StartCoroutine(WalkDelay());
            int rnd = Random.Range(1, 3);
            switch (rnd)
            {
                case 1:
                    anim.SetTrigger("hit1");
                    break;
                case 2:
                    anim.SetTrigger("hit2");
                    break;
            }


        }
        else
        {
            if (alive)
            {
                canWalk = false;
                anim.SetTrigger("dead");
                StartCoroutine(DeathDelay());
                alive = false;  // Killed   
            }
        }
    }

    private bool bAttacking = false;
    IEnumerator AttackDelay()
    {
        bAttacking = true;
        int rnd = Random.Range(1, 4);
        anim.SetInteger("attack", rnd);
        yield return new WaitForSeconds(1);
        anim.SetInteger("attack", 0);
        bAttacking = false;
    }
    IEnumerator WalkDelay()
    {
        yield return new WaitForSeconds(.5f);
        canWalk = true;
    }
    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }
}
