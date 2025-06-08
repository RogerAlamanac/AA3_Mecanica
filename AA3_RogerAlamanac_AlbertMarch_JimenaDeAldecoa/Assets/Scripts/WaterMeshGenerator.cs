using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script genera una malla plana que representa la superficie del agua
public class WaterMeshGenerator : MonoBehaviour
{
    // Tamaño de la malla en X y Z (número de segmentos)
    public int xSize = 50;
    public int zSize = 50;

    // Distancia entre cada vértice del grid
    public float gridSpacing = 1f;

    // Referencia al objeto Mesh y a su array de vértices
    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        // Se crea una nueva malla y se asigna al componente MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Se genera la malla con los vértices y triángulos
        CreateMesh();
    }

    void CreateMesh()
    {
        // Se inicializan los vértices del plano de agua
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Cada vértice se coloca en su posición XZ, con Y = 0 (altura base)
                vertices[i] = new Vector3(x * gridSpacing, 0, z * gridSpacing);
                i++;
            }
        }

        // Se definen los triángulos que forman cada cuadrado del grid
        int[] triangles = new int[xSize * zSize * 6];
        for (int ti = 0, vi = 0, z = 0; z < zSize; z++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                // Dos triángulos por cuadrado (en sentido horario)
                triangles[ti] = vi;
                triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 2] = vi + 1;

                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }

        // Se asignan los vértices y triángulos a la malla
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // Se recalculan las normales para iluminación
    }

    // Método para obtener el array actual de vértices (usado por la boya)
    public Vector3[] GetVertices()
    {
        return vertices;
    }

    // Método para actualizar los vértices de la malla (usado por ondas)
    public void UpdateMesh(Vector3[] updatedVertices)
    {
        mesh.vertices = updatedVertices;
        mesh.RecalculateNormals(); // Se actualizan las normales tras mover los vértices
    }
}
