using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int width = 256;
    [SerializeField] private int height = 256;
    [SerializeField] private int depth = 20;
    [SerializeField] private float scale = 20f;

    //randomize the terrain
    public float offsetX = 100f;
    public float offsetY = 100f;

    private void Start()
    {
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
    }

    private void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];

        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    private float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
