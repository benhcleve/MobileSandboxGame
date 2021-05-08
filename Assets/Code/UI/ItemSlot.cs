using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType { Generic, Inventory, Hotbar, Shop }
    public SlotType slotType = SlotType.Generic;
    GameObject stackUI;
    TextMeshProUGUI stackCountText;
    Button button;
    Image icon;
    public int currentItemID;
    public Item currentItem;

    void Start()
    {
        button = GetComponent<Button>();
        //These components are found by NAME. Do not change names of children in Item Slot gameobject.
        icon = transform.Find("Icon").GetComponent<Image>();
        stackUI = transform.Find("Stack UI").gameObject;
        stackCountText = stackUI.transform.Find("Stack Count").GetComponent<TextMeshProUGUI>();
        icon.gameObject.SetActive(false);

        UpdateItemButton(currentItemID);
    }

    public void UpdateItemButton(int ID)
    {
        button.onClick.RemoveAllListeners();

        if (ID == 0)
        {
            icon.gameObject.SetActive(false);
        }

        Item[] items = Resources.LoadAll<Item>("Items").ToArray();
        foreach (Item i in items)
            if (i.ID == ID)
            {
                currentItem = Object.Instantiate(i);
                currentItemID = ID;
                icon.gameObject.SetActive(true);
                icon.sprite = currentItem.icon;
                stackUI.SetActive(currentItem.stackable);
                if (currentItem.stackable)
                    stackCountText.text = currentItem.stackCount.ToString();

                SwitchSlotFunction();
            }
    }

    void SwitchSlotFunction()
    {
        switch (slotType)
        {
            case SlotType.Generic:
                break;

            case SlotType.Inventory:
                break;

            case SlotType.Hotbar:
                button.onClick.AddListener(delegate { currentItem.Use(); });
                break;
            case SlotType.Shop:

                break;
        }
    }

}

