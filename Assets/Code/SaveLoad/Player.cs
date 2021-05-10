using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{

    public string name;
    public List<Item> inventory;

    public Player()
    {
        this.name = "";
        this.inventory = new List<Item>();
    }
}