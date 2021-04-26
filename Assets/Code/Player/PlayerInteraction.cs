using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private static PlayerInteraction _instance;
    public static PlayerInteraction instance { get { return _instance; } }

    public GameObject target;
    public GameObject TouchMarker;

    private void Awake() => CreateInstance();
    void CreateInstance() //Make this UI Manager an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Update()
    {
        selectTarget();
        InteractWithTarget();
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

    void InteractWithTarget()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            float interactDistance = target.GetComponent<Interactable>().interactDistance;
            bool isInteracting = target.GetComponent<Interactable>().isInteracting;

            if (distance <= interactDistance && !isInteracting)
            {
                target.GetComponent<Interactable>().Interact();
                GetComponent<PlayerMovement>().StopMovement();
            }

        }

    }
}
