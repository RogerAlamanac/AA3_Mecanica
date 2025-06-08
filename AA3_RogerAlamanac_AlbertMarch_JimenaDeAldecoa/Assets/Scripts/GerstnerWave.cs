using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Este script simula olas utilizando la fórmula de Gerstner, que deforma la malla en las 3 dimensiones (x, y, z)
public class GerstnerWave : MonoBehaviour
{
    // Parámetros de la onda de Gerstner
    public float amplitude = 1f;       // A: Amplitud de la onda (altura máxima)
    public float wavelength = 5f;      // L: Longitud de onda
    public float speed = 3f;           // v: Velocidad de propagación
    public float phase = 0f;           // ϕ: Fase inicial
    public float steepness = 0.5f;     // Q: Inclinación de la ola (cuánto se desplazan horizontalmente los vértices)
    public Vector2 direction = new Vector2(1, 0); // D: Dirección de propagación en el plano XZ

    // Referencias a la malla y arrays de vértices
    private WaterMeshGenerator waterMesh;
    private Vector3[] baseVertices;       // Vértices originales de la malla
    private Vector3[] displacedVertices;  // Vértices desplazados con la onda
    private float k;        // Número de onda (k = 2π / L)
    private float frequency; // Frecuencia angular (ω = k * v)

    void Update()
    {
        // Inicialización de vértices y parámetros al primer frame
        if (baseVertices == null)
        {
            waterMesh = GetComponent<WaterMeshGenerator>();
            baseVertices = waterMesh.GetVertices();
            if (baseVertices == null) return;

            displacedVertices = new Vector3[baseVertices.Length];

            k = 2 * Mathf.PI / wavelength;     // Cálculo del número de onda
            frequency = k * speed;             // Cálculo de frecuencia angular
            direction.Normalize();             // Se normaliza la dirección
        }

        // Aplicación de la fórmula de Gerstner a cada vértice
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];

            // Proyección del vértice en la dirección de propagación de la ola
            float d = vertex.x * direction.x + vertex.z * direction.y;

            // Se calcula el ángulo theta en función del desplazamiento y el tiempo
            float theta = k * (d - speed * Time.time) + phase;

            // Se calculan las componentes trigonométricas
            float cosTheta = Mathf.Cos(theta);
            float sinTheta = Mathf.Sin(theta);

            // Se aplican los desplazamientos según la fórmula de Gerstner:
            // Movimiento en X y Z (horizontal) y en Y (altura)
            float x = vertex.x + steepness * amplitude * cosTheta * direction.x;
            float y = amplitude * sinTheta;
            float z = vertex.z + steepness * amplitude * cosTheta * direction.y;

            // Se almacena el nuevo vértice desplazado
            displacedVertices[i] = new Vector3(x, y, z);
        }

        // Se actualiza la malla con los nuevos vértices desplazados
        waterMesh.UpdateMesh(displacedVertices);
    }
}
