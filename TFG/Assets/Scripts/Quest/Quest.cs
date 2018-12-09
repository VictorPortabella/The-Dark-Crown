using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest{

    [SerializeField]
    private string title;

    [TextArea(3, 10)]
    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives;

    [SerializeField]
    private Reward reward;

    private bool isComplete = false;

    private bool accepted = false;

    public bool IsAccepted
    {
        get
        {
            return accepted;
        }
        set
        {
            accepted = value;
        }
    }

    public string MyDescription
    {
        get { return description; }
        set { description = value; }
    }

    public CollectObjective[] MyCollectObjectives
    {
        get { return collectObjectives; }
    }

    public Reward MyReward
    {
        get { return reward; }
    }

    public bool IsComplete
    {
        get
        {
            foreach(Objective o in collectObjectives)
            {
                if (!o.IsComplete)
                {
                    isComplete = false;
                    return false;
                }
            }
            isComplete = true;
            return true;
        }
    }

    public QuestScript MyQuestScript { get; set; } 

    public string MyTitle
    {
        get { return title;}
        set { title = value;}
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount
    {
        get { return amount; }
    }

    public int MyCurrentAmount
    {
        get { return currentAmount; }
        set { currentAmount = value; }
    }

    public string MyType
    {
        get { return type; }
    }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToString().ToLower() == item.type.ToString().ToLower())
        {
            MyCurrentAmount = Inventory.MyInstance.GetItemCount(item.type.ToString());
            Questlog.MyInstance.UpdateSelected();
            Questlog.MyInstance.CheckCompletion();
        }
    }
}

[System.Serializable]
public class Reward
{
    [SerializeField]
    private int gold;

    [SerializeField]
    private int exp;

    [SerializeField]
    public Item[] items;

    public int MyGold
    {
        get { return gold; }
    }

    public int MyExp
    {
        get { return exp; }
    }

    public Item[] MyItems
    {
        get { return items; }
    }
}