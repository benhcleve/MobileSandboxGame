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
    public Transform rightHand;
    public GameObject equippedItem;

    private static PlayerEquipmentManager _instance;
    public static PlayerEquipmentManager instance { get { return _instance; } }

    void CreateInstance() //Make this UI Manager an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void Awake()
    {
        CreateInstance();
        stitcher = new Stitcher();
    }

    private void Start()
    {
        StartCoroutine(SpawnSavedOutfit());
    }

    IEnumerator SpawnSavedOutfit()
    {
        StartCoroutine(Wear(2002, "Legs", null, new Color32(255, 255, 255, 0)));
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Wear(2001, "Chest", null, new Color32(255, 255, 255, 0)));
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

    public IEnumerator Wear(int apparelID, string apparelType, Texture texture, Color32 color)
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

                yield return new WaitForSeconds(0.01f);

                var clothingMesh = clothing.transform.GetChild(0).gameObject;

                clothingMesh.GetComponent<SkinnedMeshRenderer>().material.SetColor("_MainColor", color);
                if (texture != null)
                    clothingMesh.GetComponent<SkinnedMeshRenderer>().material.SetTexture("_MainTex", texture);
            }
    }


    public void EquipToolToHand(GameObject prefab)
    {
        //Remove if another item is already equipped
        if (equippedItem != null)
            Destroy(equippedItem);

        equippedItem = Instantiate(prefab, rightHand.position, rightHand.rotation);
        equippedItem.transform.parent = rightHand;
    }
}
