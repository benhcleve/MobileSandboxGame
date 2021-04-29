using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Button button;
    public int currentItemID;
    public Image icon;
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
                currentItemID = ID;
                icon.gameObject.SetActive(true);
                icon.sprite = i.icon;

                button.onClick.AddListener(delegate () { i.Use(); });
            }
    }

}

