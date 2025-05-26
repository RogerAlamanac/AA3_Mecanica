using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalWave : MonoBehaviour
{
    public float amplitude = 1f;
    public float wavelength = 2f;
    public float speed = 1f;
    public float phase = 0f;
    public Vector2 direction = new Vector2(1, 0);

    private Mesh mesh;
    private Vector3[] baseVertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        baseVertices = mesh.vertices;
    }

    void Update()
    {
        Vector3[] vertices = new Vector3[baseVertices.Length];
        float k = 2 * Mathf.PI / wavelength;
        float omega = speed * k;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = baseVertices[i];
            float dot = direction.normalized.x * v.x + direction.normalized.y * v.z;
            v.y = amplitude * Mathf.Sin(k * dot - omega * Time.time + phase);
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}

