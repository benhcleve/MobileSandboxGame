using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public GameObject prefab;

    public virtual void Use() { }
}