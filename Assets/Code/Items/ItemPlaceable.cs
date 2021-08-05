using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Item/Placeable", order = 6)]
public class ItemPlaceable : Item
{
    public enum PlaceableType { Structure, InsideFurniture, OutsideFurniture }
    public PlaceableType placeableType;
    public GameObject placementPrefab;
    public float buildtime;
    public override void Use()
    {
        ObjectPlacement.instance.gameObject.SetActive(true);
        ObjectPlacement.instance.placedObjectPrefab = placementPrefab;
        ObjectPlacement.instance.buildTimer = buildtime;
        ObjectPlacement.instance.placedObjectItem = this;
    }
}
