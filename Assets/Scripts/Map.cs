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

    public int viewAreaRadius;

    private float xOffset;
    private float zOffset;

    private uint widthInChunks;
    private uint heightInChunks;

    private MapChunk[,] chunks;

    // Start is called before the first frame update
    void Start()
    {
        TerrainGenerator gen = GetComponent<TerrainGenerator>();

        float[,] data = gen.GenerateHeightmap();

        // 1. Work out chunk size
        widthInChunks = (uint) data.GetLength(0) / (uint) CHUNK_DIM;
        heightInChunks = (uint) data.GetLength(1) / (uint) CHUNK_DIM;

        chunks = new MapChunk[widthInChunks, heightInChunks];

        xOffset = 0 - ((widthInChunks - 1) * CHUNK_DIM) / 2;
        zOffset = 0 - ((heightInChunks - 1) * CHUNK_DIM) / 2;

        // 2. iterate over chunks and create MapChunk from it.
        for (uint x = 0; x < widthInChunks; x++) {
            for (uint y = 0; y < heightInChunks; y++) {

                uint startX = x * (uint) CHUNK_DIM;
                uint startY = y * (uint) CHUNK_DIM;

                uint meshWidth = (uint) CHUNK_DIM + 1;
                uint meshHeight = (uint) CHUNK_DIM + 1;

                if (startX > 0) {   // Make meshes meet
                    startX--;
                    //meshWidth++;
                }

                if (startY > 0) {
                    startY--;
                    //meshHeight++;
                }

                chunks[x,y] = new MapChunk(data, startX, startY, meshWidth, meshHeight, mapMaterial);

                float xPos = ((float)(x * CHUNK_DIM)) + xOffset;
                float zPos = ((float)(y * CHUNK_DIM)) + zOffset;

                chunks[x,y].UpdatePosition(new Vector3(xPos, 0, zPos));

                Debug.Log("adding chunk to: (" + x + ", " + y + ")");
            }
        }
    }

    void LateUpdate()
    {
        for (int x = 0; x < widthInChunks; x++) {
            for (int y = 0; y < heightInChunks; y++) {
                chunks[x, y].Hide();
            }
        }

        int xIndex = (int) Math.Round((player.transform.position.x - xOffset) / CHUNK_DIM);
        int yIndex = (int) Math.Round((player.transform.position.z - zOffset) / CHUNK_DIM);

        if (xIndex >= 0 && xIndex < widthInChunks && yIndex >= 0 && yIndex < heightInChunks) {
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
        if (maxX >= widthInChunks) {
            maxX = (int) widthInChunks - 1;
        }

        int minY = yIndex - viewAreaRadius;
        if (minY < 0) {
            minY = 0;
        }

        int maxY = yIndex + viewAreaRadius;
        if (maxY >= heightInChunks) {
            maxY = (int) heightInChunks - 1;
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
    private MeshCollider meshCollider;


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

        meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    public MapChunk(float[,] data, uint xStart, uint yStart, uint width, uint height, Material mat)
    {

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (width - 1) / -2f;

        Vector3[] vertices = new Vector3[width * height];
        int[] triangles = new int[(width-1) * (height-1) * 6];

        int verticesPerLine = (int) width - 1;

        uint triangleIndex = 0;
        uint vertexIndex = 0;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                vertices[x * height + y] = new Vector3(x + topLeftX, data[x + xStart, y + yStart], y - topLeftZ);

                if (x < width - 1 && y < height - 1) {
                    triangles[triangleIndex] = (int) (x * height + (y + 1));
                    triangles[triangleIndex+1] = (int) ((x + 1) * height + (y + 1));
                    triangles[triangleIndex+2] = (int) (x * height + y);

                    triangleIndex += 3;

                    triangles[triangleIndex] = (int) ((x + 1) * height + (y + 1));
                    triangles[triangleIndex+1] = (int) ((x + 1) * height + y);
                    triangles[triangleIndex+2] = (int) (x * height + y);

                    triangleIndex += 3;
                }

                vertexIndex++;
            }
        }

        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        obj = new GameObject();

        meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = mat;

        meshFilter = obj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

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
