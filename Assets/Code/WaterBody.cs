using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBody : MonoBehaviour
{
    //THIS SCRIPT ONLY WORKS IF USING WATER MESH
    public bool showDebug;
    int xSize, zSize;
    public List<Vector3> surfacePoints = new List<Vector3>();
    public List<Vector3> groundPoints = new List<Vector3>();
    public LayerMask groundMask;

    void Start()
    {
        xSize = (int)Mathf.Ceil(transform.lossyScale.x);
        zSize = (int)Mathf.Ceil(transform.lossyScale.z);

        GenerateSurfacePoints();
        GenerateGroundPoints();
    }

    private void Update()
    {
        if (showDebug)
            DebugLines();
    }

    void GenerateSurfacePoints()
    {
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                Vector3 localPoint = new Vector3(x, .5f, z);
                surfacePoints.Insert(i, localPoint);
            }
        }
    }

    void GenerateGroundPoints()
    {
        //Loop to get rid of surface points that dont make contact with ground.
        for (int i = 0; i < surfacePoints.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(surfacePoints[i] + transform.position, Vector3.down, out hit, 10, groundMask))
            {
                Debug.Log(" Surface Point: " + (surfacePoints[i] + transform.position) + " Ground Point: " + hit.point);
            }
            else
            {
                surfacePoints.Remove(surfacePoints[i]);
                i--; //Go back one due to item being removed from list.
            }
        }
        //Loop again to add ground points.
        for (int i = 0; i < surfacePoints.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(surfacePoints[i] + transform.position, Vector3.down, out hit, 10, groundMask))
            {
                groundPoints.Add(hit.point);
            }
        }

    }

    void DebugLines()
    {
        for (int i = 0; i < surfacePoints.Count; i++)
        {

            if (showDebug)
                Debug.DrawLine(surfacePoints[i] + transform.position, groundPoints[i], Color.black);

        }
    }

    private void OnDrawGizmos()
    {
        if (surfacePoints == null || groundPoints == null || !showDebug)
            return;

        for (int i = 0; i < surfacePoints.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(surfacePoints[i] + transform.position, 0.05f);
        }
        for (int i = 0; i < groundPoints.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundPoints[i], 0.05f);
        }
    }


}
