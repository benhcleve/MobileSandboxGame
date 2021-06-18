using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerInteraction : MonoBehaviour
{
    private static PlayerInteraction _instance;
    public static PlayerInteraction instance { get { return _instance; } }

    public GameObject target;

    Vector2 touchStartPos;

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
        selectTargetTouch();
        selectTargetMouse();
        InteractWithTarget();
    }

    void selectTargetTouch()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
        {
            if (Input.touchCount == 1)
            {
                //Prevents moving when clicking UI elements
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) || EventSystem.current.IsPointerOverGameObject())
                    return;

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                    touchStartPos = Input.GetTouch(0).position; //Set starting position of touch 1

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    float dragDistance = Vector3.Distance(touchStartPos, Input.GetTouch(0).position);

                    if (dragDistance < 50)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;
                        // You successfully hit
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform.gameObject.GetComponent<Interactable>())
                            {
                                target = hit.transform.gameObject;
                                PlayerMovement.instance.navMeshAgent.destination = target.transform.position; //Move to interactable object
                            }

                        }
                    }

                }

            }
        }
    }

    void selectTargetMouse()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Prevents moving when clicking UI elements
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // You successfully hit
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.GetComponent<Interactable>())
                    {
                        target = hit.transform.gameObject;
                        PlayerMovement.instance.navMeshAgent.destination = target.transform.position; //Move to interactable object
                    }

                }

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
