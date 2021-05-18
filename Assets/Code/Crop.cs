﻿using System.Collections;
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
    bool isPlanted = true;


    private void Start()
    {
        spawnPos = transform.position;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

    }


    private void Update()
    {
        DetectTouch();
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
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                // You successfully hit
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform == this.transform) //If not interactable, set target to null
                        isSelected = true;

                if (isSelected && !isPlanted) //If already pulled up, pick up crop
                {
                    PlayerInventory.instance.AddToInventory(item, gameObject);
                }

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
            if (Input.GetTouch(0).phase == TouchPhase.Ended && isSelected && !isPlanted)
            {
                isSelected = false;
                col.isTrigger = false;
                rb.isKinematic = false;
                rb.angularVelocity = rb.transform.right * 5;
            }
        }
        else
            isTwoTouch = false; //Set to false when not touching screen
    }

}
