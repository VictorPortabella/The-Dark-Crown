using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum EnemyStatsType { TROLL, GOBLIN, BOSS };

public class EnemyStats : MonoBehaviour {
    GlobalStats globalStats;
    public EnemyStatsType type;

    [SerializeField]
    private TextMeshProUGUI levelText;

    //nivel
    public int Level;
    int minLevel;
    int maxLevel;
    //Vida
    public int totalHealth;
    //Ataque
    public int attackDamage;
    float timeBetweenAttack;
    //LOOT
    public int gold;
    public int experience;

    // Use this for initialization
    void Start () {
        globalStats = GameObject.FindWithTag("GameController").GetComponent<GlobalStats>();
        int heroLevel = globalStats.GetHeroLevel();
        //LevelRange
        switch (type)
        {
            case EnemyStatsType.GOBLIN:
                minLevel = 1;
                maxLevel = 5;
                break;
            case EnemyStatsType.TROLL:
                minLevel = 3;
                maxLevel = 8;
                break;
            case EnemyStatsType.BOSS:
                minLevel = 8;
                maxLevel = 8;
                break;
        }
        if (heroLevel == 1)
        {
            Level = heroLevel;
        }
        else
        {
            if (heroLevel < minLevel)
            {
                Level = minLevel;
            }
            else if (heroLevel <= maxLevel)
            {
                int changeLevel = Random.Range(0, 10);

                if (changeLevel == 1 && heroLevel - 1 >= minLevel) //enemigo flojo;
                {
                    Level = heroLevel - 1;
                }
                else if (changeLevel == 9 && heroLevel + 1 <= maxLevel) //enemigo fuerte;
                {
                    Level = heroLevel + 1;
                }
                else
                {
                    Level = heroLevel;
                }
            }
            else
            {
                Level = maxLevel;
            }
        }
        
        levelText.text = Level.ToString();

        switch (type)
        {
            case EnemyStatsType.GOBLIN:
                totalHealth = 3 * (int)Mathf.Pow(2, Level - 1); //1 combo
                attackDamage = 1 * (int)Mathf.Pow(2, Level - 1);
                timeBetweenAttack = 3f;
                experience = 1;
                gold = 1;
                break;
            case EnemyStatsType.TROLL:
                totalHealth = 9 * (int)Mathf.Pow(2, Level - 1); //3 combo
                attackDamage = 1 * (int)Mathf.Pow(2, Level - 1);
                timeBetweenAttack = 3f;
                experience = 4;
                gold = 3;
                break;
            case EnemyStatsType.BOSS:
                totalHealth = 18 * (int)Mathf.Pow(2, 6 - 1); //6 combo
                attackDamage = 1 * (int)Mathf.Pow(2, 6 - 1);
                timeBetweenAttack = 2f;
                experience = 10;
                gold = 15;
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public int GetHealth()
    {
        return totalHealth;
    }
    public int GetAttackDamage()
    {
        return attackDamage;
    }
    public float GetTimeBetweenAttacks()
    {
        return timeBetweenAttack;
    }
    public int GetExp()
    {
        return experience;
    }
    public int GetGold()
    {
        return gold;
    }
}

