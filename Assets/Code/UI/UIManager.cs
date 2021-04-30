using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager instance { get { return _instance; } }

    public GameObject dialogueUI;
    public GameObject objectPlacementUI;
    public GameObject itemBar;

    private void Awake() => CreateInstance();

    void CreateInstance() //Make this UI Manager an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public bool uiActive()
    {
        if (dialogueUI.activeInHierarchy || objectPlacementUI.activeInHierarchy)
            return true;
        else return false;
    }

    public void WriteDialogue(string[] dialogue)
    {
        dialogueUI.SetActive(true);
        dialogueUI.GetComponent<Dialogue>().Write(dialogue);
    }

}
