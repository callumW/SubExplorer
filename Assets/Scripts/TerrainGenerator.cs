using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public uint width;
    public uint height;

    public float noiseScale;
    public float stepScale;

    public NoiseSource[] sources;

    public float[,] GenerateHeightmap()
    {
        Debug.Log("num noise sources: " + sources.GetLength(0));
        float[,] terrainData = new float[width,height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                foreach (NoiseSource source in sources) {
                    terrainData[x,y] += source.Value(x, y);
                }
            }
        }

        return terrainData;
    }
}
