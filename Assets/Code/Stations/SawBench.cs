using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SawBench : Interactable
{
    [SerializeField] Image progressBar;
    [SerializeField] GameObject exitButton;
    [SerializeField] GameObject SawButton;
    [SerializeField] GameObject selectionOutline;
    Animator animator;
    public ItemSlot outputSlot;
    public GameObject sawBenchUI;

    public GameObject[] selectionButtons;
    public Item selectedCraftable;



    bool isSawing;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        SetInteractionFalse();

        if (!isInteracting && sawBenchUI.activeInHierarchy)
        {
            sawBenchUI.SetActive(false);
            UIManager.instance.uiState = UIManager.UIState.Default;
        }
    }

    public override void Interact()
    {
        base.Interact();
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
        PlayerInventory.instance.inventoryUI.SetActive(true);
        sawBenchUI.SetActive(true);
    }

    public void StartSawing() => StartCoroutine(Saw());

    IEnumerator Saw()
    {
        float sawProgress = 0;
        exitButton.SetActive(false);
        SawButton.SetActive(false);


        while (sawProgress < 1f)
        {
            sawProgress += Time.deltaTime;
            progressBar.fillAmount = sawProgress;
            yield return new WaitForEndOfFrame();
        }
        exitButton.SetActive(true);
        SawButton.SetActive(true);

    }

    public void SelectCraftable(string itemName)
    {
        switch (itemName)
        {
            case "Oak":
                selectionOutline.transform.position = selectionButtons[0].transform.position;
                // ** Make plank of tree type selectedCraftable
                break;
            case "Pine":
                selectionOutline.transform.position = selectionButtons[1].transform.position;
                // ** Make plank of tree type selectedCraftable
                break;
            case "Cedar":
                selectionOutline.transform.position = selectionButtons[2].transform.position;
                // ** Make plank of tree type selectedCraftable
                break;
            case "Birch":
                selectionOutline.transform.position = selectionButtons[3].transform.position;
                // ** Make plank of tree type selectedCraftable
                break;
        }
    }

    public void ExitSawBench()
    {
        StopAllCoroutines();
        PlayerInteraction.instance.target = null;
        UIManager.instance.uiState = UIManager.UIState.Default;
        PlayerInventory.instance.inventoryUI.SetActive(false);
        sawBenchUI.SetActive(false);
    }
}
