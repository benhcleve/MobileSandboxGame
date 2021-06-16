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
    public bool craftable;
    public int craftOutputStackCount = 1;  //If stackable, how many are output when crafted?
    public Item[] craftingRecipeItems; //Item in array is associated with craftingRecipeItemCount (How many items are needed for recipe)
    public int[] craftingRecipeItemCount;


    public Item()
    {
        this.ID = 0;
        this.stackCount = 1;
    }

    public virtual void Use() { }
}