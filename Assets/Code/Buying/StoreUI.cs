using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public List<Item> saleStock = new List<Item>();
    int buyCount = 1;
    public TextMeshProUGUI buyCountDisplay;
    public Image itemIcon;
    public GameObject countPanel;
    public GameObject listingPanel;
    public GameObject purchasePanel;
    public TextMeshProUGUI price;
    public GameObject itemButton;
    public Item selectedItem;
    List<GameObject> allSlots = new List<GameObject>();



    void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
            DetectTouch();
    }

    Vector2 touchStartPos;
    void DetectTouch()
    {
        if (Input.touchCount == 1)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;
            float touchMoveDist = 0;
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                touchStartPos = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance

                if (touchMoveDist < 50) //If 1 touch tap
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    // You successfully hit
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == this.transform) //If not interactable, set target to null
                            if (Vector3.Distance(PlayerMovement.instance.transform.position, this.transform.position) < 5) //If player is near
                            {
                                transform.Find("Store Canvas").gameObject.SetActive(true);
                                GenerateStock();
                            }

                    }
                }
            }
        }
    }


    void GenerateStock()
    {
        foreach (Item item in saleStock)
        {
            GameObject buttonPref = Instantiate(itemButton);
            buttonPref.transform.SetParent(listingPanel.transform.Find("Listing Panel Slots"));
            buttonPref.transform.localScale = Vector3.one;
            buttonPref.GetComponent<ItemSlot>().currentItem = item;
            buttonPref.GetComponent<ItemSlot>().slotType = ItemSlot.SlotType.Shop;
            allSlots.Add(buttonPref);
            StartCoroutine(addPlantListener(buttonPref.GetComponent<Button>()));
        }
    }

    IEnumerator addPlantListener(Button button)
    {
        yield return new WaitForSeconds(0.1f);
        button.onClick.AddListener(() => this.SelectItem(button.transform.GetComponent<ItemSlot>().currentItem));
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;
        purchasePanel.SetActive(true);
        listingPanel.SetActive(false);
        itemIcon.sprite = item.icon;
        buyCount = 1;
        buyCountDisplay.text = buyCount.ToString();
        price.text = item.value.ToString();

        if (item.stackable)
        {
            countPanel.SetActive(true);
            purchasePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 300);
        }

        else
        {
            countPanel.SetActive(false);
            purchasePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 300);
        }
    }

    public void IncreaseBuyCount()
    {
        buyCount++;
        buyCountDisplay.text = buyCount.ToString();
        price.text = (selectedItem.value * buyCount).ToString();
    }

    public void DecreaseBuyCount()
    {
        if (buyCount >= 2)
            buyCount--;
        buyCountDisplay.text = buyCount.ToString();
        price.text = (selectedItem.value * buyCount).ToString();
    }

    public void BuyItem()
    {
        if (PlayerInventory.instance.coins >= selectedItem.value)
        {
            int freeInvSlotIndex = -1;
            foreach (Item item in PlayerInventory.instance.inventory) //find first free slot index in inventory, or inventory is full
            {
                if (item == null)
                    freeInvSlotIndex = PlayerInventory.instance.inventory.IndexOf(item);
            }

            //If item is not stackable and there is space, create new instance in inventory
            if (!selectedItem.stackable && freeInvSlotIndex != -1)
            {
                Item purchasedItem = Object.Instantiate(selectedItem);
                PlayerInventory.instance.inventory[freeInvSlotIndex] = purchasedItem;
                PlayerInventory.instance.coins -= selectedItem.value;
            }
            else
            {
                //If stackable item already exists in inventory, add to current stack
                foreach (Item item in PlayerInventory.instance.inventory)
                {
                    if (item != null && item.ID == selectedItem.ID)
                    {
                        item.stackCount += buyCount;
                        PlayerInventory.instance.coins -= selectedItem.value * buyCount;
                        return;
                    }
                }

                //If no item in inventory with this ID exists and there is space, create new stack
                if (freeInvSlotIndex != -1)
                {
                    Item purchasedItem = Object.Instantiate(selectedItem);
                    PlayerInventory.instance.inventory[freeInvSlotIndex] = purchasedItem;
                    purchasedItem.stackCount = buyCount;
                    PlayerInventory.instance.coins -= selectedItem.value * buyCount;
                }

            }
            if (freeInvSlotIndex == -1)
                Debug.Log("Inventory is full!");
        }

        PlayerInventory.instance.UpdateSlots();
    }

    public void ExitShop()
    {
        foreach (GameObject slot in allSlots)
            Destroy(slot);
        allSlots.Clear();
        transform.Find("Store Canvas").gameObject.SetActive(false);

    }
}
