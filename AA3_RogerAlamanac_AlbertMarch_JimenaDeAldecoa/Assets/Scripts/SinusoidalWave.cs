using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalWave : MonoBehaviour
{
    public float amplitude = 1f;
    public float wavelength = 5f;
    public float speed = 3f;
    public float phase = 0f;
    public Vector2 direction = new Vector2(1, 0);

    private WaterMeshGenerator waterMesh;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;
    private float k;
    private float frequency;

    void Update()
    {
        if (baseVertices == null)
        {
            waterMesh = GetComponent<WaterMeshGenerator>();
            baseVertices = waterMesh.GetVertices();
            if (baseVertices == null) return;
            displacedVertices = new Vector3[baseVertices.Length];

            k = 2 * Mathf.PI / wavelength;
            frequency = k * speed;
            direction.Normalize();
        }

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            float x = vertex.x * direction.x + vertex.z * direction.y;
            float y = amplitude * Mathf.Sin(k * (x - speed * Time.time) + phase);
            displacedVertices[i] = new Vector3(vertex.x, y, vertex.z);
        }

        waterMesh.UpdateMesh(displacedVertices);
    }
}

