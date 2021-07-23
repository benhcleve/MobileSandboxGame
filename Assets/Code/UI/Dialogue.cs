using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;
    public delegate void AfterDialogue();

    public void Write(string[] dialogue, AfterDialogue afterDialogue = null) => StartCoroutine(PlayText(dialogue, afterDialogue));

    IEnumerator PlayText(string[] dialogue, AfterDialogue afterDialogue)
    {
        UIManager.instance.UpdateUIManager(UIManager.UIState.Dialogue);

        foreach (string line in dialogue)
        {

            text.text = "";

            foreach (char c in line)
            {
                text.text += c;
                yield return new WaitForSeconds(.01f);
            }

            yield return new WaitUntil(() => Input.touchCount == 1 || Input.GetMouseButtonDown(0));
        }

        //End dialogue by closing ui and setting text to empty
        text.text = "";

        UIManager.instance.UpdateUIManager(UIManager.UIState.Default);

        if (afterDialogue != null)
            afterDialogue();

    }


}
