using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GoldText : MonoBehaviour {

    Text goldText;
	// Use this for initialization
	void Start () {
        goldText = GetComponent<Text>();
        ChangeGoldText(PlayerStats.MyInstance.gold);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ChangeGoldText(int gold)
    {
        goldText.text = gold.ToString();
    }
}
