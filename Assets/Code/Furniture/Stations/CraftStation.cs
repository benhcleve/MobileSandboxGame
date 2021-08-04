using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CraftStation : Interactable
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject craftButton;
    Animator animator;
    public ItemSlot outputSlot;
    public GameObject recipePanelIcons;
    public GameObject recipeIcon;
    public GameObject craftingUI;
    public Sprite checkmark;
    public Sprite xmark;
    public Image recipeCheckImage;

    public GameObject[] selectionButtons;
    public Item selectedCraftable;
    public bool isCrafting;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        SetInteractionFalse();

        if (!isInteracting && craftingUI.activeInHierarchy)
        {
            craftingUI.SetActive(false);
            UIManager.instance.uiState = UIManager.UIState.Default;
        }

        if (outputSlot.currentItem != null || isCrafting || selectedCraftable != null && !HasCraftingMaterials() || selectedCraftable == null)
            craftButton.SetActive(false);
        else craftButton.SetActive(true);
    }

    public override void Interact()
    {
        base.Interact();
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
        PlayerInventory.instance.inventoryUI.SetActive(true);
        PlayerInventory.instance.UpdateSlots();
        craftingUI.SetActive(true);
        selectedCraftable = null;
    }

    bool HasCraftingMaterials()
    {
        //Loop through crafting recipe items needed
        for (int i = 0; i < selectedCraftable.craftingRecipeItems.Count(); i++)
        {
            int inventoryCount = 0;
            Item recipeItem = selectedCraftable.craftingRecipeItems[i];

            //Look for item in inventory
            foreach (Item item in PlayerInventory.instance.inventory)
            {
                if (item != null && item.ID == recipeItem.ID) //Match by ID
                {
                    if (!item.stackable)
                        inventoryCount++;
                    else if (item.stackable)
                        inventoryCount += item.stackCount;
                }
            }
            //Return false if inventory count does not meet required count
            if (inventoryCount < selectedCraftable.craftingRecipeItemCount[i])
                return false;
        }

        //If all items meet requirements
        return true;
    }

    public void SelectCraftable(int itemID)
    {

        //Get the item by its item ID from recources folder
        Item[] items = Resources.LoadAll<Item>("Items/Craftable").ToArray();
        foreach (Item i in items)
            if (i.ID == itemID)
            {
                selectedCraftable = i;
            }

        foreach (Transform child in recipePanelIcons.transform) //Remove any previous icons
            Destroy(child.gameObject);

        for (int i = 0; i < selectedCraftable.craftingRecipeItems.Count(); i++)
        {
            GameObject icon = Instantiate(recipeIcon);
            icon.transform.SetParent(recipePanelIcons.transform);
            icon.transform.localScale = Vector3.one;
            icon.transform.Find("Icon").GetComponent<Image>().sprite = selectedCraftable.craftingRecipeItems[i].icon;
            icon.transform.Find("Count").GetComponent<TextMeshProUGUI>().text = selectedCraftable.craftingRecipeItemCount[i].ToString();
            recipeCheckImage.gameObject.SetActive(true);

            if (HasCraftingMaterials())
                recipeCheckImage.sprite = checkmark;
            else recipeCheckImage.sprite = xmark;
        }
    }

    public void StartCrafting() => StartCoroutine(Craft());

    IEnumerator Craft()
    {
        float craftProgress = 0;
        exitButton.SetActive(false);
        isCrafting = true;
        animator.SetBool("isCrafting", true);

        while (craftProgress < 1f)
        {
            craftProgress += Time.deltaTime;
            progressBar.fillAmount = craftProgress;
            yield return new WaitForEndOfFrame();
        }
        isCrafting = false;
        animator.SetBool("isCrafting", false);
        CraftItem();
        exitButton.SetActive(true);

        if (HasCraftingMaterials())
            recipeCheckImage.sprite = checkmark;
        else recipeCheckImage.sprite = xmark;
    }


    void CraftItem()
    {
        if (HasCraftingMaterials())
        {
            outputSlot.currentItem = Instantiate(selectedCraftable);
            if (outputSlot.currentItem.stackable) //If stackable, use craftOutputStackCount for stack count
                outputSlot.currentItem.stackCount = outputSlot.currentItem.craftOutputStackCount;
            outputSlot.UpdateItemSlot();

            for (int i = 0; i < selectedCraftable.craftingRecipeItems.Count(); i++)
            {
                int itemsNeeded = selectedCraftable.craftingRecipeItemCount[i];
                Item recipeItem = selectedCraftable.craftingRecipeItems[i];

                for (int n = 0; n < PlayerInventory.instance.inventory.Count(); n++)
                {
                    var playerInv = PlayerInventory.instance.inventory;

                    if (playerInv[n] != null && playerInv[n].ID == recipeItem.ID) //Match by ID
                    {
                        if (!playerInv[n].stackable)
                        {
                            playerInv[n] = null;
                            itemsNeeded--;
                        }
                        else if (playerInv[n].stackable)
                        {
                            if (playerInv[n].stackCount > itemsNeeded)
                            {
                                playerInv[n].stackCount -= itemsNeeded;
                                itemsNeeded = 0;
                            }
                            else if (playerInv[n].stackCount <= itemsNeeded)
                            {
                                itemsNeeded -= playerInv[n].stackCount;
                                playerInv[n].stackCount = 0;
                            }


                        }
                    }
                    if (itemsNeeded == 0)
                        break;
                }
            }
            PlayerInventory.instance.UpdateSlots();
        }
    }

    public void ExitCraftStation()
    {
        StopAllCoroutines();
        foreach (Transform child in recipePanelIcons.transform) //Remove ingredient icons
            Destroy(child.gameObject);

        PlayerInteraction.instance.target = null;
        UIManager.instance.uiState = UIManager.UIState.Default;
        recipeCheckImage.gameObject.SetActive(false);
        PlayerInventory.instance.inventoryUI.SetActive(false);
        craftingUI.SetActive(false);
    }
}
