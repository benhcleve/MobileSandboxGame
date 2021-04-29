using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public GameObject avatar; //Drag human model to this slot, the whole model + Armature
    private Stitcher stitcher;

    public GameObject headWorn;
    public GameObject chestWorn;
    public GameObject legsWorn;
    public GameObject feetWorn;
    public GameObject backWorn;

    public void Awake() => stitcher = new Stitcher();

    private void Start()
    {
        StartCoroutine(LoadClothes());
    }

    IEnumerator LoadClothes()
    {
        Wear(2001, "Chest");
        yield return new WaitForSeconds(0.01f);
        Wear(2002, "Legs");
        yield return new WaitForSeconds(0.01f);
        Wear(2003, "Head");
    }

    public void RemoveWorn(string apparelType)
    {
        switch (apparelType)
        {
            case "Head":
                if (headWorn == null)
                    return;
                Destroy(headWorn);
                headWorn = null;
                break;
            case "Chest":
                if (chestWorn == null)
                    return;
                Destroy(chestWorn);
                chestWorn = null;
                break;
            case "Legs":
                if (legsWorn == null)
                    return;
                Destroy(legsWorn);
                legsWorn = null;
                break;
            case "Feet":
                if (feetWorn == null)
                    return;
                Destroy(feetWorn);
                feetWorn = null;
                break;
            case "Back":
                if (backWorn == null)
                    return;
                Destroy(backWorn);
                backWorn = null;
                break;
        }
    }

    public void Wear(int apparelID, string apparelType)
    {
        Item[] items = Resources.LoadAll<ItemApparel>("Items/Apparel").ToArray();
        foreach (Item i in items)
            if (i.ID == apparelID)
            {
                GameObject clothing = Instantiate(i.prefab);

                switch (apparelType)
                {
                    case "Head":
                        headWorn = stitcher.Stitch(clothing, avatar);
                        break;
                    case "Chest":
                        chestWorn = stitcher.Stitch(clothing, avatar);
                        break;
                    case "Legs":
                        legsWorn = stitcher.Stitch(clothing, avatar);
                        break;
                    case "Feet":
                        feetWorn = stitcher.Stitch(clothing, avatar);
                        break;
                    case "Back":
                        backWorn = stitcher.Stitch(clothing, avatar);
                        break;
                }
            }
    }
}
