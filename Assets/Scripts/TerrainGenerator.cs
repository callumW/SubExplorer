using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public uint width;
    public uint height;

    public float noiseScale;


    public float[,] GenerateHeightmap()
    {
        float[,] terrainData = new float[width,height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                terrainData[x,y] = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);

                Debug.Log("(" + x + "," + y + ")height: " + terrainData[x,y]);
            }
        }

        return terrainData;
    }
}
