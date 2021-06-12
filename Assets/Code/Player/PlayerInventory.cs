using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coins;
    public int inventoryLimit;
    public GameObject inventoryUI;
    public GameObject slotPrefab;
    public List<Item> inventory = new List<Item>(); //Hotbar items are 0-3
    public List<ItemSlot> inventorySlots = new List<ItemSlot>();
    private static PlayerInventory _instance;
    public static PlayerInventory instance { get { return _instance; } }

    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    public void Awake() => CreateInstance();

    private void Start()
    {
        CreateInventorySlots();
        UpdateSlots();
    }


    public bool isInventoryFull()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] == null)
                return false;
        }

        return true;
    }

    void CreateInventorySlots()
    {
        int slotNumber = 1;
        while (inventorySlots.Count < inventoryLimit)
        {
            inventory.Add(null); //Add room to inventory item list
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(inventoryUI.transform.Find("Inventory Slots"));
            slot.transform.localScale = Vector3.one;
            inventorySlots.Add(slot.GetComponent<ItemSlot>());
            slot.GetComponent<ItemSlot>().slotType = ItemSlot.SlotType.Inventory;
            slot.name = "Inventory Slot " + slotNumber;
            slotNumber++;
        }
    }

    public void UpdateSlots()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventorySlots[i].gameObject.activeInHierarchy)
            {
                inventorySlots[i].currentItem = inventory[i];

                if (inventory[i] != null)
                    if (inventory[i].stackable && inventory[i].stackCount < 1)
                    {
                        Destroy(inventory[i]);
                        inventory[i] = null;
                        inventorySlots[i].currentItem = null;
                    }

                inventorySlots[i].UpdateItemSlot();
            }
        }
    }

    public void OpenCloseInventory()
    {
        if (!inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(true);
            UpdateSlots();
        }
        else inventoryUI.SetActive(false);
    }

    public void AddToInventory(Item selectedItem, GameObject destroyMe = null)
    {
        int freeInvSlotIndex = -1;
        foreach (Item item in inventory) //find first free slot index in inventory, or inventory is full
        {
            if (item == null)
                freeInvSlotIndex = inventory.IndexOf(item);
        }

        //If item is not stackable and there is space, create new instance in inventory
        if (!selectedItem.stackable && freeInvSlotIndex != -1)
        {
            Item newItem = Object.Instantiate(selectedItem);
            inventory[freeInvSlotIndex] = newItem;
        }
        else
        {
            //If stackable item already exists in inventory, add to current stack
            foreach (Item item in PlayerInventory.instance.inventory)
            {
                if (item != null && item.ID == selectedItem.ID)
                {
                    item.stackCount += selectedItem.stackCount;
                    PlayerInventory.instance.UpdateSlots();
                    if (freeInvSlotIndex != -1 && destroyMe != null) //Destroys gameobject that is being picked up
                        Destroy(destroyMe);
                    return;
                }
            }

            //If no item in inventory with this ID exists and there is space, create new stack
            if (freeInvSlotIndex != -1)
            {
                Item newItem = Object.Instantiate(selectedItem);
                PlayerInventory.instance.inventory[freeInvSlotIndex] = newItem;
                newItem.stackCount = selectedItem.stackCount;
            }
        }
        if (freeInvSlotIndex == -1)
            Debug.Log("Inventory is full!");
        if (freeInvSlotIndex != -1 && destroyMe != null) //Destroys gameobject that is being picked up
            Destroy(destroyMe);

        PlayerInventory.instance.UpdateSlots();
    }

}



