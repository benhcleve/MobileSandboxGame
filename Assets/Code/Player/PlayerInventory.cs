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
        inventoryUI.SetActive(false);
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


}



