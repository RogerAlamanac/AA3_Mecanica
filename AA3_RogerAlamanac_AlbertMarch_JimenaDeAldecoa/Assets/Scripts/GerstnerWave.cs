using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerstnerWave : MonoBehaviour
{
    public float amplitude = 1f;
    public float wavelength = 2f;
    public float speed = 1f;
    public float phase = 0f;
    public float steepness = 1f;
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
            float dot = Vector2.Dot(direction.normalized, new Vector2(v.x, v.z));
            float theta = k * dot - omega * Time.time + phase;
            float cosTheta = Mathf.Cos(theta);
            float sinTheta = Mathf.Sin(theta);

            float Q = steepness / (k * amplitude);

            v.x += Q * amplitude * direction.x * cosTheta;
            v.y = amplitude * sinTheta;
            v.z += Q * amplitude * direction.y * cosTheta;
            vertices[i] = v;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}

