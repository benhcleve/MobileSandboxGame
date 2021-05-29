using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Camera cam;
    public Vector3 destination;
    public GameObject touchMarker;

    public LayerMask walkable;

    //touch variables
    bool isTwoTouch = false;
    Vector2 touchStartPos;

    private static PlayerMovement _instance;
    public static PlayerMovement instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (UIManager.instance.uiState == UIManager.UIState.Default)
            DetectTouch();


    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            float touchMoveDist = 0;
            if (Input.touchCount >= 2) //If touch 2 is used
                isTwoTouch = true;

            if (!isTwoTouch)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                    touchStartPos = Input.GetTouch(0).position;

                if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary) //Drag to move
                    SetDestination();

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance

                    if (touchMoveDist < 50) //If tap to move, set destination
                        SetDestination();
                    else if (touchMoveDist >= 50) //If has been dragging to move, end destination
                        navMeshAgent.destination = transform.position;
                }
            }
        }
        else
            isTwoTouch = false; //Set to false when not touching screen
    }

    void SetDestination()
    {
        // Cast a ray from screen point
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        // Save the info
        RaycastHit hit;
        // You successfully hit
        if (Physics.Raycast(ray, out hit))
        {
            if (walkable == (walkable | (1 << hit.transform.gameObject.layer)))
            {
                destination = hit.point;
                touchMarker.transform.position = destination;
                navMeshAgent.SetDestination(destination);

            }

            if (!hit.transform.gameObject.GetComponent<Interactable>()) //If not interactable, set target to null
                GetComponent<PlayerInteraction>().target = null;
        }
    }

    public void StopMovement() => navMeshAgent.destination = transform.position;

}