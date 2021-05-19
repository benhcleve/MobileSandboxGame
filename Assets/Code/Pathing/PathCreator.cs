using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    public LayerMask pathableLayer;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();
    public bool pathDrawn;
    public bool onPath;
    Vector3 pathStartPoint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        OnNewPathCreated += SetPoints;
    }

    private void Update()
    {

        UpdatePathing();
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                points.Clear();

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, pathableLayer))
                {
                    if (DistanceToLastPoint(hitInfo.point) > 1f)
                    {
                        points.Add(hitInfo.point + new Vector3(0, .5f, 0));

                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());

                    }
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                OnNewPathCreated(points);
                pathDrawn = true;
                pathStartPoint = pathPoints.ToArray()[0];
            }

        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
            return Mathf.Infinity;
        return Vector3.Distance(points.Last(), point);
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        pathPoints = new Queue<Vector3>(points);
    }

    public void UpdatePathing()
    {
        if (ShouldSetDesintation())
        {
            lineRenderer.positionCount = pathPoints.Count;
            lineRenderer.SetPositions(pathPoints.ToArray());
            PlayerMovement.instance.navMeshAgent.SetDestination(pathPoints.Dequeue());

            if (Vector3.Distance(PlayerMovement.instance.navMeshAgent.transform.position, pathStartPoint) < 1f)
                onPath = true;

            if (pathPoints.Count == 0)
            {
                pathDrawn = false;
                onPath = false;
            }
        }

        if (onPath)
            PlayerMovement.instance.navMeshAgent.speed = 2;
        else PlayerMovement.instance.navMeshAgent.speed = 3.5f;

    }

    private bool ShouldSetDesintation()
    {
        if (pathPoints.Count == 0)
            return false;

        if (PlayerMovement.instance.navMeshAgent.hasPath == false || PlayerMovement.instance.navMeshAgent.remainingDistance < 0.5f)
            return true;

        return false;
    }


}
