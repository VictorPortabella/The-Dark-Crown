using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    private static PlayerHealth instance;
    public static PlayerHealth MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHealth>();
            }
            return instance;
        }
    }

    public int startingHealth;
    public int currentHealth;
    public Slider healthSlider;

    int startingStamina;
    public int currentStamina;
    public Slider staminaSlider;

    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    Animator anim;
    PlayerMovement playerMovement;
    PlayerAttacking playerAttacking;
    PlayerStats playerStats;

    AudioSource playerAudio;
    AudioSource playerAudioDefense;
    AudioSource playerDead;

    bool isProtected;

    float timeToRecoverStamina = 2.5f;
    float timer;
    float elapsed;
    int staminaRecovered;
    int staminaToDefense;

    bool isDead;
    bool damaged;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttacking = GetComponent<PlayerAttacking>();
        playerStats = GetComponent<PlayerStats>();
        AudioSource[] audios = GetComponents<AudioSource>();

        playerAudio = audios[0];
        playerAudioDefense = audios[1];
        playerDead = audios[2];

        startingHealth = playerStats.totalHealth;
        startingStamina = playerStats.totalStamina;

        currentHealth = startingHealth;
        currentStamina = startingStamina;

        healthSlider.maxValue = startingHealth;
        staminaSlider.maxValue = startingStamina;
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;

        staminaToDefense = playerStats.staminaToDeffense;
        staminaRecovered = playerStats.staminaRegeneration;

        timer = 0;
        elapsed = 0;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        elapsed += Time.deltaTime;

        if (damaged)
        {
            playerAudio.Play();
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (timer >= timeToRecoverStamina)
        {
            if (elapsed >= 1f && currentStamina < startingStamina) //Resta 1 de Stamina cada segundo
            {
                elapsed = elapsed % 1f;
                this.RecoverStamina();
            }
        }
	}

    public void TakeDamage (int amount)
    {
        if (!isProtected)
        {
            if (!playerAttacking.inCombo)
            {
                damaged = true;
                currentHealth -= amount;
                healthSlider.value = currentHealth;
                if (currentHealth <= 0 && !isDead)
                {
                    Death();
                }
            }
        } else
        {//se ha defendido con escudo
            playerAudioDefense.Play();
            this.UseStamina(staminaToDefense);
            playerAttacking.IncreaseCombo(1);
        }
    }

    public void UseStamina (int amount)
    {
        if (!playerAttacking.inCombo)
        {
            timer = 0;
            currentStamina -= amount;
            staminaSlider.value = currentStamina;
        }
    }

    public bool CheckStamina(int amount)
    {
        if(currentStamina - amount >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Death()
    {
        isDead = true;
        anim.SetTrigger("Die");
        playerDead.Play();
        playerMovement.enabled = false;
        playerAttacking.enabled = false;

        if(Inventory.MyInstance.bagIsClose == false)
        {
            Inventory.MyInstance.OpenClose();
        }
        //Quitar Potenciadores
        if (MenuStats.MyInstance.PotExtraLife == true)
        {
            //poner efecto resucitar
            MenuStats.MyInstance.PotExtraLife = false;
        }
        else
        {
            MenuStats.MyInstance.PotDoubleGold = false;
            MenuStats.MyInstance.PotDoubleXp = false;
            //Poner espera 5 segundos
            GameSystemInGame.MyInstance.ChangeScene("SceneInterfaz", 6);
        }
    }

    public void PlayerProtected (bool playerProtected){
        isProtected = playerProtected;    
    }

    //Recuperaremos stamina cuando haga Xs que no atacamos ni defendemos
    void RecoverStamina()
    {
        currentStamina += staminaRecovered;
        if (currentStamina > startingStamina)
        {
            currentStamina = startingStamina;
        }

        staminaSlider.value = currentStamina;

    }

    public void UpdateStats(int totalHealth, int totalStamina) 
    {
        int restoreHealth = totalHealth - startingHealth;
        int restoreStamina = totalStamina - startingStamina;

        startingHealth = totalHealth;
        startingStamina = totalStamina;

        currentHealth += restoreHealth;
        currentStamina += restoreStamina;

        healthSlider.maxValue = startingHealth;
        staminaSlider.maxValue = startingStamina;

        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;
    }

    public void Heal ()
    {
        currentHealth += startingHealth / 4;
        if (currentHealth > startingHealth)
        {
            currentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
    }

    public void Recover()
    {
        currentStamina += startingStamina / 4;
        if (currentStamina > startingStamina)
        {
            currentStamina = startingStamina;
        }
        healthSlider.value = currentStamina;
    }
}
