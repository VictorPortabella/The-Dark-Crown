using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour {

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField] 
    private Transform questParent;

    [SerializeField]
    private Text questDescription;

    [SerializeField]
    private Text numberQuests;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<Quest> questList;

    private Quest selected;

    private static Questlog instance;
    public static Questlog MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Questlog>();
            }
            return instance;
        }
    }

	// Use this for initialization
	void Awake () {
        questList = new List<Quest>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L))
        {
            OpenClose();
        }
	}

    public void AcceptQuest(Quest quest)
    {
        GameObject go = Instantiate(questPrefab, questParent);

        QuestScript qs = go.GetComponent<QuestScript>(); 
        //have a reference of each other
        qs.MyQuest = quest;
        quest.MyQuestScript = qs;

        go.GetComponent<Text>().text = quest.MyTitle;

        questList.Add(quest);
        UpdateNumberQuests();
    }

    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selected != null && selected != quest)
            {
                selected.MyQuestScript.DeSelect();
            }

            string objectives = string.Empty;
            string reward = string.Empty;

            selected = quest;

            string title = quest.MyTitle;

            foreach (Objective obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            if (quest.MyReward.MyItems.Length != 0)
            {
                foreach(Item item in quest.MyReward.MyItems)
                {
                    reward += item.type.ToString().ToLower() + "\n";
                }
            }
            if (quest.MyReward.MyGold != 0)
            {
                reward += "Gold: " + quest.MyReward.MyGold + "\n";
            }
            if (quest.MyReward.MyExp != 0)
            {
                reward += "Exp: " + quest.MyReward.MyExp + "\n";
            }

            questDescription.text = string.Format("<b>{0}</b>\n<size=10>{1}</size>\n\nObjectives\n<size=10>{2}</size>\nReward\n<size=10>{3}</size>", title, quest.MyDescription, objectives,reward);
        }
    }

    public void UpdateQuestItemCount(Item item)
    {
        foreach(Quest q in questList)
        {
            foreach(CollectObjective obj in q.MyCollectObjectives)
            {
                obj.UpdateItemCount(item);
            }
        }
    }

    public void CheckCompletion()
    {
        foreach (Quest q in questList)
        {
            q.MyQuestScript.IsComplete();
        }
    }

    public void OpenClose()
    {
        UpdateNumberQuests();
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void UpdateNumberQuests()
    {
        numberQuests.text = questList.Count.ToString();
    }

    public void RemoveQuest(Quest quest)
    {
        questList.Remove(quest);
        quest.MyQuestScript.DestroyPrefab();
    }
}
