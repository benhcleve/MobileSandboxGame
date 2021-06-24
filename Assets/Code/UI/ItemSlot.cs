using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public enum SlotType { Generic, Inventory, Hotbar, Storage, Shop, CraftOutput }
    public SlotType slotType = SlotType.Generic;
    GameObject stackUI;
    GameObject fillUI;
    TextMeshProUGUI stackCountText;
    Image fillBar;
    Button button;
    public Image icon;
    public Item currentItem;
    public int inventoryIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        //These components are found by NAME. Do not change names of children in Item Slot gameobject.
        icon = transform.Find("Icon").GetComponent<Image>();
        stackUI = transform.Find("Stack UI").gameObject;
        stackCountText = stackUI.transform.Find("Stack Count").GetComponent<TextMeshProUGUI>();
        fillUI = transform.Find("Fill UI").gameObject;
        fillBar = fillUI.transform.Find("Fill Bar").GetComponent<Image>();
        icon.gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateItemSlot();

        //Set inventory index
        if (slotType == SlotType.Inventory || slotType == SlotType.Hotbar)
            inventoryIndex = PlayerInventory.instance.inventorySlots.IndexOf(this);
    }
    private void Update()
    {
        if (slotType == SlotType.Inventory || slotType == SlotType.Hotbar)
            if (currentItem != PlayerInventory.instance.inventory[inventoryIndex] || PlayerInventory.instance.inventory[inventoryIndex] == null && icon.gameObject.activeInHierarchy)
                UpdateItemSlot();
    }



    public void UpdateItemSlot()
    {
        button.onClick.RemoveAllListeners();

        //Update current item to inventory instance value
        if (slotType == SlotType.Inventory || slotType == SlotType.Hotbar)
        {
            int itemIndex = PlayerInventory.instance.inventorySlots.IndexOf(this);
            Item invItem = PlayerInventory.instance.inventory[itemIndex];
            currentItem = invItem;
        }

        if (currentItem == null)
        {
            icon.gameObject.SetActive(false);
            stackUI.SetActive(false);
            fillUI.SetActive(false);
            return;
        }

        currentItem.currentSlot = this;
        icon.gameObject.SetActive(true);
        icon.sprite = currentItem.icon;
        stackUI.SetActive(currentItem.stackable);
        fillUI.SetActive(currentItem.usesFill);
        if (currentItem.stackable)
            stackCountText.text = currentItem.stackCount.ToString();
        if (currentItem.usesFill)
            fillBar.fillAmount = currentItem.fill;

        SwitchSlotFunction();
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
                stackUI.SetActive(false);
                fillUI.SetActive(false);
                break;
            case SlotType.Storage:
                break;
        }
    }

}

