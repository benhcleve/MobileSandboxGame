using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Item/Seeds", order = 4)]
public class ItemSeeds : Item
{

    public DateTime growTime;
    public GameObject seeds;
    public GameObject sproutGrown;
    public GameObject halfGrown;
    public GameObject fullGrown;
    public GameObject dead;
    public float requiredWaterSat;

    public override void Use()
    {
        Debug.Log("This is a seed");
    }
}
