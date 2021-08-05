using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{

    public GameObject bedUI;

    public override void Interact()
    {
        base.Interact();
        UIManager.instance.UpdateUIManager(UIManager.UIState.Sleeping);
        PlayerAnimation.instance.SetAnimation("isSleeping");
        PlayerMovement.instance.navMeshAgent.updateRotation = false;
        PlayerMovement.instance.transform.localRotation = transform.rotation;
        PlayerMovement.instance.transform.position = transform.position;
        Time.timeScale = 20;
        bedUI.SetActive(true);

    }

    public void WakeUp()
    {
        UIManager.instance.UpdateUIManager(UIManager.UIState.Default);
        PlayerAnimation.instance.SetAnimation("isIdle");
        bedUI.SetActive(false);
        PlayerInteraction.instance.target = null;
        PlayerMovement.instance.navMeshAgent.updateRotation = true;
        Time.timeScale = 1;
    }
}
