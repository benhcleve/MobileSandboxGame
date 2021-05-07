using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item/Apparel", order = 3)]
public class ItemApparel : Item
{
    public Texture texture;
    public Color32 color;
    public string apparelType;
    public override void Use()
    {
        PlayerEquipmentManager.instance.RemoveWorn(apparelType);
        PlayerEquipmentManager.instance.Wear(ID, apparelType, texture, color);
    }
}
