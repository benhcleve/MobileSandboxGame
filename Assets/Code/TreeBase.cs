using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeBase : Interactable
{
    public int hitpoints;
    public GameObject treeSeed;
    public GameObject woodDrop;
    public GameObject textPopup;

    void Update()
    {
        //Called from base class Interactable
        SetInteractionFalse();
    }

    public void TakeDamage(int damage, int damageType)
    {
        hitpoints -= damage;

        //0 idle, 1 weak, 2 hit, 3 critical
        switch (damageType)
        {
            case 0:
                TextPopupManager.instance.GeneratePopUp(damage.ToString(), new Color32(0, 0, 0, 255), new Vector3(0.8f, 0.8f, 0.8f), TextPopup.Effect.Damage, 2, transform, new Vector3(0, 2, 0));
                break;
            case 1:
                TextPopupManager.instance.GeneratePopUp(damage.ToString(), new Color32(159, 132, 49, 255), Vector3.one, TextPopup.Effect.Damage, 2, transform, new Vector3(0, 2, 0));
                break;
            case 2:
                TextPopupManager.instance.GeneratePopUp(damage.ToString(), new Color32(76, 159, 49, 255), Vector3.one, TextPopup.Effect.Damage, 2, transform, new Vector3(0, 2, 0));

                break;
            case 3:
                TextPopupManager.instance.GeneratePopUp(damage.ToString(), new Color32(228, 138, 21, 255), new Vector3(1.5f, 1.5f, 1.5f), TextPopup.Effect.Damage, 2, transform, new Vector3(0, 2, 0));
                break;
        }
    }




}
