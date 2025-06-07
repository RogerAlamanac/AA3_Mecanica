using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy : MonoBehaviour
{
    public float objectVolume = 1f;
    public float waterDensity = 1000f;
    public float gravity = 9.81f;

    public Transform waterSurface;
    private WaterMeshGenerator waterMesh;
    private Rigidbody rb;

    public float dampingCoefficient = 5f;  // Aumenta para más amortiguación
    public float maxVerticalSpeed = 2f;     // Limita la velocidad vertical

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (waterSurface != null)
        {
            waterMesh = waterSurface.GetComponent<WaterMeshGenerator>();
        }
    }

    void FixedUpdate()
    {
        if (waterMesh == null) return;

        Vector3 position = transform.position;
        float waterHeight = GetWaterHeightAtPosition(position);

        if (position.y < waterHeight)
        {
            float submergedDepth = waterHeight - position.y;
            float maxSubmergedDepth = transform.localScale.y;
            float submersion = Mathf.Clamp01(submergedDepth / maxSubmergedDepth);

            float displacedVolume = objectVolume * submersion;
            Vector3 buoyantForce = Vector3.up * waterDensity * gravity * displacedVolume;
            rb.AddForce(buoyantForce, ForceMode.Force);
        }

        // Aplicar damping vertical SIEMPRE (no solo sumergida)
        Vector3 verticalVelocity = Vector3.up * rb.velocity.y;
        Vector3 dampingForce = -verticalVelocity * dampingCoefficient;
        rb.AddForce(dampingForce, ForceMode.Force);

        // Limitar velocidad vertical máxima
        if (Mathf.Abs(rb.velocity.y) > maxVerticalSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxVerticalSpeed, rb.velocity.z);
        }
    }

    float GetWaterHeightAtPosition(Vector3 position)
    {
        Vector3[] vertices = waterMesh.GetVertices();
        float minDistance = Mathf.Infinity;
        float waterHeight = 0f;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldPos = waterSurface.TransformPoint(vertex);
            float distance = Vector2.Distance(new Vector2(worldPos.x, worldPos.z), new Vector2(position.x, position.z));

            if (distance < minDistance)
            {
                minDistance = distance;
                waterHeight = worldPos.y;
            }
        }

        return waterHeight;
    }
}
