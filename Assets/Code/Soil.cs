using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soil : MonoBehaviour
{
    public enum CropStatus { Vacant, Seeds, Sprout, HalfGrown, FullGrown }
    public CropStatus status = CropStatus.Vacant;
    public ItemSeeds itemSeeds;
    public GameObject currentCrop;
    public int plantTime;
    public GameObject radialMenu;

    // Update is called once per frame
    void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
            DetectTouch();

        if (status != CropStatus.Vacant)
            StatusChange();
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

    public void PlantCrop()
    {
        plantTime = GameTime.instance.gameTime;
        GameObject seedPrefab = Instantiate(itemSeeds.prefab, transform.root.position, transform.root.rotation);
        seedPrefab.transform.parent = transform.root;
        currentCrop = seedPrefab;
        status = CropStatus.Seeds;
    }

    public float GrowthPercent()
    {
        int growingTime = GameTime.instance.gameTime - plantTime; //Time taken to grow so far
        float growthPercent = (((float)growingTime / (float)GameTime.instance.DurationToGametime(itemSeeds.growTime)));
        return growthPercent;
    }

    void StatusChange()
    {
        float growthPercent = GrowthPercent();
        bool statusDidChange = true;

        if (growthPercent < .1f && status != CropStatus.Seeds)
            status = CropStatus.Seeds;
        else if (growthPercent >= .1f && growthPercent < .5f && status != CropStatus.Sprout)
            status = CropStatus.Sprout;
        else if (growthPercent >= .5f && growthPercent < 1f && status != CropStatus.HalfGrown)
            status = CropStatus.HalfGrown;
        else if (growthPercent >= 1f && status != CropStatus.FullGrown)
            status = CropStatus.FullGrown;
        else statusDidChange = false; //If nothing triggered, status did not change

        if (statusDidChange) //If status changed, do something
        {
            Destroy(currentCrop);
            switch (status)
            {
                case CropStatus.Sprout:
                    currentCrop = Instantiate(itemSeeds.sproutGrown, transform.root.position, transform.root.rotation);
                    break;
                case CropStatus.HalfGrown:
                    currentCrop = Instantiate(itemSeeds.halfGrown, transform.root.position, transform.root.rotation);
                    break;
                case CropStatus.FullGrown:
                    currentCrop = Instantiate(itemSeeds.fullGrown, transform.root.position, transform.root.rotation);
                    break;
            }
        }

    }







}
