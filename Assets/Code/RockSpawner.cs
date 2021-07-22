using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public int xSize, zSize;
    public Terrain plotTerrain;
    public LayerMask groundMask;
    public GameObject[] RockObjs;
    public int rockCount = 10;

    void Start()
    {
        xSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.x);
        zSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.z);

        GenerateSurfacePoints();
    }
    void GenerateSurfacePoints()
    {
        List<Vector3> spawnLocs = new List<Vector3>();

        for (int x = 0; x < rockCount; x++)
        {
            Vector3 spawnPos = GenerateSpawnPosition(xSize, zSize);

            if (spawnLocs.Contains(spawnPos)) //Ensure trees are not spawned in same location
            {
                Debug.Log("Double up");
                while (spawnLocs.Contains(spawnPos)) //Loops until tree is in spot not taken by another tree.
                    spawnPos = GenerateSpawnPosition(xSize, zSize);
            }

            Vector3 localPoint = new Vector3(spawnPos.x, 1, spawnPos.z);

            Vector3 randomness = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Vector3 terrainPos = new Vector3(plotTerrain.transform.position.x, 0, plotTerrain.transform.position.z);
            RaycastHit hit;
            if (Physics.Raycast(localPoint + randomness, Vector3.down, out hit, 10, groundMask))
            {
                GameObject rock = Instantiate(RockObjs[Random.Range(0, 3)], hit.point, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                rock.transform.rotation = Quaternion.LookRotation(rock.transform.forward, hit.normal);
                rock.transform.parent = transform;
            }
        }
    }

    Vector3 GenerateSpawnPosition(int x, int z)
    {
        Vector3 spawnLoc = new Vector3(Random.Range(5, x - 5), plotTerrain.terrainData.size.y, Random.Range(5, z - 5)) + plotTerrain.transform.position;
        //Subtract 5 to prevent spawning on edges of plot
        return spawnLoc;
    }
}