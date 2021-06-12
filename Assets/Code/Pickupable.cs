using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Item item;
    public bool playerMagnet;

    void Update() => Pickup();

    void Pickup()
    {
        float distFromPlayer = Vector3.Distance(PlayerMovement.instance.transform.position, transform.position);
        if (distFromPlayer < 3)
            if (!PlayerInventory.instance.isInventoryFull())
            {
                if (playerMagnet && !GetComponent<Rigidbody>().isKinematic)
                    transform.position = Vector3.MoveTowards(transform.position, PlayerMovement.instance.transform.position, 5 * Time.deltaTime);
                if (distFromPlayer < .5f)
                    PlayerInventory.instance.AddToInventory(item, gameObject);
            }
    }
}
