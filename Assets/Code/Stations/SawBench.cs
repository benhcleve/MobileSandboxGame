using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SawBench : Interactable
{
    [SerializeField] Image progressBar;
    Animator animator;
    public ItemSlot inputslot;
    public ItemSlot outputSlot;
    public GameObject sawBenchUI;
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

        while (sawProgress < 1f)
        {
            sawProgress += Time.deltaTime;
            progressBar.fillAmount = sawProgress;
            yield return new WaitForEndOfFrame();
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
