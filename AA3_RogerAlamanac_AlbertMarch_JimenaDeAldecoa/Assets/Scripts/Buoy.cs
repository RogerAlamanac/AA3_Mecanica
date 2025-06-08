using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que simula el comportamiento de una boya flotando sobre una malla de agua
public class Buoy : MonoBehaviour
{
    // Volumen del objeto (usado para calcular la fuerza de flotaci�n)
    public float objectVolume = 1f;

    // Densidad del agua (kg/m�) y aceleraci�n de la gravedad (m/s�)
    public float waterDensity = 1000f;
    public float gravity = 9.81f;

    // Referencia al objeto que representa la superficie del agua
    public Transform waterSurface;

    // Referencia al generador de la malla de agua
    private WaterMeshGenerator waterMesh;

    // Referencia al Rigidbody de la boya
    private Rigidbody rb;

    // Par�metros para amortiguaci�n y velocidad m�xima vertical
    public float dampingCoefficient = 5f;  // Aumenta para m�s amortiguaci�n
    public float maxVerticalSpeed = 2f;     // Limita la velocidad vertical

    void Start()
    {
        // Se obtiene el Rigidbody del objeto
        rb = GetComponent<Rigidbody>();

        // Se intenta obtener el script que genera la malla de agua
        if (waterSurface != null)
        {
            waterMesh = waterSurface.GetComponent<WaterMeshGenerator>();
        }
    }

    void FixedUpdate()
    {
        // Si no se ha encontrado el generador de malla, se aborta
        if (waterMesh == null) return;

        // Se calcula la posici�n actual y la altura del agua en ese punto
        Vector3 position = transform.position;
        float waterHeight = GetWaterHeightAtPosition(position);

        // Si la boya est� por debajo del agua, se calcula la flotaci�n
        if (position.y < waterHeight)
        {
            // Se calcula la profundidad sumergida y la fracci�n sumergida
            float submergedDepth = waterHeight - position.y;
            float maxSubmergedDepth = transform.localScale.y;
            float submersion = Mathf.Clamp01(submergedDepth / maxSubmergedDepth);

            // Se calcula el volumen desplazado y la fuerza de flotaci�n
            float displacedVolume = objectVolume * submersion;
            Vector3 buoyantForce = Vector3.up * waterDensity * gravity * displacedVolume;

            // Se aplica la fuerza de flotaci�n al Rigidbody
            rb.AddForce(buoyantForce, ForceMode.Force);
        }

        // Se aplica una fuerza de amortiguaci�n vertical, incluso si no est� sumergida
        Vector3 verticalVelocity = Vector3.up * rb.velocity.y;
        Vector3 dampingForce = -verticalVelocity * dampingCoefficient;
        rb.AddForce(dampingForce, ForceMode.Force);

        // Se limita la velocidad vertical para evitar saltos excesivos
        if (Mathf.Abs(rb.velocity.y) > maxVerticalSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxVerticalSpeed, rb.velocity.z);
        }
    }

    // Devuelve la altura del agua en una posici�n concreta, buscando el v�rtice m�s cercano de la malla
    float GetWaterHeightAtPosition(Vector3 position)
    {
        Vector3[] vertices = waterMesh.GetVertices();
        float minDistance = Mathf.Infinity;
        float waterHeight = 0f;

        // Se recorre cada v�rtice para encontrar el m�s cercano en XZ
        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldPos = waterSurface.TransformPoint(vertex);
            float distance = Vector2.Distance(new Vector2(worldPos.x, worldPos.z), new Vector2(position.x, position.z));

            // Se guarda la altura del v�rtice m�s cercano
            if (distance < minDistance)
            {
                minDistance = distance;
                waterHeight = worldPos.y;
            }
        }

        // Se devuelve la altura del v�rtice m�s cercano
        return waterHeight;
    }
}
