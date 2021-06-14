using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageChest : Interactable
{
    Animator animator;
    void Start() => animator = GetComponent<Animator>();

    private void Update()
    {
        SetInteractionFalse();

        if (!isInteracting && animator.GetBool("isChestOpen"))
            animator.SetBool("isChestOpen", false);
    }

    public override void Interact()
    {
        base.Interact();
        animator.SetBool("isChestOpen", true);
    }

}
