using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

    private Stack<Item> items;
    
    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    public Text stackTxt;

    public Sprite slotEmpty;
    public Sprite slotHighlight;

    public bool IsEmpty
    {
        get { return items.Count == 0; }
    }

    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > items.Count; }
    }

    public Item CurrentItem
    {
        get { return items.Peek(); }
    }

    RectTransform slotRect;
    RectTransform txtRect;
    // Use this for initialization
    void Awake () {
        items = new Stack<Item>();

        slotRect = GetComponent<RectTransform>();
        txtRect = stackTxt.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void AddItem(Item item)
    {
        items.Push(item);

        if(items.Count > 1)
        {
            stackTxt.text = items.Count.ToString();
        }

        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);

        stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);

    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

    private void UseItem()
    {
        if (!IsEmpty)
        {
            if (items.Peek().type.ToString().ToLower() != "aguasagrada")
            {
                items.Pop().Use();

                stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
                    ChangeSprite(slotEmpty, slotHighlight);
                    Inventory.EmptySlots++;
                }
            }
        }
    }

    public void ClearSlot()
    {
        Item tmp = items.Peek();
        items.Clear();
        Questlog.MyInstance.UpdateQuestItemCount(tmp);
        ChangeSprite(slotEmpty, slotHighlight);
        stackTxt.text = string.Empty;
        Inventory.EmptySlots++;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    public void ResizeSlack() {
        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        stackTxt.resizeTextMaxSize = txtScaleFactor;
        stackTxt.resizeTextMinSize = txtScaleFactor;
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
    }

    public int DestroyItem(int amount)
    {
        int itemDestroyed = 0;
        for (int i = 0; i < amount; i++)
        {
            if (!IsEmpty && itemDestroyed < amount)
            {
                items.Pop();
                itemDestroyed++;
                stackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;

                if (IsEmpty)
                {
                    ChangeSprite(slotEmpty, slotHighlight);
                    Inventory.EmptySlots++;
                }
            }
        }
        return itemDestroyed;
    }
}
