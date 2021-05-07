using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item/Generic", order = 1)]
public class Item : ScriptableObject
{
    public int ID;
    public string itemName;
    public string description;
    public bool stackable;
    public int stackCount;
    public Sprite icon;
    public GameObject prefab;

    public virtual void Use() { }
}