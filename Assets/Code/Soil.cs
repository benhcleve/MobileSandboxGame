using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soil : Interactable
{
    public enum CropStatus { Vacant, Seeds, Sprout, HalfGrown, FullGrown }
    public CropStatus status = CropStatus.Vacant;
    public ItemSeeds itemSeeds;
    public GameObject currentCrop;
    public int plantTime;
    public GameObject radialMenu;
    public float waterSaturation = 0f;
    public Material material;
    int last_Gametime;


    public override void Interact()
    {
        base.Interact();
        radialMenu.SetActive(true);
    }

    private void Start()
    {
        last_Gametime = GameTime.instance.gameTime;
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (status != CropStatus.Vacant)
            StatusChange();

        if (isInteracting && !radialMenu.activeInHierarchy)
            isInteracting = false;


        UpdateWaterSaturation();

    }

    Vector2 touchStartPos;


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

    void UpdateWaterSaturation()
    {
        if (GameTime.instance.gameTime != last_Gametime && waterSaturation > 0)
            waterSaturation -= (GameTime.instance.gameTime - last_Gametime) * .01f;

        material.SetFloat("_TextureTransition", waterSaturation);

        last_Gametime = GameTime.instance.gameTime;
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
