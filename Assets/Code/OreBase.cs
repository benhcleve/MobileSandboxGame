using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OreBase : Interactable
{
    public int hitpoints;
    public int oreStage;
    public GameObject oreDrop;
    public GameObject[] oreStageObjs;

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

        if (hitpoints <= 0)
        {
            //If larger sized rock, break into smaller rock. If smallest rock, destroy

            switch (oreStage)
            {
                case 3:
                    GameObject nextStage = Instantiate(oreStageObjs[1], transform.position, transform.rotation);
                    break;
                case 2:
                    nextStage = Instantiate(oreStageObjs[0], transform.position, transform.rotation);
                    break;
                case 1:

                    break;
            }


            GameObject ore = Instantiate(oreDrop, transform.position + Vector3.up, Quaternion.identity);
            ore.GetComponent<Pickupable>().playerMagnet = true;
            ore = Instantiate(oreDrop, transform.position + (Vector3.up * 1.5f), Quaternion.identity);
            ore.GetComponent<Pickupable>().playerMagnet = true;
            ore = Instantiate(oreDrop, transform.position + (Vector3.up * 2f), Quaternion.identity);
            ore.GetComponent<Pickupable>().playerMagnet = true;

            PlayerInteraction.instance.target = null;

            transform.DOScale(Vector3.zero, .5f);
            transform.DOShakePosition(.5f, .1f, 20, 90);

            Destroy(this.gameObject, 1);

        }

    }
}
