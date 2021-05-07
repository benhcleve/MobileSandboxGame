using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    public bool isInventory;
    public GameObject stackUI;
    public TextMeshProUGUI stackCountText;
    public Button button;
    public int currentItemID;
    public Image icon;

    public Item currentItem;

    void Start()
    {
        button = GetComponent<Button>();

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

                if (isInventory)
                    button.onClick.AddListener(delegate { currentItem.Use(); });
            }
    }

}

