using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using DG.Tweening;



public class ObjectPlacement : MonoBehaviour
{

    Transform player;
    bool isObjectPlaced = false;
    public GameObject terrain;
    public LayerMask cannotBuildLayer;
    GameObject rotateButton;
    GameObject placeButton;

    public GameObject placedObjectPrefab;
    public GameObject placedObjectPlaceholder;
    public Item placedObjectItem;
    public float buildTimer;
    public Material placementGlowMat;
    public GameObject buildSmoke;

    private static ObjectPlacement _instance;
    public static ObjectPlacement instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    private void Start()
    {
        player = PlayerMovement.instance.transform;
        placeButton = transform.Find("Place Button").gameObject;
        rotateButton = transform.Find("Rotate Button").gameObject;

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
        if (UIManager.instance.uiState != UIManager.UIState.Placement)
            UIManager.instance.UpdateUIManager(UIManager.UIState.Placement);

        if (placedObjectPlaceholder == null && !isObjectPlaced) //If object instance has not been instantiated
        {
            //Instantiates initial prefab to position in front of player
            Vector3 nearPlayerPos = new Vector3(RoundToNearestMultiple(player.position.x + (player.forward.x * 2), 2), 0, RoundToNearestMultiple(player.position.z + (player.forward.z * 2), 2));
            placedObjectPlaceholder = Instantiate(placedObjectPrefab, nearPlayerPos, Quaternion.identity);
            placedObjectPlaceholder.layer = 2; //Sets layer to ignore raycast so canPlace doesnt check self

            //Adds glow material
            if (placedObjectPlaceholder.GetComponent<MeshRenderer>())
            {
                var prefabTxtr = placedObjectPlaceholder.GetComponent<MeshRenderer>().material.mainTexture;
                placedObjectPlaceholder.GetComponent<MeshRenderer>().material = placementGlowMat;
                placementGlowMat.SetTexture("_Texture", prefabTxtr);
            }
            if (placedObjectPlaceholder.GetComponent<Door>())
            {
                SkinnedMeshRenderer doorSkin = placedObjectPlaceholder.transform.Find("CottageDoor").GetComponent<SkinnedMeshRenderer>();
                var prefabTxtr = doorSkin.material.mainTexture;
                doorSkin.material = placementGlowMat;
                placementGlowMat.SetTexture("_Texture", prefabTxtr);

            }
            //Adds silouette when behind other gameobjects
            var outline = placedObjectPlaceholder.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.SilhouetteOnly;
            outline.OutlineColor = placementGlowMat.GetColor("_GlowColor");
            SetPlacementeColor();
        }

        //IF MOBILE
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
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
                    if (touchMoveDist < 50) //If 1 touch tap
                    {
                        MoveObjectInstance(Input.GetTouch(0).position);
                    }
                }
            }
        }
        else //IF PC
        {
            //Prevents moving when clicking UI elements
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            float touchMoveDist = 0;
            if (Input.GetMouseButtonDown(0)) //Set starting position of touch 1
                touchStartPos = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.mousePosition); //Detect touch 1 drag distance
                if (touchMoveDist < 50) //If 1 touch tap
                {
                    MoveObjectInstance(Input.mousePosition);
                }
            }
        }

    }
    void MoveObjectInstance(Vector3 movePos)
    {
        //MOVE OBJECT
        Ray ray = Camera.main.ScreenPointToRay(movePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 placementPos = new Vector3(RoundToNearestMultiple(hit.point.x, 2), 0, RoundToNearestMultiple(hit.point.z, 2));
            placedObjectPlaceholder.transform.position = placementPos;
            SetPlacementeColor();
        }
    }
    public bool canPlace() //Detect if object can be placed here
    {
        // FLOOR
        if (placedObjectPlaceholder.GetComponent<Flooring>())
        {
            RaycastHit hit;
            if (Physics.Raycast(placedObjectPlaceholder.transform.position + new Vector3(0, 2, 0), placedObjectPlaceholder.transform.TransformDirection(Vector3.down), out hit, 2f))
            {
                if (hit.transform.gameObject.GetComponent<Foundation>())
                    return true;
                else return false;
            }
        }
        // WALL
        else if (placedObjectPlaceholder.GetComponent<Wall>())
        {
            RaycastHit hit;
            if (Physics.Raycast(placedObjectPlaceholder.transform.position + new Vector3(0, 2, 0), placedObjectPlaceholder.transform.TransformDirection(Vector3.down), out hit, 2f))
            {
                if (hit.transform.gameObject.GetComponent<Flooring>())
                {
                    Collider[] colliders = Physics.OverlapSphere(placedObjectPlaceholder.transform.position, 2f);
                    if (colliders.Length >= 1)
                    {
                        foreach (var collider in colliders)
                        {
                            if (collider != placedObjectPlaceholder.GetComponent<Collider>() && collider.transform.gameObject.GetComponent<Wall>() &&
                            collider.transform.eulerAngles == placedObjectPlaceholder.transform.eulerAngles && collider.transform.position == placedObjectPlaceholder.transform.position)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
        }
        // DOOR
        else if (placedObjectPlaceholder.GetComponent<Door>())
        {
            Collider[] colliders = Physics.OverlapSphere(placedObjectPlaceholder.transform.position, 2f);
            if (colliders.Length >= 1)
            {
                foreach (var collider in colliders)
                {
                    //If in doorway
                    if (collider != placedObjectPlaceholder.GetComponent<Collider>() && collider.transform.gameObject.GetComponent<WallDoorway>() &&
                    collider.transform.eulerAngles == placedObjectPlaceholder.transform.eulerAngles && collider.transform.position == placedObjectPlaceholder.transform.position)
                    {
                        Debug.Log("In Doorway");
                        return true;
                    }
                }
                Debug.Log("Default False");
                return false;
            }
        }
        //DEFAULT
        else if (Physics.CheckSphere(placedObjectPlaceholder.transform.position, 1, cannotBuildLayer))
            return false;
        else return true;

        return false;
    }

    void SetPlacementeColor()
    {
        //Detect if object can be placed here
        if (canPlace())
        {
            if (placedObjectPlaceholder.GetComponent<MeshRenderer>())
            {
                placedObjectPlaceholder.GetComponent<MeshRenderer>().material.SetColor("_GlowColor", Color.green);
                placedObjectPlaceholder.GetComponent<Outline>().OutlineColor = new Color(0, 1, 0, .5f);
            }

            //Door mesh renderer is on child
            if (placedObjectPlaceholder.GetComponent<Door>())
            {
                SkinnedMeshRenderer doorSkin = placedObjectPlaceholder.transform.Find("CottageDoor").GetComponent<SkinnedMeshRenderer>();
                doorSkin.material.SetColor("_GlowColor", Color.green);
                placedObjectPlaceholder.GetComponent<Outline>().OutlineColor = new Color(0, 1, 0, .5f);
            }
        }
        else
        {
            if (placedObjectPlaceholder.GetComponent<MeshRenderer>())
            {
                placedObjectPlaceholder.GetComponent<MeshRenderer>().material.SetColor("_GlowColor", Color.red);
                placedObjectPlaceholder.GetComponent<Outline>().OutlineColor = new Color(1, 0, 0, .5f);
            }

            //Door mesh renderer is on child
            if (placedObjectPlaceholder.GetComponent<Door>())
            {
                SkinnedMeshRenderer doorSkin = placedObjectPlaceholder.transform.Find("CottageDoor").GetComponent<SkinnedMeshRenderer>();
                doorSkin.material.SetColor("_GlowColor", Color.red);
                placedObjectPlaceholder.GetComponent<Outline>().OutlineColor = new Color(1, 0, 0, .5f);
            }
        }
    }

    public IEnumerator PlaceObjCoroutine()
    {
        isObjectPlaced = true;
        placeButton.SetActive(false);
        rotateButton.SetActive(false);
        Vector3 placementPos = placedObjectPlaceholder.transform.position;
        Quaternion placementRot = placedObjectPlaceholder.transform.rotation;

        PlayerMovement.instance.navMeshAgent.SetDestination(placedObjectPlaceholder.transform.position);

        Destroy(placedObjectPlaceholder);
        placedObjectPlaceholder = null;

        yield return new WaitUntil(() => Vector3.Distance(player.position, placementPos) < 2);

        PlayerMovement.instance.navMeshAgent.SetDestination(player.position);
        player.LookAt(new Vector3(placementPos.x, player.position.y, placementPos.z));

        PlayerAnimation.instance.SetBuildAnimation(1);
        PlayerAnimation.instance.SetAnimation("isBuilding");

        yield return new WaitForSeconds(buildTimer);

        GameObject placedObj = Instantiate(placedObjectPrefab, placementPos, placementRot);

        if (placedObj.GetComponent<Door>())
            placedObj.GetComponent<Door>().isPlaced = true;

        //If the placed object comes from inventory
        if (placedObjectItem != null)
        {
            if (!placedObjectItem.stackable)
            {
                Destroy(placedObjectItem);
            }
            else if (placedObjectItem.stackable)
                placedObjectItem.stackCount--;
        }

        //Rebuild navmesh after placing object
        terrain.GetComponent<NavMeshSurface>().BuildNavMesh();

        placedObj.name = placedObjectPrefab.name;
        placedObj.transform.localScale = Vector3.zero;
        placedObj.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.OutBounce);
        placedObj.transform.DOJump(placedObj.transform.position, 2, 1, .5f);
        var smoke = Instantiate(buildSmoke, placedObj.transform.position, Quaternion.identity);
        Destroy(smoke, 1);



        ExitPlacement();
    }

    public float RoundToNearestMultiple(float numberToRound, float multipleOf)
    {
        int multiple = Mathf.RoundToInt(numberToRound / multipleOf);

        return multiple * multipleOf;
    }

    //BUTTONS
    public void RotatePlacementObjInstance()
    {
        placedObjectPlaceholder.transform.Rotate(0, 90, 0);
        SetPlacementeColor();
    }

    public void PlaceObjInstance()
    {
        if (canPlace())
            StartCoroutine(PlaceObjCoroutine());
    }

    public void ExitPlacement()
    {
        PlayerMovement.instance.navMeshAgent.SetDestination(player.position);
        if (placedObjectPlaceholder != null)
            Destroy(placedObjectPlaceholder);
        if (placedObjectPrefab != null)
            placedObjectPrefab = null;
        if (PlayerEquipmentManager.instance.equippedItem != null)
            Destroy(PlayerEquipmentManager.instance.equippedItem);

        placedObjectItem = null;
        placeButton.SetActive(true);
        rotateButton.SetActive(true);
        PlayerAnimation.instance.animator.SetBool("isBuilding", false);
        PlayerAnimation.instance.animator.SetInteger("buildAnim", 0);
        buildTimer = 0;
        isObjectPlaced = false;
        UIManager.instance.UpdateUIManager(UIManager.UIState.Default);
        PlayerInventory.instance.UpdateSlots();
        gameObject.SetActive(false);
    }

}
