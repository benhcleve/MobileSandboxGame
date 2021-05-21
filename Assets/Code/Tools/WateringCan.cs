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
    private void Awake()
    {
        col = GetComponent<Collider>();
        if (PlayerEquipmentManager.instance.equippedItem = gameObject)
        {
            animator = PlayerAnimation.instance.animator;
            animator.SetBool("isHoldingWaterCan", true);
        }

    }

    private void Update()
    {
        if (pathCreator.onPath && !waterParticles.activeInHierarchy)
        {
            waterParticles.SetActive(true);
            col.enabled = true;
            animator.SetBool("isWateringWaterCan", true);
            animator.SetBool("isHoldingWaterCan", false);
            PlayerMovement.instance.navMeshAgent.speed = 1;
        }

        else if (!pathCreator.onPath && waterParticles.activeInHierarchy)
        {
            waterParticles.SetActive(false);
            col.enabled = false;
            animator.SetBool("isWateringWaterCan", false);
            animator.SetBool("isHoldingWaterCan", true);
            PlayerMovement.instance.navMeshAgent.speed = 3.5f;
        }

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
