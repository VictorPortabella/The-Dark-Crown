using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStats : MonoBehaviour {
    PlayerStats playerStats;

	// Use this for initialization
	void Awake () {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetHeroLevel()
    {
        return playerStats.GetLevel();
    }
}
