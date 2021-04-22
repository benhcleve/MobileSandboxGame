﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public Camera cam;
    public Vector3 destination;
    public GameObject moveMarker;

    //touch variables
    bool isTwoTouch = false;
    Vector2 touchStartPos;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectTouch();
    }

    void DetectTouch()
    {
        if (Input.touchCount > 0)
        {
            float touchMoveDist = 0;

            if (Input.touchCount >= 2) //If touch 2 is used
                isTwoTouch = true;

            if (Input.GetTouch(0).phase == TouchPhase.Began) //Set starting position of touch 1
                touchStartPos = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchMoveDist = Vector2.Distance(touchStartPos, Input.GetTouch(0).position); //Detect touch 1 drag distance

                if (!isTwoTouch && touchMoveDist < 50) //If 1 touch tap, set destination
                    SetDestination();
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
        // You successfully hi
        if (Physics.Raycast(ray, out hit))
        {
            moveMarker.transform.position = hit.point;
            destination = moveMarker.transform.position;
            navMeshAgent.SetDestination(destination);
        }

    }

}