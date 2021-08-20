using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    public int xSize, zSize;
    public Terrain plotTerrain;
    public LayerMask groundMask;
    public GameObject grassObj;

    void Start()
    {
        xSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.x);
        zSize = (int)Mathf.Ceil(plotTerrain.terrainData.size.z);

        GenerateGrass();
    }
    void GenerateGrass()
    {
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            if (z % 2 == 0) //If Z is even number
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    if (x % 2 == 0) //If X is even number
                    {
                        Vector3 localPoint = new Vector3(x, 5, z);

                        Vector3 randomness = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                        Vector3 terrainPos = new Vector3(plotTerrain.transform.position.x, 0, plotTerrain.transform.position.z);

                        RaycastHit hit;
                        if (Physics.Raycast(localPoint + terrainPos + randomness, Vector3.down, out hit, 10, groundMask))
                        {
                            GameObject grass = Instantiate(grassObj, hit.point, Quaternion.Euler(0, Random.Range(0f, 360f), 0));
                            grass.transform.localScale = new Vector3(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
                            grass.transform.rotation = Quaternion.LookRotation(grass.transform.forward, hit.normal);
                            grass.transform.parent = transform;
                        }
                    }
                }
            }
        }
    }
}
