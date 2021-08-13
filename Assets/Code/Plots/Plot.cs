using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Plot : MonoBehaviour
{
    public Terrain plotTerrain;
    public Vector2 plotCoordinates;
    public FenceManager fenceManager;
    public bool isPurchased;


    public void BakeTerrain()
    {
        plotTerrain.GetComponent<NavMeshSurface>().BuildNavMesh();
        fenceManager.UpdateFencing();
    }

}
