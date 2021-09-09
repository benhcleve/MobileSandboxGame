using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class DragDropItems : MonoBehaviour
{
    public ItemSlot fromSlot;
    public ItemSlot toSlot;
    Vector3 touchStartPos;
    public Image draggedIcon;
    public GameObject placeableBoxPrefab;

    AudioSource audioSource;
    public AudioClip popSFX;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        DetectTouch();
        DetectMouse();
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            bool isTwoTouch = false;
            float touchMoveDist = 0;

            if (Input.touchCount >= 2) // If touch 2 is used
                isTwoTouch = true;

            if (Input.GetTouch(0).phase == TouchPhase.Began) // Get the slot that is being dragged
            {
                touchStartPos = Input.GetTouch(0).position;
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>())
                    {
                        ItemSlot dragSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>();
                        if (dragSlot.slotType == ItemSlot.SlotType.Inventory || dragSlot.slotType == ItemSlot.SlotType.Hotbar || dragSlot.slotType == ItemSlot.SlotType.Storage
                        || dragSlot.slotType == ItemSlot.SlotType.CraftOutput || dragSlot.slotType == ItemSlot.SlotType.BaitSlot)
                            fromSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>();
                        else return;
                    }

                }
            }

            // Drag the icon of the fromSlot with the touch drag
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
                        draggedIcon.transform.DOScale(1.5f, 1f).SetEase(Ease.OutElastic);

                        audioSource.pitch = 1f;
                        audioSource.PlayOneShot(popSFX);
                    }
                    RectTransform iconRect = draggedIcon.GetComponent<RectTransform>();
                    iconRect.position = Input.GetTouch(0).position;
                }
            }

            // Drop the dragged item
            if (fromSlot != null)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    draggedIcon.transform.DOKill(); //Ends tweening animation if still playing while dropped
                    draggedIcon.transform.localScale = Vector3.one;
                    if (fromSlot != null) //Turn off dragged icon and return icon to itemslot
                        fromSlot.icon.enabled = true;
                    draggedIcon.sprite = null;
                    draggedIcon.gameObject.SetActive(false);

                    // Raycast at the point of the touch end and get itemslot as toSlot if exists
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.GetTouch(0).position;
                    List<RaycastResult> raycastResultsList = new List<RaycastResult>();

                    EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
                    for (int i = 0; i < raycastResultsList.Count; i++)
                    {
                        if (raycastResultsList[i].gameObject.GetComponent<ItemSlot>())
                        {
                            var slot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            if (slot.slotType == ItemSlot.SlotType.Inventory || slot.slotType == ItemSlot.SlotType.Hotbar || slot.slotType == ItemSlot.SlotType.Storage)
                                toSlot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            break; //End if found a toSlot
                        }
                    }

                    // Drop item if dragged from inventory or hotbar, and not touching any UI
                    if (fromSlot.currentItem != null && raycastResultsList.Count == 0)
                        if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                            DropItem(Input.GetTouch(0).position);

                    // Item placement logic
                    if (fromSlot == toSlot)
                    {
                        fromSlot = null;
                        toSlot = null;
                        return;
                    }
                    if (toSlot != null)
                    {
                        //If moving stackable item to a stackable item with same item ID
                        if (fromSlot.currentItem != null && toSlot.currentItem != null
                         && toSlot.currentItem.stackable && fromSlot.currentItem.stackable && toSlot.currentItem.ID == fromSlot.currentItem.ID)
                        {
                            if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[toSlot.inventoryIndex].stackCount += fromSlot.currentItem.stackCount;
                            else toSlot.currentItem.stackCount += fromSlot.currentItem.stackCount;
                            if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = null;
                            else fromSlot.currentItem = null;

                            audioSource.pitch = .5f;
                            audioSource.PlayOneShot(popSFX);

                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();

                        }
                        else if (toSlot.currentItem == null)
                        {
                            // Get actual item from inventory instance
                            Item fromSlotItem = fromSlot.currentItem;
                            Item toSlotItem = toSlot.currentItem;

                            if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[toSlot.inventoryIndex] = fromSlotItem;
                            if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = null;

                            //Place fromSlot Item on toSlot and remove reference to item on fromSlot
                            toSlot.currentItem = fromSlotItem;
                            fromSlot.currentItem = null;

                            audioSource.pitch = .9f;
                            audioSource.PlayOneShot(popSFX);

                            // Update slot UI
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();
                        }
                        else if (toSlot.currentItem != null)
                        {

                            // Get actual item from inventory instance
                            Item fromSlotItem = fromSlot.currentItem;
                            Item toSlotItem = toSlot.currentItem;

                            // Update Inventory/Hotbar Items in inventory instance if exist
                            if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = toSlotItem;
                            if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                PlayerInventory.instance.inventory[toSlot.inventoryIndex] = fromSlotItem;

                            //Place fromSlot Item on toSlot and remove reference to item on fromSlot
                            toSlot.currentItem = fromSlotItem;
                            fromSlot.currentItem = toSlotItem;

                            audioSource.pitch = .9f;
                            audioSource.PlayOneShot(popSFX);

                            //Update slot UI
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();


                        }
                    }
                    fromSlot = null;
                    toSlot = null;
                }
            }
        }
    }

    void DetectMouse()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            float touchMoveDist = 0;

            if (Input.GetMouseButtonDown(0)) // Get the slot that is being dragged
            {
                touchStartPos = Input.mousePosition;
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>())
                    {
                        ItemSlot dragSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>();
                        if (dragSlot.slotType == ItemSlot.SlotType.Inventory || dragSlot.slotType == ItemSlot.SlotType.Hotbar || dragSlot.slotType == ItemSlot.SlotType.Storage
                        || dragSlot.slotType == ItemSlot.SlotType.CraftOutput || dragSlot.slotType == ItemSlot.SlotType.BaitSlot)
                            fromSlot = EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>();
                        else return;
                    }

                }
            }

            // Drag the icon of the fromSlot with the touch drag
            if (fromSlot != null && fromSlot.currentItem != null)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.mousePosition); //Detect touch 1 drag distance
                if (touchMoveDist > 50)
                {
                    if (!draggedIcon.gameObject.activeInHierarchy)
                    {
                        draggedIcon.gameObject.SetActive(true);
                        draggedIcon.sprite = fromSlot.icon.sprite;
                        fromSlot.icon.enabled = false;
                        draggedIcon.transform.DOScale(1.5f, 1f).SetEase(Ease.OutElastic);

                        audioSource.pitch = 1f;
                        audioSource.PlayOneShot(popSFX);
                    }
                    RectTransform iconRect = draggedIcon.GetComponent<RectTransform>();
                    iconRect.position = Input.mousePosition;
                }
            }

            // Drop the dragged item
            if (fromSlot != null && fromSlot.currentItem != null)
            {
                if (Input.GetMouseButtonUp(0))
                {

                    draggedIcon.transform.DOKill(); //Ends tweening animation if still playing while dropped
                    draggedIcon.transform.localScale = Vector3.one;
                    if (fromSlot != null) //Turn off dragged icon and return icon to itemslot
                        fromSlot.icon.enabled = true;
                    draggedIcon.sprite = null;
                    draggedIcon.gameObject.SetActive(false);

                    audioSource.pitch = .9f;
                    audioSource.PlayOneShot(popSFX);

                    // Raycast at the point of the touch end and get itemslot as toSlot if exists
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;
                    List<RaycastResult> raycastResultsList = new List<RaycastResult>();

                    EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
                    for (int i = 0; i < raycastResultsList.Count; i++)
                    {
                        if (raycastResultsList[i].gameObject.GetComponent<ItemSlot>())
                        {
                            var slot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            if (slot.slotType == ItemSlot.SlotType.Inventory || slot.slotType == ItemSlot.SlotType.Hotbar
                            || slot.slotType == ItemSlot.SlotType.Storage)
                                toSlot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();
                            if (slot.slotType == ItemSlot.SlotType.BaitSlot && fromSlot.currentItem.GetType() == typeof(ItemBait))
                                toSlot = raycastResultsList[i].gameObject.GetComponent<ItemSlot>();

                            break; //End if found a toSlot
                        }
                    }

                    // Drop item if dragged from inventory or hotbar, and not touching any UI
                    if (fromSlot.currentItem != null && raycastResultsList.Count == 0)
                        if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                            DropItem(Input.mousePosition);



                    // Item placement logic
                    if (fromSlot == toSlot)
                    {
                        fromSlot = null;
                        toSlot = null;
                        return;
                    }

                    if (toSlot != null)
                    {
                        //If moving stackable item to a stackable item with same item ID
                        if (fromSlot.currentItem != null && toSlot.currentItem != null
                         && toSlot.currentItem.stackable && fromSlot.currentItem.stackable && toSlot.currentItem.ID == fromSlot.currentItem.ID)
                        {
                            if (toSlot.slotType == ItemSlot.SlotType.BaitSlot)
                            {
                                //Do not allow stacking bait on bait slot.
                            }
                            else
                            {
                                if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[toSlot.inventoryIndex].stackCount += fromSlot.currentItem.stackCount;
                                else toSlot.currentItem.stackCount += fromSlot.currentItem.stackCount;
                                if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = null;
                                else fromSlot.currentItem = null;
                            }

                            audioSource.pitch = .9f;
                            audioSource.PlayOneShot(popSFX);

                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();

                        }
                        else if (toSlot.currentItem == null)
                        {
                            if (toSlot.slotType == ItemSlot.SlotType.BaitSlot)
                            {
                                PlayerInventory.instance.inventory[fromSlot.inventoryIndex].stackCount--;
                                toSlot.currentItem = Object.Instantiate(fromSlot.currentItem);
                                toSlot.currentItem.stackCount = 1;
                            }
                            else
                            {

                                // Get actual item from inventory instance
                                Item fromSlotItem = fromSlot.currentItem;
                                Item toSlotItem = toSlot.currentItem;

                                if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[toSlot.inventoryIndex] = fromSlotItem;
                                if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = null;

                                //Place fromSlot Item on toSlot and remove reference to item on fromSlot
                                toSlot.currentItem = fromSlotItem;
                                fromSlot.currentItem = null;
                            }

                            // Update slot UI
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();
                        }
                        else if (toSlot.currentItem != null)
                        {
                            if (toSlot.slotType == ItemSlot.SlotType.BaitSlot || toSlot.slotType == ItemSlot.SlotType.CraftOutput)
                            {
                                //Dont allow swapping in these slot types
                            }
                            else
                            {
                                // Get actual item from inventory instance
                                Item fromSlotItem = fromSlot.currentItem;
                                Item toSlotItem = toSlot.currentItem;

                                // Update Inventory/Hotbar Items in inventory instance if exist
                                if (fromSlot.slotType == ItemSlot.SlotType.Inventory || fromSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[fromSlot.inventoryIndex] = toSlotItem;
                                if (toSlot.slotType == ItemSlot.SlotType.Inventory || toSlot.slotType == ItemSlot.SlotType.Hotbar)
                                    PlayerInventory.instance.inventory[toSlot.inventoryIndex] = fromSlotItem;

                                //Place fromSlot Item on toSlot and remove reference to item on fromSlot
                                toSlot.currentItem = fromSlotItem;
                                fromSlot.currentItem = toSlotItem;
                            }

                            audioSource.pitch = .9f;
                            audioSource.PlayOneShot(popSFX);

                            //Update slot UI
                            toSlot.UpdateItemSlot();
                            fromSlot.UpdateItemSlot();
                        }
                    }


                    fromSlot = null;
                    toSlot = null;
                }
            }
        }
    }


    public void DropItem(Vector3 touchPos)
    {
        // Cast a ray from screen point
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit))
        {
            GameObject drop = Instantiate(fromSlot.currentItem.prefab, hit.point + new Vector3(0, 1, 0), Quaternion.identity);

            if (fromSlot.currentItem.prefab == placeableBoxPrefab)
            {
                Debug.Log(fromSlot.currentItem.prefab);
                drop.GetComponent<Pickupable>().item = fromSlot.currentItem;
            }


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
