using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Write(string[] dialogue) => StartCoroutine(PlayText(dialogue));

    IEnumerator PlayText(string[] dialogue)
    {
        UIManager.instance.itemBar.SetActive(false); //Hide ItemBar UI

        foreach (string line in dialogue)
        {

            text.text = "";

            foreach (char c in line)
            {
                text.text += c;
                yield return new WaitForSeconds(.005f);
            }

            yield return new WaitUntil(() => Input.touchCount == 1);
        }

        //End dialogue by closing ui and setting text to empty
        text.text = "";

        UIManager.instance.itemBar.SetActive(true); //Bring back ItemBar UI

        UIManager.instance.dialogueUI.SetActive(false);
    }


}
