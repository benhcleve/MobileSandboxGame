using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Item/Placeable", order = 6)]
public class ItemPlaceable : Item
{
    public GameObject placementPrefab;
    public override void Use()
    {
        ObjectPlacement.instance.gameObject.SetActive(true);
        ObjectPlacement.instance.placedObjectPrefab = placementPrefab;
        ObjectPlacement.instance.buildTimer = 5;
        ObjectPlacement.instance.placedObjectItem = this;
    }
}
