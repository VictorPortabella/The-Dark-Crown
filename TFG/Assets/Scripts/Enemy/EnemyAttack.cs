using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    float timeBetweenAttacks;
    float timeNextAttack;
    int attackDamage;

    GameObject player;
    PlayerHealth playerHealth;
    bool playerInRange;
    float timer;
    public Animator anim;
    public EnemyHealth enemyHealth;
    public EnemyMovement enemyMovement;
    public EnemyStats enemyStats;

    float timeAttacking = 2f;
    bool attackDone = false;
    bool enemyHurt = false;

    bool setStats = false;

    bool lower_stronger = false;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }
	
	// Update is called once per frame
	void Update () {

        if (setStats == false)
        {
            attackDamage = enemyStats.GetAttackDamage();
            timeBetweenAttacks = enemyStats.GetTimeBetweenAttacks();
            timeNextAttack = timeBetweenAttacks;
            setStats = true;
        }

        timer += Time.deltaTime;

        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")|| anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2")) && enemyHurt == true && attackDone == false && playerHealth.currentHealth > 0)
        {
            if (lower_stronger == false)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else {
                playerHealth.TakeDamage((int)Mathf.RoundToInt(attackDamage * 1.5f));
            }
            attackDone = true;
        }
        if (timer > timeNextAttack && playerInRange && enemyHealth.currentHealth > 0)
        {
            Attack();
        }
        if(playerHealth.currentHealth <= 0)
        {
            anim.SetBool("PlayerDead",true);
            enemyMovement.enabled = false;
        }	
	}

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
            enemyMovement.EnemyClose(true);
        }
    }
    
    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
            enemyMovement.EnemyClose(false);
        }
    }

    void Attack()
    {
        attackDone = false;
        float typeAttack = Random.Range(0f, 10f);
        if (typeAttack <= 7)
        {
            anim.SetTrigger("Attacking");
            lower_stronger = false;
        }
        else
        {
            lower_stronger = true;
            anim.SetTrigger("Attack2");
        }
        timeNextAttack = timeBetweenAttacks + Random.Range(-0.5f, 1.5f);
        timer = 0f;
    }

    public void SuccessfulAttack(bool isHurt)
    {
        enemyHurt = isHurt;
    }
}
