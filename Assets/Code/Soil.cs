using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soil : MonoBehaviour
{
    public GameObject currentCrop;
    public GameObject radialMenu;

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
            DetectTouch();
    }

    Vector2 touchStartPos;
    void DetectTouch()
    {
        if (Input.touchCount == 1)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;


            float touchMoveDist = 0;

            if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                touchStartPos = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance

                if (touchMoveDist < 50) //If 1 touch tap
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    // You successfully hit
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == this.transform) //If not interactable, set target to null
                            if (Vector3.Distance(PlayerMovement.instance.transform.position, this.transform.position) < 5) //If player is near
                                radialMenu.SetActive(true);
                    }


                }
            }
        }
    }

}
