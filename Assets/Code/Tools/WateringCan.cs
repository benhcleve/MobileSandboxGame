using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WateringCan : MonoBehaviour
{
    Animator animator;
    public GameObject waterParticles;
    Collider col;
    public GameObject fillButton;
    float waterFill;
    public GameObject waterInCan;
    public Item item;
    bool isWatering;
    public Image isWateringIcon;
    public Sprite x_Icon;
    public Sprite check_Icon;

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
        item.currentSlot.UpdateItemSlot();

        if (item.fill <= 0 && isWatering)
        {
            waterInCan.SetActive(false);
            ToggleWatering();
        }


        if (isWatering && item.fill > 0)
            item.fill -= Time.deltaTime * 0.1f;
    }

    private void FixedUpdate()
    {
        if (IsByWater() && !fillButton.activeInHierarchy && item.fill < 1)
            fillButton.SetActive(true);
        else if (!IsByWater() && fillButton.activeInHierarchy || item.fill >= 1 && fillButton.activeInHierarchy)
            fillButton.SetActive(false);

    }

    public void ToggleWatering()
    {
        isWatering = !isWatering;

        if (item.fill <= 0)
            isWatering = false;

        if (isWatering)
        {
            if (!waterParticles.activeInHierarchy)
            {
                waterParticles.SetActive(true);
                col.enabled = true;
                animator.SetBool("isWateringWaterCan", true);
                animator.SetBool("isHoldingWaterCan", false);
                PlayerMovement.instance.navMeshAgent.speed = 2;
                isWateringIcon.sprite = x_Icon;
            }
        }

        if (!isWatering)
        {
            waterParticles.SetActive(false);
            col.enabled = false;
            animator.SetBool("isWateringWaterCan", false);
            animator.SetBool("isHoldingWaterCan", true);
            PlayerMovement.instance.navMeshAgent.speed = 3.5f;
            isWateringIcon.sprite = check_Icon;
        }
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
        waterInCan.SetActive(true);
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
