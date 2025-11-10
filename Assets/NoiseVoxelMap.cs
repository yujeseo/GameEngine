using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseVoxelMap : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject GrassPrefab;
    public GameObject WaterPrefab;

    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;

    [SerializeField] float noiseScale = 20f;

    public int waterLevel = 6;

    private int[,] heightMap;

    void Start()
    {
        heightMap = new int[width, depth];

        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.FloorToInt(noise * maxHeight);

                heightMap[x, z] = h;

                if (h <= 0) continue;

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                        PlaceGrass(x, y, z);
                    else
                        Place(x, y, z);
                }
            }
        }


        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int groundHeight = heightMap[x, z];

                for (int y = 0; y <= waterLevel; y++)
                {
                    if (y > groundHeight)
                    {
                        PlaceWater(x, y, z);
                    }
                }
            }
        }
    }

    private void Place(int x, int y, int z)
    {
        var go = Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
    }

    private void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(GrassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"B_{x}_{y}_{z}";
    }

    private void PlaceWater(int x, int y, int z)
    {
        var go = Instantiate(WaterPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"W_{x}_{y}_{z}";
    }
}
