using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiverWindow : Window {
    private static QuestGiverWindow instance;

    [SerializeField]
    private GameObject backBtn, acceptBtn,completeBtn, questDescription;

    public static QuestGiverWindow MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }

            return instance;
        }
    }

    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;

    [SerializeField]
    private Transform questArea;

    private List<GameObject> quests = new List<GameObject>();

    private Quest selectedQuest;

    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;

        foreach(GameObject go in quests)
        {
            Destroy(go);
        }

        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);

        foreach (Quest quest in questGiver.MyQuests)
        {
            GameObject go = Instantiate(questPrefab, questArea);
            go.GetComponent<Text>().text = quest.MyTitle;
            //Set de color
            if(quest.IsAccepted == true && quest.IsComplete == false)
            {
                go.GetComponent<Text>().color = Color.grey;
            }
            if(quest.IsAccepted == true && quest.IsComplete == true)
            {
                go.GetComponent<Text>().color = Color.green;
            }
            if (quest.IsAccepted == false)
            {
                go.GetComponent<Text>().color = Color.black;
            }

            go.GetComponent<QGQuestScript>().MyQuest = quest;

            quests.Add(go);
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        completeBtn.SetActive(false);
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        this.selectedQuest = quest;

        backBtn.SetActive(true);
        if(selectedQuest.IsAccepted == true)
        {
            completeBtn.SetActive(true);
        }
        else
        {
            acceptBtn.SetActive(true);
        }
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        string objectives = string.Empty;
        string reward = string.Empty;

        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        if (quest.MyReward.MyItems.Length != 0)
        {
            foreach (Item item in quest.MyReward.MyItems)
            {
                reward += item.type.ToString().ToLower() + "\n";
            }
        }
        if (quest.MyReward.MyGold != 0)
        {
            reward += "Oro: " + quest.MyReward.MyGold + "\n";
        }
        if (quest.MyReward.MyExp != 0)
        {
            reward += "Exp: " + quest.MyReward.MyExp + "\n";
        }

        questDescription.GetComponent<Text>().text = string.Format("<b>{0}</b>\n<size=12>{1}</size>\n\nRecompensa\n<size=12>{2}</size>", quest.MyTitle, quest.MyDescription,reward);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        completeBtn.SetActive(false);
        ShowQuests(questGiver);
    }

    public void Accept()
    {
        Questlog.MyInstance.AcceptQuest(selectedQuest);
        questGiver.QuestAccepted(selectedQuest);
        Back();
    }

    public void Complete()
    {
       if(selectedQuest.IsComplete == true)
        {
            questGiver.CompleteQuest(selectedQuest);
            Back();
        }
    }
}
