using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public GameObject player;

    private MapChunk chunk;

    // Start is called before the first frame update
    void Start()
    {
        chunk = new MapChunk(10, 10);

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = chunk.mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class MapChunk
{
    private float width, height;
    public Mesh mesh;

    public MapChunk(float width, float height)
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
    }

}
