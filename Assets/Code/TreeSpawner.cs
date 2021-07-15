using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public int xSize, zSize;
    public Terrain plotTerrain;
    public LayerMask groundMask;
    public GameObject TreeObj;
    public int treeCount = 10;

    void Start()
    {
        xSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.x);
        zSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.z);

        GenerateSurfacePoints();
    }
    void GenerateSurfacePoints()
    {
        List<Vector3> spawnLocs = new List<Vector3>();

        for (int x = 0; x < treeCount; x++)
        {
            Vector3 spawnPos = GenerateSpawnPosition(xSize, zSize);

            if (spawnLocs.Contains(spawnPos)) //Ensure trees are not spawned in same location
            {
                Debug.Log("Double up");
                while (spawnLocs.Contains(spawnPos)) //Loops until tree is in spot not taken by another tree.
                    spawnPos = GenerateSpawnPosition(xSize, zSize);
            }
            while (plotTerrain.SampleHeight(spawnPos) != plotTerrain.terrainData.size.y) //Only spawn on flat default height terrain
            {
                spawnPos = GenerateSpawnPosition(xSize, zSize);
                Debug.Log("Moving due to height");
            }

            GameObject tree = Instantiate(TreeObj, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            spawnLocs.Add(spawnPos);
        }
    }

    Vector3 GenerateSpawnPosition(int x, int z)
    {
        Vector3 spawnLoc = new Vector3(Random.Range(5, x - 5), plotTerrain.terrainData.size.y, Random.Range(5, z - 5)) + plotTerrain.transform.position;
        //Subtract 5 to prevent spawning on edges of plot
        return new Vector3(Random.Range(5, x - 5), plotTerrain.terrainData.size.y, Random.Range(5, z - 5)) + plotTerrain.transform.position;
    }
}
