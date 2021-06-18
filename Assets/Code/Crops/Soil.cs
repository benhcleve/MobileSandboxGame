using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soil : Interactable
{
    public enum CropStatus { Vacant, Seeds, Sprout, HalfGrown, FullGrown, Dead }
    public CropStatus status = CropStatus.Vacant;
    public ItemSeeds itemSeeds;
    public GameObject currentCrop;
    public int plantTime;
    public GameObject radialMenu;
    public float waterSaturation = 0f;
    public Material material;
    int last_Gametime;
    public float cropHealth;


    public override void Interact()
    {
        base.Interact();
        if (status == CropStatus.Vacant)
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
        {
            StatusChange();
            if (status != CropStatus.FullGrown)
                DamageWhenDry();
        }

        if (currentCrop == null && status != CropStatus.Vacant)
        {
            status = CropStatus.Vacant;
            itemSeeds = null;
            currentCrop = null;
        }

        if (isInteracting && !radialMenu.activeInHierarchy)
            isInteracting = false;

        UpdateWaterSaturation();

        last_Gametime = GameTime.instance.gameTime; //Keep at end up update
    }

    public void PlantCrop()
    {
        plantTime = GameTime.instance.gameTime;
        GameObject seedPrefab = Instantiate(itemSeeds.seeds, transform.root.position, transform.root.rotation);
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

    void DamageWhenDry()
    {
        if (waterSaturation < itemSeeds.requiredWaterSat && status != CropStatus.Dead)
        {
            if (GameTime.instance.gameTime != last_Gametime)
                cropHealth -= (GameTime.instance.gameTime - last_Gametime) * .1f;

            if (cropHealth <= 0f)
                status = CropStatus.Dead;
        }
    }

    void UpdateWaterSaturation()
    {
        if (GameTime.instance.gameTime != last_Gametime && waterSaturation > 0)
            waterSaturation -= (GameTime.instance.gameTime - last_Gametime) * .0005f;

        material.SetFloat("_TextureTransition", waterSaturation);
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
        else if (growthPercent < 1f && cropHealth <= 0 && status != CropStatus.Dead)
            status = CropStatus.Dead;
        else statusDidChange = false; //If nothing triggered, status did not change

        if (statusDidChange) //If status changed, do something
        {
            Destroy(currentCrop);
            switch (status)
            {
                case CropStatus.Sprout:
                    currentCrop = Instantiate(itemSeeds.sproutGrown, transform.root.position, transform.root.rotation);
                    currentCrop.transform.parent = transform;
                    break;
                case CropStatus.HalfGrown:
                    currentCrop = Instantiate(itemSeeds.halfGrown, transform.root.position, transform.root.rotation);
                    currentCrop.transform.parent = transform;
                    break;
                case CropStatus.FullGrown:
                    currentCrop = Instantiate(itemSeeds.fullGrown, transform.root.position, transform.root.rotation);
                    foreach (Transform child in currentCrop.transform)
                    {
                        child.GetComponent<Rigidbody>().isKinematic = true;
                        child.GetComponent<Collider>().isTrigger = true;
                        child.GetComponent<Crop>().isPlanted = true;
                    }
                    currentCrop.transform.parent = transform;
                    break;
                case CropStatus.Dead:
                    currentCrop = Instantiate(itemSeeds.dead, transform.root.position, transform.root.rotation);
                    currentCrop.transform.parent = transform;
                    break;
            }
        }

    }







}
