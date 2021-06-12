using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropItems : MonoBehaviour
{
    public ItemSlot fromSlot;
    public ItemSlot toSlot;
    Vector3 touchStartPos;
    public Image draggedIcon;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectTouch();
    }

    void DetectTouch()
    {
        bool isTwoTouch = false;
        float touchMoveDist = 0;
        if (Input.touchCount > 0)
        {
            if (Input.touchCount >= 2) //If touch 2 is used
                isTwoTouch = true;

            if (Input.GetTouch(0).phase == TouchPhase.Began) //Get the slot that is being dragged
            {
                touchStartPos = Input.GetTouch(0).position;
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>())
                        fromSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>();
                }
            }

            //Drag the icon of the fromSlot with the touch drag
            if (fromSlot != null && fromSlot.currentItem != null)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance
                if (!isTwoTouch && touchMoveDist > 50)
                {
                    if (!draggedIcon.gameObject.activeInHierarchy)
                    {
                        draggedIcon.gameObject.SetActive(true);
                        draggedIcon.sprite = fromSlot.icon.sprite;
                        fromSlot.icon.enabled = false;
                    }
                    RectTransform iconRect = draggedIcon.GetComponent<RectTransform>();
                    iconRect.position = Input.GetTouch(0).position;
                }
            }

            //Drop the dragged item
            if (fromSlot != null)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance

                    //Raycast at the point of the touch end and get itemslot as toSlot if exists
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.GetTouch(0).position;
                    List<RaycastResult> raycastResultsList = new List<RaycastResult>();

                    EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
                    for (int i = 0; i < raycastResultsList.Count; i++)
                    {
                        if (raycastResultsList[i].gameObject.GetComponent<ItemSlot>())
                        {
                            var slot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            if (slot.slotType == ItemSlot.SlotType.Inventory || slot.slotType == ItemSlot.SlotType.Hotbar)
                                toSlot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            break; //End if found a toSlot
                        }
                    }

                    //Drop item if dragged from inventory or hotbar, and not touching any UI
                    if (fromSlot.currentItem != null && raycastResultsList.Count == 0)
                        if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                            DropItem(Input.GetTouch(0).position);


                    if (fromSlot != null) //Turn off dragged icon and return icon to itemslot
                        fromSlot.icon.enabled = true;
                    draggedIcon.sprite = null;
                    draggedIcon.gameObject.SetActive(false);

                    //Item placement logic
                    if (toSlot != null)
                    {
                        if (toSlot.currentItem == null)
                        {
                            toSlot.currentItem = fromSlot.currentItem;
                            fromSlot.currentItem = null;
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();
                        }
                        else if (toSlot.currentItem != null)
                        {
                            Item toSlotItem = toSlot.currentItem; //Save so that the fromSlot can switch
                            toSlot.currentItem = fromSlot.currentItem;
                            fromSlot.currentItem = toSlotItem;
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();
                        }
                    }


                    fromSlot = null;
                    toSlot = null;

                    // PlayerInventory.instance.UpdateInventory(); //Update placement of items in Inventory instance
                }
            }
        }
    }

    public void DropItem(Vector3 touchPos)
    {
        // Cast a ray from screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit))
        {
            GameObject drop = Instantiate(fromSlot.currentItem.prefab, hit.point + new Vector3(0, 1, 0), Quaternion.identity);

            if (fromSlot.currentItem.stackable)
                fromSlot.currentItem.stackCount--;

            else if (!fromSlot.currentItem.stackable)
            {
                //Get index of dragged item and remove it from inventory instance
                int itemIndex = PlayerInventory.instance.inventory.IndexOf(fromSlot.currentItem);
                PlayerInventory.instance.inventory[itemIndex] = null;

                fromSlot.currentItem = null;
            }

            if (drop.GetComponent<Pickupable>())
                drop.GetComponent<Pickupable>().playerMagnet = false;

            PlayerInventory.instance.UpdateSlots();
        }
    }


}
