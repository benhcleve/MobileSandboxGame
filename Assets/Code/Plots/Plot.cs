using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Plot : MonoBehaviour
{
    public Terrain plotTerrain;


    public void BakeTerrain()
    {
        plotTerrain.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

}
