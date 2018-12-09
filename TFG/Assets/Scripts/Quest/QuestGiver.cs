using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestGiver : NPC {

    [SerializeField]
    private List<Quest> quests;

    public List<Quest> MyQuests
    {
        get
        {
            return quests;
        }
    }

    //Debugging
    [SerializeField]
    private Questlog tmpLog;

    private SphereCollider visionQuestGiver;
    private bool playerIn = false;

    public Canvas questGiverCanvas;

    private void Start()
    {
        visionQuestGiver = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(playerIn == true && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIn = false;
            StopInteract();
        }
    }

    public void QuestAccepted(Quest quest)
    {
        foreach (Quest q in quests)
        {
            if (q == quest)
            {
                q.IsAccepted = true;
            }
        }

    }

    private void DestroyQuest(Quest quest)
    {
        quests.Remove(quest);
        Questlog.MyInstance.RemoveQuest(quest);
        if(quests.Count == 0)
        {
            Destroy(questGiverCanvas);
        }
    }

    public void CompleteQuest(Quest quest)
    {
        foreach(CollectObjective co in quest.MyCollectObjectives)
        {
            Inventory.MyInstance.DestroyItem(co.MyType, co.MyAmount);
        }

        if (quest.MyReward.MyItems.Length != 0)
        {
            foreach (Item item in quest.MyReward.MyItems)
            {
                Inventory.MyInstance.AddItem(item);
            }
        }
        if (quest.MyReward.MyGold != 0)
        {
            PlayerStats.MyInstance.GetGold(quest.MyReward.MyGold);
        }
        if (quest.MyReward.MyExp != 0)
        {
            PlayerStats.MyInstance.GrantExperience(quest.MyReward.MyExp);
        }
        this.DestroyQuest(quest);

        Questlog.MyInstance.UpdateNumberQuests();
    }
}
