using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageChest : Interactable
{
    Animator animator;
    public GameObject storageUI;

    void Start() => animator = GetComponent<Animator>();

    private void Update()
    {
        SetInteractionFalse();

        if (!isInteracting && animator.GetBool("isChestOpen"))
        {
            animator.SetBool("isChestOpen", false);
            storageUI.SetActive(false);
            UIManager.instance.uiState = UIManager.UIState.Default;
            PlayerInventory.instance.inventoryUI.SetActive(false);
        }

    }

    public override void Interact()
    {
        base.Interact();
        animator.SetBool("isChestOpen", true);
        storageUI.SetActive(true);
        PlayerInventory.instance.inventoryUI.SetActive(true);
        PlayerInventory.instance.UpdateSlots();
        UIManager.instance.uiState = UIManager.UIState.Default_NoMovement;
    }

    public void ExitButton() => PlayerInteraction.instance.target = null;

}
