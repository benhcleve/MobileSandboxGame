using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject placedObject;

    void Update()
    {
        PlaceObject();
    }


    void PlaceObject()
    {
        UIManager.instance.uiActive = true;

        if (Input.touchCount > 0)
        {
            // Cast a ray from screen point
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            // Save the info
            RaycastHit hit;
            // You successfully hit
            if (Physics.Raycast(ray, out hit))
            {

                Vector3 placementPos = new Vector3(RoundToNearestMultiple(hit.point.x, 2), 0, RoundToNearestMultiple(hit.point.z, 2));

                placedObject.transform.position = placementPos;

            }
        }
    }

    public float RoundToNearestMultiple(float numberToRound, float multipleOf)
    {
        int multiple = Mathf.RoundToInt(numberToRound / multipleOf);

        return multiple * multipleOf;
    }
}
