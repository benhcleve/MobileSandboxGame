using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject target;
    public GameObject TouchMarker;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        selectTarget();
    }

    void selectTarget()
    {
        if (Input.touchCount > 0)
        {
            // Cast a ray from screen point
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            // Save the info
            RaycastHit hit;
            // You successfully hit
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Interactable>())
                    target = hit.transform.gameObject;

            }
        }

    }
}
