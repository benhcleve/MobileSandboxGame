using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterColliderGen : MonoBehaviour
{
    //THIS SCRIPT ONLY WORKS IF USING WATER MESH
    int xSize, zSize;
    Vector3[] surfacePoints;
    List<Vector3> groundPoints = new List<Vector3>();
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
        DrawLines();
    }

    void GenerateSurfacePoints()
    {
        surfacePoints = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                Vector3 localPoint = new Vector3(x, .5f, z);
                surfacePoints[i] = localPoint;
            }
        }
    }

    void GenerateGroundPoints()
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(surfacePoints[i] + transform.position, Vector3.down, out hit, 10, groundMask))
            {
                groundPoints.Add(hit.point);
            }
        }
    }

    void DrawLines()
    {
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(surfacePoints[i] + transform.position, Vector3.down, out hit, 10, groundMask))
                Debug.DrawLine(surfacePoints[i] + transform.position, hit.point, Color.black);
        }
    }

    private void OnDrawGizmos()
    {
        if (surfacePoints == null || groundPoints == null)
            return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < surfacePoints.Length; i++)
        {
            Gizmos.DrawSphere(surfacePoints[i] + transform.position, 0.05f);
        }
        for (int i = 0; i < groundPoints.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundPoints[i], 0.05f);
        }
    }


}
