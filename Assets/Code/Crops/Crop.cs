using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crop : MonoBehaviour
{
    public ItemCrop item;
    Rigidbody rb;
    Collider col;
    bool isTwoTouch = false;
    Vector2 touchStartPos;
    Vector3 spawnPos;
    bool isSelected;
    public bool isPlanted;


    private void Start()
    {
        spawnPos = transform.position;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

    }


    private void Update()
    {
        DetectTouch();
        DetectMouse();
        PlayerMagnet();
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            if (Input.touchCount >= 2) //If touch 2 is used
                isTwoTouch = true;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchStartPos = Input.GetTouch(0).position; //Set starting position of touch 1
                Ray ray = Camera.main.ScreenPointToRay(touchStartPos);
                RaycastHit hit;
                // You successfully hit
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform == this.transform) //If not interactable, set target to null
                        isSelected = true;

                if (isSelected && GetComponent<Outline>() == null)
                    SetOutline(true);

            }

            //Pulling crop up
            if (Input.GetTouch(0).phase == TouchPhase.Moved && isSelected
            || Input.GetTouch(0).phase == TouchPhase.Stationary && isSelected)
            {
                float yTouchDist = Input.GetTouch(0).position.y - touchStartPos.y;
                if (!isTwoTouch && yTouchDist > 100)
                {
                    transform.position = Vector3.Lerp(transform.position, spawnPos + Vector3.up, Time.deltaTime * 5);
                    transform.Rotate(Vector3.up * (Time.deltaTime * 100));
                    isPlanted = false;
                }
                else if (!isTwoTouch && yTouchDist < 100 && isPlanted)
                {
                    transform.position = spawnPos;
                    float x = transform.position.x + Random.Range(-0.05f, 0.05f);
                    float y = transform.position.y;
                    float z = transform.position.z + Random.Range(-0.05f, 0.05f);

                    transform.position = new Vector3(x, y, z);
                }

            }

            //Dropping crop after pulling up
            if (Input.GetTouch(0).phase == TouchPhase.Ended && isSelected)
            {
                SetOutline(false); //Get rid of selected outline
                isSelected = false;
                if (!isPlanted)
                {
                    col.isTrigger = false;
                    rb.isKinematic = false;
                    rb.angularVelocity = rb.transform.right * 5;
                }
            }
        }
        else
            isTwoTouch = false; //Set to false when not touching screen
    }

    void DetectMouse()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject())
                return;


            if (Input.GetMouseButtonDown(0))
            {
                touchStartPos = Input.mousePosition; //Set starting position of touch 1
                Ray ray = Camera.main.ScreenPointToRay(touchStartPos);
                RaycastHit hit;
                // You successfully hit
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform == this.transform) //If not interactable, set target to null
                        isSelected = true;

                if (isSelected && GetComponent<Outline>() == null)
                    SetOutline(true);

            }

            //Pulling crop up
            if (isSelected)
            {
                float yTouchDist = Input.mousePosition.y - touchStartPos.y;
                if (yTouchDist > 100)
                {
                    transform.position = Vector3.Lerp(transform.position, spawnPos + Vector3.up, Time.deltaTime * 5);
                    transform.Rotate(Vector3.up * (Time.deltaTime * 100));
                    isPlanted = false;
                }
                else if (yTouchDist < 100 && isPlanted)
                {
                    transform.position = spawnPos;
                    float x = transform.position.x + Random.Range(-0.05f, 0.05f);
                    float y = transform.position.y;
                    float z = transform.position.z + Random.Range(-0.05f, 0.05f);

                    transform.position = new Vector3(x, y, z);
                }

            }

            //Dropping crop after pulling up
            if (Input.GetMouseButtonUp(0))
            {
                SetOutline(false); //Get rid of selected outline
                isSelected = false;
                if (!isPlanted)
                {
                    col.isTrigger = false;
                    rb.isKinematic = false;
                    rb.angularVelocity = rb.transform.right * 5;
                }
            }
        }
        else
            isTwoTouch = false; //Set to false when not touching screen
    }

    void SetOutline(bool isOn)
    {
        if (isOn)
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.white;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineWidth = 5;
        }
        else if (!isOn)
            Destroy(GetComponent<Outline>());
    }

    void PlayerMagnet()
    {
        float distFromPlayer = Vector3.Distance(PlayerMovement.instance.transform.position, transform.position);
        if (distFromPlayer < 3)
            if (!PlayerInventory.instance.isInventoryFull())
            {
                if (!isPlanted && !isSelected)
                    transform.position = Vector3.MoveTowards(transform.position, PlayerMovement.instance.transform.position, 5 * Time.deltaTime);

                if (distFromPlayer < .5f && !isSelected && !isPlanted)
                    PlayerInventory.instance.AddToInventory(item, gameObject);
            }
    }



}
