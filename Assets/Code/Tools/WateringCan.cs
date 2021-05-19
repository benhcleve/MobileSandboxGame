using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WateringCan : MonoBehaviour
{
    Animator animator;
    public PathCreator pathCreator;
    public GameObject waterParticles;
    private void Awake()
    {
        if (PlayerEquipmentManager.instance.equippedItem = gameObject)
        {
            animator = PlayerAnimation.instance.animator;
            animator.SetBool("isHoldingWaterCan", true);
        }

    }

    private void Update()
    {
        if (pathCreator.onPath && !waterParticles.activeInHierarchy)
            waterParticles.SetActive(true);
        else if (!pathCreator.onPath && waterParticles.activeInHierarchy)
            waterParticles.SetActive(false);
    }

    private void OnDestroy()
    {
        animator.SetBool("isHoldingWaterCan", false);
    }

    Vector3 touchStartPos;
    void DetectTouch()
    {
        bool isTwoTouch = false;

        if (Input.touchCount > 0)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            if (Input.touchCount >= 2) //If touch 2 is used
                isTwoTouch = true;

            if (Input.GetTouch(0).phase == TouchPhase.Began && !isTwoTouch)
            {
                touchStartPos = Input.GetTouch(0).position; //Set starting position of touch 1
            }

            //Dropping crop after pulling up
            if (Input.GetTouch(0).phase == TouchPhase.Ended && !isTwoTouch)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                // You successfully hit soil
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.GetComponent<Soil>())
                    {

                    }

            }
        }
    }




}
