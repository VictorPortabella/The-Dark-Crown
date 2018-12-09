using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    Transform player;
    Transform transform;
    UnityEngine.AI.NavMeshAgent nav;
    Animator anim;
    public Vector3 originalPosition;

    bool enemyTooClose = false;
    bool attackEnemy = false;
    bool firstMove = true;

    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    

	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform = GetComponent<Transform>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        originalPosition = transform.position;

        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();
	}
	
	// Update is called once per frame
	void Update () {

        if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && enemyTooClose == false)
        {
            nav.enabled = true;
            if (attackEnemy == true)
            {
                anim.SetBool("DetectPlayer", true);
                nav.SetDestination(player.position);

                Vector3 relativePos = player.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 100 * Time.deltaTime);
            }
            else 
            {
                if(firstMove == true)
                {
                    anim.SetBool("DetectPlayer", true);
                }
                nav.SetDestination(originalPosition);
                if (V3Equal_1(transform.position, originalPosition))
                {
                    anim.SetBool("DetectPlayer", false);
                    firstMove = false;
                }
            }
        }
        else
        {
            nav.enabled = false;
        }
	}
    public bool V3Equal_1(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 10;
    }

    public void EnemyClose (bool isClose)
    {
        enemyTooClose = isClose;
    }
    public void SetAttackEnemy(bool inRange)
    {
        attackEnemy = inRange;
    }
}
