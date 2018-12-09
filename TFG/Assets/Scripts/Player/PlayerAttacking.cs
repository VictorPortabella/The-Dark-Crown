using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerAttacking : MonoBehaviour {
    private static PlayerAttacking instance;
    public static PlayerAttacking MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerAttacking>();
            }
            return instance;
        }
    }

    public static bool enableAttack = true;
    public bool EnableAttack
    {
        get { return enableAttack; }
        set { enableAttack = value; }
    }

    public static bool enableDefense = true;
    public bool EnableDefense
    {
        get { return enableDefense; }
        set { enableDefense = value; }
    }

    public static bool enableCombo = false;
    public bool EnableCombo
    {
        get { return enableCombo; }
        set { enableCombo = value; }
    }

    int damagePerAttack;

    public GameObject comboEffect;
    GameObject effect;
    public GameObject Q_text;
    int currentCombo = 0;
    public bool inCombo = false;
    public Slider comboSlider;
    public GameObject comboUI;

    Animator anim;
    float timer;
    float comboTimer;
    float elapsed;
    float elapsedCombo;
    float comboDuration;
    float timerAttack2;
    float defendingTimer;
    float nextDefenseTimer;
    float checkCannotDefense;


    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    PlayerStats playerStats;
    public GameObject particleSystem;
    public GameObject healthParticleSystem;
    public GameObject staminaParticleSystem;

    Rigidbody rb;

    public GameObject shield;
    Renderer shieldRenderer;

    bool disabledMovement = false;
    float timeAttacking = 0.5f;
    bool attacking = false;
    bool enemyHurt = false;
    bool enemyHurt2 = false;
    bool enemyHurt3 = false;

    bool secondAttackDone = false;

    int staminaToAttack;
    int staminaToDefense;
    bool playerProtecting = false;

    GameObject enemyHitted;
    public Inventory inventory;

    int increaseDmgCombo;

    AudioSource audioCombo;
    AudioSource audioCannotDefense;

    bool aux = false;
    float timerAux = 0;
    // Use this for initialization
    void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        playerHealth = GetComponent<PlayerHealth>();
        playerStats = GetComponent<PlayerStats>();
        AudioSource[] audios = GetComponents<AudioSource>();
        audioCombo = audios[4];
        audioCannotDefense = audios[5];

        damagePerAttack = playerStats.attackDamage;
        anim = GetComponent<Animator>();
        timer = 0;
        timerAttack2 = 0;
        defendingTimer = 0;
        nextDefenseTimer = 0;
        comboTimer = 0;
        elapsedCombo = 0;
        comboDuration = 0;
        checkCannotDefense = 0;
        shieldRenderer = shield.GetComponent<Renderer>();
        shieldRenderer.enabled = false;
        comboUI.SetActive(false);

        staminaToDefense = playerStats.staminaToDeffense;
        staminaToAttack = playerStats.staminaToAttack;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        timerAttack2 += Time.deltaTime;
        nextDefenseTimer += Time.deltaTime;
        comboTimer += Time.deltaTime;
        checkCannotDefense += Time.deltaTime;
        timerAux += Time.deltaTime;

        if (effect != null)
        {
            effect.transform.position = this.transform.position;
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            playerMovement.EnableMovement();
            enemyHurt = false;
            enemyHurt2 = false;
            enemyHurt3 = false;
            anim.ResetTrigger("TriggerAttack2");
        }
        if(timerAux > 1)
        {
            aux = true;
        }
        if ((Input.GetButton("Fire1") && aux == true && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") &&  playerHealth.CheckStamina(staminaToAttack) == true && !inventory.eventSystem.IsPointerOverGameObject(-1) && !inventory.IsMovingItem()) && enableAttack == true)
        {
            Attack();
        }if((Input.GetButton("Fire1") && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && secondAttackDone == false && timer > 0.2 && playerHealth.CheckStamina(staminaToAttack) == true && !inventory.eventSystem.IsPointerOverGameObject(-1) && !inventory.IsMovingItem()))
        {
            Attack2();
        }

       
        if (enemyHitted != null && enemyHurt == false && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            Debug.Log(enemyHitted);
            enemyHitted.GetComponent<EnemyHealth>().TakeDamage(damagePerAttack);
            enemyHurt = true;
            IncreaseCombo(1);
        }

        if (enemyHitted != null && enemyHurt2 == false && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            enemyHitted.GetComponent<EnemyHealth>().TakeDamage(damagePerAttack);
            enemyHurt2 = true;
            timerAttack2 = 0;
            IncreaseCombo(1);
        }
        if ((enemyHitted != null && enemyHurt3 == false && timerAttack2 > 0.2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))){
            enemyHitted.GetComponent<EnemyHealth>().TakeDamage(damagePerAttack);
            enemyHurt3 = true;
            IncreaseCombo(1);
        }

        if (Input.GetButton("Fire2") && defendingTimer < 1 && nextDefenseTimer > 2 && playerHealth.CheckStamina(staminaToDefense) == true && !inventory.eventSystem.IsPointerOverGameObject(-1) && !inventory.IsMovingItem() && enableDefense == true) //protegerse
        {
            if (playerProtecting == false)
            {
                defendingTimer = 0;
            }
            defendingTimer += Time.deltaTime;
            playerProtecting = true;
            shieldRenderer.enabled = true;
            playerHealth.PlayerProtected(true);
            playerMovement.PlayerProtected(true);
        } else if (Input.GetButton("Fire2") && checkCannotDefense > 0.5 && (nextDefenseTimer < 2 || playerHealth.CheckStamina(staminaToDefense) == false) && !inventory.eventSystem.IsPointerOverGameObject(-1) && !inventory.IsMovingItem() && enableDefense == true) {
            audioCannotDefense.Play();
            checkCannotDefense = 0;
        }
        else {
            shieldRenderer.enabled = false;
            playerHealth.PlayerProtected(false);
            playerMovement.PlayerProtected(false);
        }

        if (Input.GetButtonUp("Fire2") && playerProtecting == true)
        {
            defendingTimer = 0;
            nextDefenseTimer = 0;
            playerProtecting = false;
        }
        if(comboTimer > 5 && currentCombo > 0 && inCombo == false)
        {
            elapsedCombo += Time.deltaTime;
            if (elapsedCombo >= 1f) //Resta 1 de Stamina cada segundo
            {
                elapsedCombo = elapsedCombo % 1f;
                currentCombo -= 1;
                comboSlider.value = currentCombo;
            }
            if(currentCombo<= 0)
            {
                comboUI.SetActive(false);
            }
        }
        if (currentCombo > 10)
        {
            currentCombo = 10;
        }
        if (currentCombo >= 10 && Input.GetKeyDown(KeyCode.Q) && inCombo == false)
        {
            ActiveCombo();
        }
        if(inCombo == true)
        {
            comboDuration += Time.deltaTime;
            if(comboDuration >= 10)
            {
                currentCombo = 0;
                comboSlider.value = currentCombo;
                inCombo = false;
                comboDuration = 0;
                comboUI.SetActive(false);
                damagePerAttack -= increaseDmgCombo;
                particleSystem.SetActive(false);
                healthParticleSystem.SetActive(false);
                staminaParticleSystem.SetActive(false);
            }
        }
        if(currentCombo == 10 && inCombo == false)
        {
            Q_text.SetActive(true);
        }
        if(currentCombo < 10 || inCombo == true)
        {
            Q_text.SetActive(false);
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        playerMovement.DisableMovement();
        playerHealth.UseStamina(staminaToAttack);
        timer = 0;
        secondAttackDone = false;
        aux = false;
        timerAux = 0;
    }
    void Attack2()
    {
        anim.SetTrigger("TriggerAttack2");
        playerMovement.DisableMovement();
        playerHealth.UseStamina(staminaToAttack);
        secondAttackDone = true;
    }
    
    public void HitEnemy(GameObject enemy)
    {
        enemyHitted = enemy;
    }

    public void UpdateStats(int attackDamage)
    {
        damagePerAttack = attackDamage;
    }

    public void IncreaseCombo(int combo)
    {
        if (enableCombo == true)
        {
            comboUI.SetActive(true);
            currentCombo += combo;
            comboSlider.value = currentCombo;
            comboTimer = 0;
        }
    }
    
    void ActiveCombo()
    {
        effect = Instantiate(comboEffect, this.transform.position, this.transform.rotation);
        Destroy(effect, 10f);
        inCombo = true;
        increaseDmgCombo = damagePerAttack;
        damagePerAttack += increaseDmgCombo;
        particleSystem.SetActive(true);
        healthParticleSystem.SetActive(true);
        staminaParticleSystem.SetActive(true);
        audioCombo.Play();
    }

}

