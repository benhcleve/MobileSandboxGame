using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;


public class PlantRadialMenu : MonoBehaviour
{
    public GameObject ItemButtonPrefab;
    List<GameObject> Buttons;
    RectTransform canvasRect;
    float radius = 35f;
    public Button exitButton;

    private void OnEnable()
    {
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
        exitButton.GetComponent<RectTransform>().DOScale(Vector3.one, .1f).SetEase(Ease.OutQuad);
        canvasRect = GetComponent<RectTransform>();
        Buttons = new List<GameObject>();
        PositionUI();
        AddButtons();
        Rearrange();
    }

    private void PositionUI()
    {
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.root.position);
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        // Set
        exitButton.transform.localPosition = canvasPos;
    }

    void AddButtons()
    {

        foreach (Item item in PlayerInventory.instance.inventory)
        {
            if (item != null)
            {
                if (item.ID > 7000 && item.ID < 8000) //Seed IDs are the 7000's
                {
                    GameObject entry = Instantiate(ItemButtonPrefab, transform);
                    entry.GetComponent<ItemSlot>().currentItem = item;
                    entry.GetComponent<ItemSlot>().UpdateItemSlot();
                    entry.transform.SetParent(exitButton.transform);
                    entry.transform.localPosition = Vector3.zero;
                    Buttons.Add(entry);
                    StartCoroutine(addPlantListener(entry.GetComponent<Button>()));
                }
            }
        }
    }

    IEnumerator addPlantListener(Button button)
    {
        yield return new WaitForSeconds(0.1f);
        button.onClick.AddListener(() => this.PlantSeed(button.transform.GetComponent<ItemSlot>().currentItem.ID));
    }

    void Rearrange()
    {
        radius = (10 * Buttons.Count) + 75; //Increase radius based on amount of buttons
        float radiansOfSeparation = (Mathf.PI * 2) / Buttons.Count;

        for (int i = 0; i < Buttons.Count; i++)
        {
            float x = Mathf.Sin(radiansOfSeparation * i) * radius;
            float y = Mathf.Cos(radiansOfSeparation * i) * radius;

            RectTransform rect = Buttons[i].GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad).SetDelay(0.05f * i);
            rect.DOAnchorPos(new Vector3(x, y, 0), .3f).SetEase(Ease.OutQuad).SetDelay(.05f * i);

        }
    }

    public void ExitRadial() => StartCoroutine(ExitRadialCo());

    IEnumerator ExitRadialCo()
    {
        exitButton.GetComponent<RectTransform>().DOScale(Vector3.zero, .1f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(.1f);

        for (int i = 0; i < Buttons.Count; i++)
            Destroy(Buttons[i]);

        UIManager.instance.uiState = UIManager.UIState.Default;
        gameObject.SetActive(false);
    }

    public void PlantSeed(int id)
    {
        Item[] items = Resources.LoadAll<ItemSeeds>("Items/Seeds").ToArray();
        foreach (Item i in items)
            if (i.ID == id)
            {
                GameObject seedPrefab = Instantiate(i.prefab, transform.root.position, transform.root.rotation);
                seedPrefab.transform.parent = transform.root;
                transform.root.GetComponent<Soil>().currentCrop = seedPrefab;

                foreach (Item item in PlayerInventory.instance.inventory)
                {
                    if (item != null && item.ID == id)
                    {
                        item.stackCount--;
                        PlayerInventory.instance.UpdateSlots();
                        break;
                    }
                }
            }
        ExitRadial();
    }

}
