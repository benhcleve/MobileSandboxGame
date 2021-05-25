using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WateringCan : MonoBehaviour
{
    Animator animator;
    public PathCreator pathCreator;
    public GameObject waterParticles;
    Collider col;
    public GameObject UI;
    float waterFill;
    public GameObject canWater;
    public Item item;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
        if (PlayerEquipmentManager.instance.equippedItem = gameObject)
        {
            animator = PlayerAnimation.instance.animator;
            animator.SetBool("isHoldingWaterCan", true);
            item = PlayerEquipmentManager.instance.equippedItemSlot.currentItem;
        }

    }

    private void Update()
    {
        if (item.fill <= 0)
            pathCreator.canDrawPath = false;
        else pathCreator.canDrawPath = true;

        if (pathCreator.onPath)
        {
            if (!waterParticles.activeInHierarchy)
            {
                waterParticles.SetActive(true);
                col.enabled = true;
                animator.SetBool("isWateringWaterCan", true);
                animator.SetBool("isHoldingWaterCan", false);
                PlayerMovement.instance.navMeshAgent.speed = 1;
            }

            if (item.fill > 0)
                item.fill -= Time.deltaTime * 0.1f;
        }

        if (!pathCreator.onPath && waterParticles.activeInHierarchy)
        {
            waterParticles.SetActive(false);
            col.enabled = false;
            animator.SetBool("isWateringWaterCan", false);
            animator.SetBool("isHoldingWaterCan", true);
            PlayerMovement.instance.navMeshAgent.speed = 3.5f;
        }

        if (item.fill <= 0 && canWater.activeInHierarchy)
            canWater.SetActive(false);

        item.currentSlot.UpdateItemSlot();
    }

    private void FixedUpdate()
    {
        if (IsByWater() && !UI.activeInHierarchy && item.fill < 1)
            UI.SetActive(true);
        else if (!IsByWater() && UI.activeInHierarchy || item.fill >= 1 && UI.activeInHierarchy)
            UI.SetActive(false);

    }

    public bool IsByWater()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2);
        foreach (var hitCollider in hitColliders)
            if (hitCollider.gameObject.tag == "Water")
                return true;
        return false;
    }

    public void FillWateringCan()
    {
        item.fill = 1f;
        canWater.SetActive(true);
    }

    private void OnDestroy()
    {
        animator.SetBool("isHoldingWaterCan", false);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Soil>())
        {
            if (other.gameObject.GetComponent<Soil>().waterSaturation < 1f)
                other.gameObject.GetComponent<Soil>().waterSaturation += Time.deltaTime * .5f;
        }
    }




}
