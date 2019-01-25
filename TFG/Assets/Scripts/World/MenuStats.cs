using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuStats : MonoBehaviour {
    private static MenuStats instance;
    public static MenuStats MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuStats>();
            }
            return instance;
        }
    }

    private static int scene = 1;
    public int Scene
    {
        get { return scene; }
        set { scene = value; }
    }

    private static int level = 1;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    private static int expNextLevel = 3;
    public int ExpNextLevel
    {
        get { return expNextLevel; }
        set { expNextLevel = value; }
    }

    private static int fibNextLevel = 3;
    public int FibNextLevel
    {
        get { return fibNextLevel; }
        set { fibNextLevel = value; }
    }

    //OUTGAME
    public static int totalHealthOutGame = 4;
    public static int attackDamageOutGame = 1;
    public static int totalStaminaOutGame = 6;

    public static int staminaToAttack = 1;
    public static int staminaToDef = 1;
    public static int staminaRecover = 1;

    public static int goldToHealth = 10;
    public static int goldToStamina = 10;
    public static int goldToattackDamage = 10;

    public static int goldOutGame = 0;

    public int GoldOutGame
    {
        get { return goldOutGame; }
        set { goldOutGame = value; }
    }

    public int TotalHealthOutGame
    {
        get { return totalHealthOutGame; }
        set { totalHealthOutGame = value; }
    }

    public int TotalStaminaOutGame
    {
        get { return totalStaminaOutGame; }
        set { totalStaminaOutGame = value; }
    }

    public int StaminaToAttack
    {
        get { return staminaToAttack; }
        set { staminaToAttack = value; }
    }

    public int StaminaToDef
    {
        get { return staminaToDef; }
        set { staminaToDef = value; }
    }

    public int StaminaRecover
    {
        get { return staminaRecover; }
        set { staminaRecover = value; }
    }

    public int AttackDamageOutGame
    {
        get { return attackDamageOutGame; }
        set { attackDamageOutGame = value; }
    }

    public int GoldToHealth
    {
        get { return goldToHealth; }
        set { goldToHealth = value; }
    }

    public int GoldToStamina
    {
        get { return goldToStamina; }
        set { goldToStamina = value; }
    }

    public int GoldToAttackDamage
    {
        get { return goldToattackDamage; }
        set { goldToattackDamage = value; }
    }

    public static bool potExtraLife = false;
    public bool PotExtraLife
    {
        get { return potExtraLife; }
        set { potExtraLife = value; }
    }

    public static bool potDoubleGold = false;
    public bool PotDoubleGold
    {
        get { return potDoubleGold; }
        set { potDoubleGold = value; }
    }

    public static bool potDoubleXp = false;
    public bool PotDoubleXp
    {
        get { return potDoubleXp; }
        set { potDoubleXp = value; }
    }

    public static int goldToExtraLife = 1000;
    public int GoldToExtraLife
    {
        get { return goldToExtraLife; }
        set { goldToExtraLife = value; }
    }

    public static int goldToDoubleGold = 1000;
    public int GoldToDoubleGold
    {
        get { return goldToDoubleGold; }
        set { goldToDoubleGold = value; }
    }

    public static int goldToDoubleXp = 1000;
    public int GoldToDoubleXp
    {
        get { return goldToDoubleXp; }
        set { goldToDoubleXp = value; }
    }

    static AudioSource[] audios;
    AudioSource correctBuy;
    AudioSource errorBuy;

    // Use this for initialization
    void Start () {
        Debug.Log("ORO " + goldOutGame);
        audios = GetComponents<AudioSource>();

        correctBuy = audios[0];
        errorBuy = audios[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuyHealth()
    {
        if (goldOutGame >= goldToHealth)
        {
            goldOutGame -= goldToHealth;
            goldToHealth *= 2;
            totalHealthOutGame = ((int)Mathf.RoundToInt(totalHealthOutGame * 1.1f));
            MenuManager.MyInstance.BuyHealth();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }

    public void BuyStamina()
    {
        if (goldOutGame >= goldToStamina)
        {
            goldOutGame -= goldToStamina;
            goldToStamina *= 2;
            totalStaminaOutGame = ((int)Mathf.RoundToInt(totalStaminaOutGame * 1.1f)); 
            MenuManager.MyInstance.BuyStamina();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }

    public void BuyAttackDmg()
    {
        if (goldOutGame >= goldToattackDamage)
        {
            goldOutGame -= goldToattackDamage;
            goldToattackDamage *= 2;
            attackDamageOutGame = ((int)Mathf.RoundToInt(attackDamageOutGame * 1.1f));
            MenuManager.MyInstance.BuyAttack();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }

    public void GoldToGoldOutgame(int gold)
    {
        goldOutGame = gold;
    }

    public void BuyExtraLife()
    {
        if (goldOutGame >= goldToExtraLife && potExtraLife == false)
        {
            goldOutGame -= goldToExtraLife;
            potExtraLife = true;
            MenuManager.MyInstance.BuyExtraLife();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }

    public void BuyDoubleGold()
    {
        if (goldOutGame >= goldToDoubleXp && potDoubleGold == false)
        {
            goldOutGame -= goldToDoubleGold;
            potDoubleGold = true;
            MenuManager.MyInstance.BuyDoubleGold();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }

    public void BuyDoubleXp()
    {
        if (goldOutGame >= goldToDoubleXp && potDoubleXp == false)
        {
            goldOutGame -= goldToDoubleXp;
            potDoubleXp = true;
            MenuManager.MyInstance.BuyDoubleXp();
            correctBuy.Play();
        }
        else
        {
            errorBuy.Play();
        }
    }
}
