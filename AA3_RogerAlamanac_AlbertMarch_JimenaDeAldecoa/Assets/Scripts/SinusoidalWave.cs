using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script aplica una onda sinusoidal a la malla del agua para simular el movimiento de las olas
public class SinusoidalWave : MonoBehaviour
{
    // Parámetros de la onda
    public float amplitude = 1f;         // Altura de la ola
    public float wavelength = 5f;        // Longitud de la ola
    public float speed = 3f;             // Velocidad de propagación
    public float phase = 0f;             // Fase inicial
    public Vector2 direction = new Vector2(1, 0);  // Dirección en la que se propaga la ola (en el plano XZ)

    // Referencia a la malla y arrays de vértices
    private WaterMeshGenerator waterMesh;
    private Vector3[] baseVertices;      // Vértices originales sin deformar
    private Vector3[] displacedVertices; // Vértices deformados por la onda
    private float k;                     // Número de onda (k = 2π / λ)
    private float frequency;             // Frecuencia angular (ω = k * v)

    void Update()
    {
        // Inicialización al primer frame
        if (baseVertices == null)
        {
            waterMesh = GetComponent<WaterMeshGenerator>();
            baseVertices = waterMesh.GetVertices();
            if (baseVertices == null) return;

            displacedVertices = new Vector3[baseVertices.Length];

            // Se calculan los valores derivados de la onda
            k = 2 * Mathf.PI / wavelength;
            frequency = k * speed;
            direction.Normalize(); // Se normaliza la dirección para evitar escalado incorrecto
        }

        // Para cada vértice de la malla, se aplica la fórmula de la onda sinusoidal
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            // Proyección del vértice en la dirección de la onda (para soportar cualquier orientación)
            float x = vertex.x * direction.x + vertex.z * direction.y;

            // Se calcula la nueva altura 'y' usando la fórmula de la onda sinusoidal
            float y = amplitude * Mathf.Sin(k * (x - speed * Time.time) + phase);

            // Se actualiza el vértice con la nueva altura
            displacedVertices[i] = new Vector3(vertex.x, y, vertex.z);
        }

        // Se actualiza la malla con los nuevos vértices
        waterMesh.UpdateMesh(displacedVertices);
    }
}
