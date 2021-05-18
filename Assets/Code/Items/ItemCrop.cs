using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Item/Crop", order = 5)]
public class ItemCrop : Item
{
    public override void Use()
    {
        Debug.Log("This is a crop");
    }
}
