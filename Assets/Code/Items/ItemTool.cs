using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item/Tool", order = 2)]
public class ItemTool : Item
{
    public override void Use()
    {
        if (PlayerEquipmentManager.instance.equippedItem == null || PlayerEquipmentManager.instance.equippedItem.name != prefab.name)
            PlayerEquipmentManager.instance.EquipToolToHand(this);

        else PlayerEquipmentManager.instance.UnequipTool();
    }
}
