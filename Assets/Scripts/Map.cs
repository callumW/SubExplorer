using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public GameObject player;
    public Material mapMaterial;

    private MapChunk chunk;

    // Start is called before the first frame update
    void Start()
    {
        chunk = new MapChunk(10, 10, mapMaterial);
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

    public void updatePosition(Vector3 newPos)
    {
        obj.transform.position = newPos;
    }
}
