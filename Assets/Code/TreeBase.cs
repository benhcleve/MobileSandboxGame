using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TreeBase : Interactable
{
    public int hitpoints;
    public GameObject treeSeed;
    public GameObject woodDrop;
    public GameObject treeStump;
    public bool fallen;

    void Update()
    {
        //Called from base class Interactable
        SetInteractionFalse();
    }

    public void TakeDamage(int damage, int damageType)
    {
        hitpoints -= damage;

        transform.DOShakePosition(.3f, .1f, 20, 90, false, true);

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

        if (hitpoints <= 0 && !fallen)
        {
            fallen = true;
            StartCoroutine(treeFall());
        }

    }

    IEnumerator treeFall()
    {
        Instantiate(treeStump, transform.position, transform.rotation);
        transform.DOScale(new Vector3(.8f, 1.3f, .8f), .3f);
        yield return new WaitForSeconds(.3f);
        transform.DOScale(Vector3.zero, .3f);

        //** Instantiate Logs Here**

        int dropCount = 3;

        for (int i = 0; i < dropCount; i++)
        {
            GameObject drop = Instantiate(woodDrop, transform.position, transform.rotation);

            foreach (Transform child in drop.transform)
                child.GetComponent<Rigidbody>().isKinematic = true;


            drop.transform.localScale = Vector3.zero;
            drop.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutBounce);
            drop.transform.DOMoveY((drop.transform.position.y + 3) + i, 0.3f);
            yield return new WaitForSeconds(0.5f);

            foreach (Transform child in drop.transform)
                child.GetComponent<Rigidbody>().isKinematic = false;


        }







        Destroy(this.gameObject, 3.5f);
    }


}
