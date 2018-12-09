using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour
{

    private RectTransform inventoryRect;

    private float inventoryWidth, inventoryHight;

    public int slots;

    public int rows;

    public float slotPaddingLeft, slotPaddingTop;

    public float slotSize;

    public GameObject slotPrefab;

    private static Slot from, to;

    private List<GameObject> allSlots;

    public GameObject iconPrefab;

    private static GameObject hoverObject;

    private static int emptySlots;

    [SerializeField]
    private CanvasGroup canvasGroup;

    public Canvas canvas;

    private float hoverYOffset;

    public EventSystem eventSystem;

    public bool bagIsClose;

    private Image inventorySprite;

    public static int EmptySlots
    {
        get { return emptySlots; }
        set { emptySlots = value; }
    }

    private static Inventory instance;
    public static Inventory MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        inventorySprite = GetComponent<Image>();
        CreateLayout();
        Open();
        OpenClose();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null && from.CurrentItem.type.ToString().ToLower() != "aguasagrada")
            {
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
            }
        }

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            position.Set(position.x, position.y + hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void CreateLayout()
    {
        bagIsClose = true;

        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        emptySlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        inventoryHight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();

        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHight);

        int columns = slots / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);

                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                newSlot.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);
                //localPosition = relative to the parent position
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(-slotPaddingLeft * (x + 1) - (slotSize * x), slotPaddingTop * (y + 1) + (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                newSlot.GetComponent<Slot>().ResizeSlack();
                allSlots.Add(newSlot);
            }
        }
    }

    public bool AddItem(Item item)
    {
        Open();
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            Questlog.MyInstance.UpdateQuestItemCount(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty)
                {
                    if (tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        Questlog.MyInstance.UpdateQuestItemCount(item);
                        return true;
                    }
                }
            }
            if (emptySlots > 0)
            {
                PlaceEmpty(item);
                Questlog.MyInstance.UpdateQuestItemCount(item);
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("HUDCanvas").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count == 0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }

            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }

    public bool IsMovingItem()
    {
        if (hoverObject == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (GameObject slot in allSlots)
        {
            Slot tmp = slot.GetComponent<Slot>();

            if (!tmp.IsEmpty && tmp.CurrentItem.type.ToString().ToLower() == type.ToLower())
            {
                 itemCount += tmp.Items.Count;
            }
        }

        return itemCount;
    }

    public void DestroyItem(string type, int amount)
    {
        int itemCount = 0;
        foreach (GameObject slot in allSlots)
        {
            if (itemCount < amount)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if (!tmp.IsEmpty && tmp.CurrentItem.type.ToString().ToLower() == type.ToLower())
                {
                    itemCount += tmp.DestroyItem(amount - itemCount);
                }
            }
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        for (int i = 0; i < allSlots.Count; i++)
        {
            allSlots[i].SetActive(true);
        }
        bagIsClose = false;
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            canvasGroup.alpha = 0;
            bagIsClose = true;
            canvasGroup.blocksRaycasts = false;
            for (int i = 0; i < allSlots.Count; i++)
            {
                allSlots[i].SetActive(false);
            }
        }
        else
        {
            Open();
        }
    }
}
