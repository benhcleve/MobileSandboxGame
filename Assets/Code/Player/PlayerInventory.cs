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
        for (int i = 0; i < inventoryLimit; i++)
        {
            GameObject slot = Instantiate(slotPrefab);
            slot.transform.SetParent(inventoryUI.transform.Find("Inventory Slots"));
            slot.transform.localScale = Vector3.one;
            inventorySlots.Add(slot.GetComponent<ItemSlot>());
            slot.GetComponent<ItemSlot>().slotType = ItemSlot.SlotType.Inventory;
            slot.name = "Inventory Slot " + (i + 1);
        }
    }

    void UpdateSlots()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].currentItem = inventory[i];
            inventorySlots[i].UpdateItemSlot(inventory[i]);
        }
    }



}



