using System.Collections;


using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour {

    public EnemyAttack enemyAttack;
    GameObject player;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            enemyAttack.SuccessfulAttack(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            enemyAttack.SuccessfulAttack(false);
        }
    }
}
