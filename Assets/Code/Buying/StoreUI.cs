using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject itemButton;

    void Start()
    {
        GenerateStock();
    }


    void GenerateStock()
    {
        foreach (Item item in saleStock)
        {
            GameObject buttonPref = Instantiate(itemButton);
            buttonPref.transform.SetParent(listingPanel.transform);
            buttonPref.transform.localScale = Vector3.one;
            buttonPref.GetComponent<ItemButton>().currentItemID = item.ID;
            StartCoroutine(addPlantListener(buttonPref.GetComponent<Button>()));
        }
    }

    IEnumerator addPlantListener(Button button)
    {
        yield return new WaitForSeconds(0.1f);
        button.onClick.AddListener(() => this.SelectItem(button.transform.GetComponent<ItemButton>().currentItem));
    }

    public void SelectItem(Item item)
    {
        purchasePanel.SetActive(true);
        listingPanel.SetActive(false);
        itemIcon.sprite = item.icon;
        buyCount = 1;

        if (item.stackable)
        {
            countPanel.SetActive(true);
            purchasePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 250);
        }

        else
        {
            countPanel.SetActive(false);
            purchasePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 250);
        }
    }

    public void IncreaseBuyCount()
    {
        buyCount++;
        buyCountDisplay.text = buyCount.ToString();
    }

    public void DecreaseBuyCount()
    {
        buyCount--;
        buyCountDisplay.text = buyCount.ToString();
    }
}
