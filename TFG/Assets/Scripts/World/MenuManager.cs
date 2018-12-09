using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour {
    private static MenuManager instance;
    public static MenuManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
            }
            return instance;
        }
    }

    public Canvas menuCanvas;
    public Canvas mejorasCanvas;
    public Canvas gameStartCanvas;
    public Canvas heroImproveCanvas;
    public Canvas extrasCanvas;
    public Canvas deathCanvas;

    public MenuStats menuStats;

    Text goldText;
    public GameObject healthText;
    public GameObject staminaText;
    public GameObject attackText;

    public GameObject buyHealthText;
    public GameObject buyStaminaText;
    public GameObject buyAttackText;

    public GameObject buyExtraLifeText;
    public GameObject buyDoubleGoldText;
    public GameObject buyDoubleXpText;

    static bool death = false;

    static bool firstDeath = true;



    // Use this for initialization
    void Start () {
        if (death == false) {
            gameStartCanvas.gameObject.SetActive(true);

            menuCanvas.gameObject.SetActive(false);
            mejorasCanvas.gameObject.SetActive(false);
            heroImproveCanvas.gameObject.SetActive(false);
            extrasCanvas.gameObject.SetActive(false);
            deathCanvas.gameObject.SetActive(false);
            death = true;
        }
        else
        {
            if(firstDeath == true){
                FindObjectOfType<DialogueFirstDeathMessage>().StartDialogue();
                firstDeath = false;
            }
            deathCanvas.gameObject.SetActive(true);

            gameStartCanvas.gameObject.SetActive(false);
            menuCanvas.gameObject.SetActive(false);
            mejorasCanvas.gameObject.SetActive(false);
            heroImproveCanvas.gameObject.SetActive(false);
            extrasCanvas.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoMenu()
    {
        menuCanvas.gameObject.SetActive(true);
        mejorasCanvas.gameObject.SetActive(false);
        gameStartCanvas.gameObject.SetActive(false);
        deathCanvas.gameObject.SetActive(false);
        goldText = null;
    }

    public void GoMejoras()
    {
        mejorasCanvas.gameObject.SetActive(true);
        menuCanvas.gameObject.SetActive(false);
        heroImproveCanvas.gameObject.SetActive(false);
        extrasCanvas.gameObject.SetActive(false);
        heroImproveCanvas.gameObject.SetActive(false);
        extrasCanvas.gameObject.SetActive(false);

        goldText = GameObject.FindWithTag("GoldText").GetComponent<Text>();
        UpdateGoldText();
    }

    public void GoHeroImprove()
    {
        mejorasCanvas.gameObject.SetActive(false);
        heroImproveCanvas.gameObject.SetActive(true);
        goldText = GameObject.FindWithTag("GoldText").GetComponent<Text>();
        UpdateGoldText();
        UpdateStatsText();
    }

    public void GoExtras()
    {
        extrasCanvas.gameObject.SetActive(true);
        mejorasCanvas.gameObject.SetActive(false);
        goldText = GameObject.FindWithTag("GoldText").GetComponent<Text>();
        UpdateGoldText();
        UpdatePotText();
    }

    public void Jugar()
    {
        int scene = menuStats.Scene;
        if (scene == 1)
        {
            Application.LoadLevel("Scene 1");
        }
        else
        {
            Application.LoadLevel("Scene 2");
        }
    }

    void UpdateGoldText()
    {
        goldText.text = MenuStats.MyInstance.GoldOutGame.ToString();
    }

    void UpdateStatsText()
    {
        healthText.GetComponent<Text>().text = MenuStats.MyInstance.TotalHealthOutGame.ToString();
        staminaText.GetComponent<Text>().text = MenuStats.MyInstance.TotalStaminaOutGame.ToString();
        attackText.GetComponent<Text>().text = MenuStats.MyInstance.AttackDamageOutGame.ToString();
        buyHealthText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToHealth.ToString();
        buyStaminaText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToStamina.ToString();
        buyAttackText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToAttackDamage.ToString();
    }

    void UpdatePotText()
    {
        buyExtraLifeText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToExtraLife.ToString();
        buyDoubleGoldText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToDoubleGold.ToString();
        buyDoubleXpText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToDoubleXp.ToString();
    }

    public void BuyHealth()
    {
        healthText.GetComponent<Text>().text = MenuStats.MyInstance.TotalHealthOutGame.ToString();
        buyHealthText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToHealth.ToString();
        UpdateGoldText();
    }

    public void BuyStamina()
    {
        staminaText.GetComponent<Text>().text = MenuStats.MyInstance.TotalStaminaOutGame.ToString();
        buyStaminaText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToStamina.ToString();
        UpdateGoldText();
    }

    public void BuyAttack()
    {
        attackText.GetComponent<Text>().text = MenuStats.MyInstance.AttackDamageOutGame.ToString();
        buyAttackText.GetComponent<Text>().text = MenuStats.MyInstance.GoldToAttackDamage.ToString();
        UpdateGoldText();
    }

    public void BuyExtraLife()
    {
        //PonerAlgunIndicadorDeComprado
        UpdateGoldText();
    }

    public void BuyDoubleGold()
    {
        //PonerAlgunIndicadorDeComprado
        UpdateGoldText();
    }

    public void BuyDoubleXp()
    {
        //PonerAlgunIndicadorDeComprado
        UpdateGoldText();
    }
}
