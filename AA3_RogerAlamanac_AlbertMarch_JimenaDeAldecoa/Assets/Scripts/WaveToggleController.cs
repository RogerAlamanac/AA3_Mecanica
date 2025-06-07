using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveToggleController : MonoBehaviour
{
    public Toggle sinusoidalToggle;
    public Toggle gerstnerToggle;

    public GameObject waterObject;

    private SinusoidalWave sinusoidalWave;
    private GerstnerWave gerstnerWave;

    void Start()
    {
        if (waterObject != null)
        {
            sinusoidalWave = waterObject.GetComponent<SinusoidalWave>();
            gerstnerWave = waterObject.GetComponent<GerstnerWave>();
        }

        // Asegurar que solo uno esté activo al inicio
        sinusoidalToggle.isOn = true;
        gerstnerToggle.isOn = false;
        ApplyToggleStates();

        // Suscribir eventos
        sinusoidalToggle.onValueChanged.AddListener(OnSinusoidalToggleChanged);
        gerstnerToggle.onValueChanged.AddListener(OnGerstnerToggleChanged);
    }

    void OnSinusoidalToggleChanged(bool isOn)
    {
        // Si el usuario intenta desactivar el toggle activo manualmente, lo volvemos a activar
        if (!isOn && !gerstnerToggle.isOn)
        {
            sinusoidalToggle.isOn = true;
            return;
        }

        if (isOn)
        {
            gerstnerToggle.isOn = false;
        }

        ApplyToggleStates();
    }

    void OnGerstnerToggleChanged(bool isOn)
    {
        // Si el usuario intenta desactivar el toggle activo manualmente, lo volvemos a activar
        if (!isOn && !sinusoidalToggle.isOn)
        {
            gerstnerToggle.isOn = true;
            return;
        }

        if (isOn)
        {
            sinusoidalToggle.isOn = false;
        }

        ApplyToggleStates();
    }

    void ApplyToggleStates()
    {
        if (sinusoidalWave != null)
            sinusoidalWave.enabled = sinusoidalToggle.isOn;
        if (gerstnerWave != null)
            gerstnerWave.enabled = gerstnerToggle.isOn;
    }
}

