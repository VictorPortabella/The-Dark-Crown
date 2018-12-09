using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestScript : MonoBehaviour {

    public Quest MyQuest { get; set; }
    public GameObject thisPrefab;

    private bool markedComplete = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Select()
    {
        GetComponent<Text>().color = Color.yellow;
        Questlog.MyInstance.ShowDescription(MyQuest);
    }

    public void DeSelect()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && markedComplete == false)
        {
            markedComplete = true;
            GetComponent<Text>().text += " (Complete)";
        }
        else if (!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = MyQuest.MyTitle;
        }
    }

    public void DestroyPrefab()
    {
        Destroy(thisPrefab);
    }
}
