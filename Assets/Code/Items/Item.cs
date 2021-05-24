using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Item/Generic", order = 1)]
[System.Serializable]
public class Item : ScriptableObject
{
    public int ID;
    public ItemSlot currentSlot;
    public string itemName;
    public string description;
    public bool stackable;
    public int stackCount;
    public bool usesFill;
    public float fill;
    public Sprite icon;
    public GameObject prefab;
    public int value;

    public Item()
    {
        this.ID = 0;
        this.stackCount = 1;
    }

    public virtual void Use() { }
}