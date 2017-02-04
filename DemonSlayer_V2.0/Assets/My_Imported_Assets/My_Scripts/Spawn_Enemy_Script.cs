using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy_Script : MonoBehaviour {

    public GameObject enemyType1;

    public int force;

    [SerializeField]
    private Transform playerTransform;

	// Use this for initialization
	void Start () {
        if(playerTransform == null)
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(new Vector3 (playerTransform.position.x, playerTransform.position.y + 50, playerTransform.position.z));
	}

    void SpawnEnemy()
    {
        GameObject go = Instantiate(enemyType1, transform.localPosition, transform.localRotation) as GameObject;
        Rigidbody rb = go.GetComponent(typeof(Rigidbody)) as Rigidbody;
        rb.AddForce(transform.forward * force * Time.deltaTime);
        go.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
