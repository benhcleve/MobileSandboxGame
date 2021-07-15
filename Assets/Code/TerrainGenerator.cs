using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainGenerator : MonoBehaviour
{
    public bool doGenerate;
    public int depth = 20;
    public int width = 64;
    public int height = 64;
    public float scale = 20;
    NavMeshSurface navMesh;

    private void Start()
    {
        if (doGenerate)
        {
            Terrain terrain = GetComponent<Terrain>();
            terrain.terrainData = GenerateTerrain(terrain.terrainData);
        }
        navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
