using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    private static PlayerStats instance;
    public static PlayerStats MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
            }
            return instance;
        }
    }

    PlayerHealth playerHealth;
    PlayerAttacking playerAttacking;

    public GameObject levelUpEffect;
    public MenuStats menuStats;
    AudioSource levelUpSound;
    GameObject effect;

    public Image xpSlider;
    public Text text;

    public GoldText goldText;
 
    //INGAME
        //nivel
        public int Level;
        public int expNextLvl;
        private int auxFibonacciExp;

        public int currentExp;
        //Vida
        public int totalHealth;
        //Ataque
        public int attackDamage;
        float timeBetweenAttack;
        //Stamina
        public int totalStamina;
        public int staminaToAttack;
        public int staminaToDeffense;
        public int staminaRegeneration;
        //Gold
        public int gold;



    // Use this for initialization
    void Awake () {
        AudioSource[] audios = GetComponents<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttacking = GetComponent<PlayerAttacking>();

        levelUpSound = audios[3];

        //INGAME
        //level
            Level = MenuStats.MyInstance.Level;
            text.text = Level.ToString();
            expNextLvl = MenuStats.MyInstance.ExpNextLevel;
            auxFibonacciExp = MenuStats.MyInstance.FibNextLevel;
            currentExp = 0;
            //Vida 
            totalHealth = MenuStats.MyInstance.TotalHealthOutGame;
            //Ataque
            attackDamage = MenuStats.MyInstance.AttackDamageOutGame;
            timeBetweenAttack = 0.5f;
            //Stamina
            totalStamina = MenuStats.MyInstance.TotalStaminaOutGame;
            staminaToAttack = MenuStats.MyInstance.StaminaToAttack;
            staminaToDeffense = MenuStats.MyInstance.StaminaToDef;
            staminaRegeneration = MenuStats.MyInstance.StaminaRecover;
            xpSlider.fillAmount = 0;
            //Gold
            gold = 0;            
    }
	
	// Update is called once per frame
	void Update () {
        if(effect != null)
        {
            effect.transform.position = this.transform.position;
        }
	}


    public void GrantExperience(int amount)
    {
        if (MenuStats.MyInstance.PotDoubleXp == false)
        {
            currentExp += amount;
        }
        else
        {
            currentExp += (2 * amount);
        }
        while(currentExp >= expNextLvl)
        {
            LevelUp();
        }
        xpSlider.fillAmount = (0.5f * currentExp) / expNextLvl;
    }

    void LevelUp()
    {
        currentExp -= expNextLvl;
        Level++;
        text.text = Level.ToString();
        levelUpSound.Play();
        effect = Instantiate(levelUpEffect, this.transform.position, this.transform.rotation);
        Destroy (effect, 3.5f);
        //Escalabilidad
            //Experiencia
            int aux = expNextLvl;
            expNextLvl = expNextLvl + auxFibonacciExp;
            auxFibonacciExp = aux;

            //Vida 
            totalHealth *= 2;

            //Ataque
            attackDamage *= 2;

            //Stamina
            totalStamina += 3;

        playerHealth.UpdateStats(totalHealth, totalStamina);
        playerAttacking.UpdateStats(attackDamage);

        //Acutalizamos MenuStats
        MenuStats.MyInstance.Level = Level;
        MenuStats.MyInstance.TotalHealthOutGame = totalHealth;
        MenuStats.MyInstance.TotalStaminaOutGame = totalStamina;
        MenuStats.MyInstance.AttackDamageOutGame = attackDamage;
        MenuStats.MyInstance.ExpNextLevel = expNextLvl;
        MenuStats.MyInstance.FibNextLevel = auxFibonacciExp;
    }

    public int GetLevel()
    {
        return Level;
    }

    public void GetGold(int amount)
    {
        if (MenuStats.MyInstance.PotDoubleGold == false)
        {
            gold += amount;
        }
        else
        {
            gold += (2 * amount);
        }
        goldText.ChangeGoldText(gold);
    }

    public void UseElixir()
    {
        attackDamage = (int)Mathf.RoundToInt(attackDamage * 1.1f);
        totalHealth = (int)Mathf.RoundToInt(totalHealth * 1.1f);
        totalStamina = (int)Mathf.RoundToInt(totalStamina * 1.1f);

        playerHealth.UpdateStats(totalHealth, totalStamina);
        playerAttacking.UpdateStats(attackDamage);
    }
}
