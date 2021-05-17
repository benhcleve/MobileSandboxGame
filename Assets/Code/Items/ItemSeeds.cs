using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Item/Seeds", order = 4)]
public class ItemSeeds : Item
{
    public int growTime;

    public override void Use()
    {
        Debug.Log("This is a seed");
    }
}
