﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance { get { return _instance; } }

    public GameObject dialogueUI;
    public GameObject objectPlacementUI;
    public GameObject hotbar;

    public enum UIState { Default, Default_NoMovement, Placement, Dialogue, Minigame };
    public UIState uiState = UIState.Default;

    private void Awake() => CreateInstance();

    private void Start() => UpdateUIManager(UIState.Default);
    void CreateInstance() //Make this UI Manager an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    public void WriteDialogue(string[] dialogue, Dialogue.AfterDialogue afterDialogue = null)
    {
        dialogueUI.SetActive(true);
        dialogueUI.GetComponent<Dialogue>().Write(dialogue, afterDialogue);
    }

    public void UpdateUIManager(UIState state)
    {
        uiState = state;
        switch (uiState)
        {
            case UIState.Default:
                dialogueUI.SetActive(false);
                objectPlacementUI.SetActive(false);
                hotbar.SetActive(true);
                PlayerInventory.instance.UpdateSlots();
                break;
            case UIState.Default_NoMovement:
                dialogueUI.SetActive(false);
                objectPlacementUI.SetActive(false);
                hotbar.SetActive(true);
                PlayerInventory.instance.UpdateSlots();
                break;
            case UIState.Placement:
                dialogueUI.SetActive(false);
                objectPlacementUI.SetActive(true);
                hotbar.SetActive(false);
                break;
            case UIState.Dialogue:
                dialogueUI.SetActive(true);
                objectPlacementUI.SetActive(false);
                hotbar.SetActive(false);
                break;
        }
    }

}
