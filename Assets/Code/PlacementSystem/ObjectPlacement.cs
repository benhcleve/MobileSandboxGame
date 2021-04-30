using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject placedObjectPrefab;
    public GameObject placedObjectInstance;
    Transform player;

    public bool isObjectPlaced;


    private static ObjectPlacement _instance;
    public static ObjectPlacement instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    private void Start()
    {
        player = PlayerMovement.instance.transform;
        gameObject.SetActive(false);
    }
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Update()
    {
        PlaceObject();
    }

    Vector3 touchStartPos = Vector3.zero;
    void PlaceObject()
    {
        if (placedObjectInstance == null && !isObjectPlaced) //If object instance has not been instantiated
        {
            //Instantiates initial prefab to position in front of player
            Vector3 nearPlayerPos = new Vector3(RoundToNearestMultiple(player.position.x + (player.forward.x * 2), 2), 0, RoundToNearestMultiple(player.position.z + (player.forward.z * 2), 2));
            placedObjectInstance = Instantiate(placedObjectPrefab, nearPlayerPos, Quaternion.identity);
        }

        if (Input.touchCount > 0)
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            float touchMoveDist = 0;
            if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                touchStartPos = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance
                Debug.Log(touchMoveDist);
                if (touchMoveDist < 50) //If 1 touch tap
                {
                    //MOVE OBJECT
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 placementPos = new Vector3(RoundToNearestMultiple(hit.point.x, 2), 0, RoundToNearestMultiple(hit.point.z, 2));
                        placedObjectInstance.transform.position = placementPos;
                    }
                }
            }
        }
    }

    public void RotatePlacementObjInstance() => placedObjectInstance.transform.Rotate(0, 90, 0);

    public void PlaceObjInstance() => StartCoroutine(PlaceObjCoroutine());

    public IEnumerator PlaceObjCoroutine()
    {
        isObjectPlaced = true;
        Vector3 placementPos = placedObjectInstance.transform.position;
        Quaternion placementRot = placedObjectInstance.transform.rotation;

        PlayerMovement.instance.navMeshAgent.SetDestination(placedObjectInstance.transform.position);

        Destroy(placedObjectInstance);
        placedObjectInstance = null;

        yield return new WaitUntil(() => Vector3.Distance(player.position, placementPos) < 2);
        PlayerMovement.instance.navMeshAgent.SetDestination(player.position);

        GameObject placedObj = Instantiate(placedObjectPrefab, placementPos, placementRot);
        placedObj.name = placedObjectPrefab.name;

        isObjectPlaced = false;

        gameObject.SetActive(false);
    }

    public float RoundToNearestMultiple(float numberToRound, float multipleOf)
    {
        int multiple = Mathf.RoundToInt(numberToRound / multipleOf);

        return multiple * multipleOf;
    }

}
