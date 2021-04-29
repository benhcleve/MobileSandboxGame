using UnityEngine;

public class Item : ScriptableObject
{
    public int ID;
    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject prefab;

    public virtual void Use() { }
}