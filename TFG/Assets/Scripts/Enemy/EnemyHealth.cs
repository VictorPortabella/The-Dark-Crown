using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

    private static EnemyHealth instance;
    public static EnemyHealth MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyHealth>();
            }
            return instance;
        }
    }
    [SerializeField]
    private Image healthImage;

    public int startingHealth;
    public int currentHealth;
    public float sinkSpeed = 2.5f;

    public GameObject blood;
    Renderer bloodRenderer;

    Animator anim;
    CapsuleCollider capsuleCollider;
    public bool isDead;
    bool isSinking;
    float timer = 0;
    float bloodtimer = 0;

    AudioSource enemyAudio;
    AudioSource enemyDeath;

    PlayerStats playerStats;
    EnemyStats enemyStats;
    public EnemyAttack enemyAttack;

    bool setStats = false;

    // Use this for initialization
    void Start () {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        AudioSource[] audios = GetComponents<AudioSource>();
        enemyAudio = audios[0];
        enemyDeath = audios[1];

        bloodRenderer = blood.GetComponent<Renderer>();
        bloodRenderer.enabled = false;   
	}
	
	// Update is called once per frame
	void Update () {
        if (setStats == false)
        {
            startingHealth = enemyStats.GetHealth();
            currentHealth = startingHealth;
            setStats = true;
        }

        timer += Time.deltaTime;
        bloodtimer += Time.deltaTime;

        if (timer > 30)
            timer = 0;
        if (bloodtimer > 30)
            bloodtimer = 0;

        if (bloodtimer > 0.5 && bloodRenderer.enabled == true)
        {
            bloodRenderer.enabled = false;
        }

        if (isSinking && timer > 2)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
	}

    public void TakeDamage (int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        healthImage.fillAmount -= (float)amount / (float)startingHealth;
        enemyAudio.Play();
        bloodRenderer.enabled = true;
        bloodtimer = 0;
        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        timer = 0;
        playerStats.GrantExperience(enemyStats.GetExp());
        playerStats.GetGold(enemyStats.GetGold());
        isDead = true;
        capsuleCollider.isTrigger = true;
        enemyAttack.enabled = false; 
        enemyDeath.Play();

        anim.SetTrigger("Dead");

    }

    public void StartSinking()
    {
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isSinking = true;

            Destroy(gameObject, 6f);
    }
}
