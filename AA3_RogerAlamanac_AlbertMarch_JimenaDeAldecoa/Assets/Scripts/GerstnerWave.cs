using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerstnerWave : MonoBehaviour
{
    public float amplitude = 1f;       // A
    public float wavelength = 5f;      // L
    public float speed = 1f;           // v
    public float phase = 0f;           // 
    public float steepness = 0.5f;     // Q
    public Vector2 direction = new Vector2(1, 0); // D

    private WaterMeshGenerator waterMesh;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;
    private float k;        // wave number
    private float frequency; // angular frequency

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
            float d = vertex.x * direction.x + vertex.z * direction.y;
            float theta = k * (d - speed * Time.time) + phase;

            float cosTheta = Mathf.Cos(theta);
            float sinTheta = Mathf.Sin(theta);

            float x = vertex.x + steepness * amplitude * cosTheta * direction.x;
            float y = amplitude * sinTheta;
            float z = vertex.z + steepness * amplitude * cosTheta * direction.y;

            displacedVertices[i] = new Vector3(x, y, z);
        }

        waterMesh.UpdateMesh(displacedVertices);
    }
}

