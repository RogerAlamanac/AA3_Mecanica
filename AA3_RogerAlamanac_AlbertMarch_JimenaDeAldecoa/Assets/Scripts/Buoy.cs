using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy : MonoBehaviour
{
    public Transform waterSurface;
    public float buoyancyForce = 10f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float waterHeight = GetWaterHeight(transform.position);
        if (transform.position.y < waterHeight)
        {
            float displacement = waterHeight - transform.position.y;
            rb.AddForce(Vector3.up * buoyancyForce * displacement, ForceMode.Acceleration);
        }
    }

    float GetWaterHeight(Vector3 position)
    {
        // Aquesta funció ha de ser implementada en funció del tipus d’ona (Gerstner o Sinusoidal)
        return waterSurface.GetComponent<WaveBase>().GetHeightAt(position);
    }
}
