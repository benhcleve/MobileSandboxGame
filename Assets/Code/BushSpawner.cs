using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushSpawner : MonoBehaviour
{
    public int xSize, zSize;
    public Terrain plotTerrain;
    public LayerMask groundMask;
    public GameObject bushObj;
    public int bushCount = 10;

    void Start()
    {
        xSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.x);
        zSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.z);

        GenerateSurfacePoints();
    }
    void GenerateSurfacePoints()
    {
        List<Vector3> spawnLocs = new List<Vector3>();

        for (int x = 0; x < bushCount; x++)
        {
            Vector3 spawnPos = GenerateSpawnPosition(xSize, zSize);

            if (spawnLocs.Contains(spawnPos)) //Ensure trees are not spawned in same location
            {
                while (spawnLocs.Contains(spawnPos)) //Loops until tree is in spot not taken by another tree.
                    spawnPos = GenerateSpawnPosition(xSize, zSize);
            }

            GameObject tree = Instantiate(bushObj, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            spawnLocs.Add(spawnPos);
            tree.transform.parent = transform;
        }
    }

    Vector3 GenerateSpawnPosition(int x, int z)
    {
        Vector3 spawnLoc = new Vector3(Random.Range(5, x - 5), 10, Random.Range(5, z - 5)) + plotTerrain.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(spawnLoc, Vector3.down, out hit, 100))
        {
            spawnLoc = hit.point;
        }
        else spawnLoc = new Vector3(Random.Range(5, x - 5), 5, Random.Range(5, z - 5)) + plotTerrain.transform.position;

        //Subtract 5 to prevent spawning on edges of plot
        return spawnLoc;
    }
}
