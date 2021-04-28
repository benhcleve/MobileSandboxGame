using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBox : Interactable
{
    private void Awake() => interactDistance = 2; //Set interaction distance
    public override void Interact()
    {
        base.Interact();
        UIManager.instance.WriteDialogue(new string[] { "You are interacting with the box...", "Great job!", "I could do this all day", "Just line after line..." });
    }

}
