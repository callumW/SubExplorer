using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map : MonoBehaviour
{

    private const float CHUNK_DIM = 10;

    public GameObject player;
    public Material mapMaterial;
    public Material originMaterial;

    public int width;
    public int height;

    public int viewAreaRadius;

    private float xOffset;
    private float zOffset;

    private MapChunk[,] chunks;

    // Start is called before the first frame update
    void Start()
    {
        chunks = new MapChunk[width, height];

        xOffset = 0 - ((width - 1) * CHUNK_DIM) / 2;
        zOffset = 0 - ((height - 1) * CHUNK_DIM) / 2;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                if (x == 0 && z == 0) {
                    chunks[x,z] = new MapChunk(CHUNK_DIM, CHUNK_DIM, originMaterial);
                }
                else {
                    chunks[x,z] = new MapChunk(CHUNK_DIM, CHUNK_DIM, mapMaterial);
                }

                float xPos = ((float)(x * CHUNK_DIM)) + xOffset;
                float zPos = ((float)(z * CHUNK_DIM)) + zOffset;

                chunks[x,z].UpdatePosition(new Vector3(xPos, 0, zPos));
            }
        }
    }

    void LateUpdate()
    {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                chunks[x, y].Hide();
            }
        }

        int xIndex = (int) Math.Round((player.transform.position.x - xOffset) / CHUNK_DIM);
        int yIndex = (int) Math.Round((player.transform.position.z - zOffset) / CHUNK_DIM);

        if (xIndex >= 0 && xIndex < width && yIndex >= 0 && yIndex < height) {
            ShowChunksAround(xIndex, yIndex);
        }
    }

    void ShowChunksAround(int xIndex, int yIndex)
    {
        int minX = xIndex - viewAreaRadius;
        if (minX < 0) {
            minX = 0;
        }

        int maxX = xIndex + viewAreaRadius;
        if (maxX >= width) {
            maxX = width - 1;
        }

        int minY = yIndex - viewAreaRadius;
        if (minY < 0) {
            minY = 0;
        }

        int maxY = yIndex + viewAreaRadius;
        if (maxY >= height) {
            maxY = height - 1;
        }

        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                chunks[x, y].Show();
            }
        }

    }
}

public class MapChunk
{
    private float width, height;
    private Mesh mesh;
    private GameObject obj;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;


    public MapChunk(float width, float height, Material mat)
    {
        this.width = width;
        this.height = height;

        Vector3 topLeft = new Vector3(0 - width/2, 0, height/2);
        Vector3 topRight = new Vector3(width/2, 0, height/2);
        Vector3 bottomRight = new Vector3(width/2, 0, 0 - height/2);
        Vector3 bottomLeft = new Vector3(0 - width/2, 0, 0 - height/2);

        Vector3[] vertices = {topLeft, topRight, bottomRight, bottomLeft};

        int[] triangles = {0, 1, 3, 1, 2, 3};

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        obj = new GameObject();

        meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    public void UpdatePosition(Vector3 newPos)
    {
        obj.transform.position = newPos;
    }

    public void Hide()
    {
        obj.SetActive(false);
    }

    public void Show()
    {
        obj.SetActive(true);
    }

    public bool IsHidden()
    {
        return obj.activeSelf;
    }
}
