using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMeshGenerator : MonoBehaviour
{
    public int xSize = 50;
    public int zSize = 50;
    public float gridSpacing = 1f;

    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
    }

    void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                i++;
            }
        }

        int[] triangles = new int[xSize * zSize * 6];
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public Vector3[] GetVertices()
    {
        return vertices;
    }

    public void UpdateMesh(Vector3[] updatedVertices)
    {
        mesh.vertices = updatedVertices;
        mesh.RecalculateNormals();
    }
}

