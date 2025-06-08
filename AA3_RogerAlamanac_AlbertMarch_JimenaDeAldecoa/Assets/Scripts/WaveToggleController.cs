using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Este script controla dos toggles (interruptores) de UI que permiten activar/desactivar dos tipos de olas: sinusoidales y de Gerstner
public class WaveToggleController : MonoBehaviour
{
    public Toggle sinusoidalToggle;  // Referencia al toggle de la onda sinusoidal
    public Toggle gerstnerToggle;    // Referencia al toggle de la onda de Gerstner

    public GameObject waterObject;   // Objeto que contiene los scripts de las olas

    private SinusoidalWave sinusoidalWave;  // Referencia al script de onda sinusoidal
    private GerstnerWave gerstnerWave;      // Referencia al script de onda de Gerstner

    void Start()
    {
        // Obtenemos las referencias a los scripts de ondas del objeto de agua
        if (waterObject != null)
        {
            sinusoidalWave = waterObject.GetComponent<SinusoidalWave>();
            gerstnerWave = waterObject.GetComponent<GerstnerWave>();
        }

        // Al inicio, activamos la onda sinusoidal y desactivamos la de Gerstner
        sinusoidalToggle.isOn = true;
        gerstnerToggle.isOn = false;
        ApplyToggleStates();

        // Asignamos los métodos a los eventos de cambio de valor de los toggles
        sinusoidalToggle.onValueChanged.AddListener(OnSinusoidalToggleChanged);
        gerstnerToggle.onValueChanged.AddListener(OnGerstnerToggleChanged);
    }

    void OnSinusoidalToggleChanged(bool isOn)
    {
        // Si el usuario intenta desactivar ambas olas, se vuelve a activar esta por defecto
        if (!isOn && !gerstnerToggle.isOn)
        {
            sinusoidalToggle.isOn = true;
            return;
        }

        // Si esta se activa, se desactiva la otra
        if (isOn)
        {
            gerstnerToggle.isOn = false;
        }

        ApplyToggleStates();
    }

    void OnGerstnerToggleChanged(bool isOn)
    {
        // Si el usuario intenta desactivar ambas olas, se vuelve a activar esta por defecto
        if (!isOn && !sinusoidalToggle.isOn)
        {
            gerstnerToggle.isOn = true;
            return;
        }

        // Si esta se activa, se desactiva la otra
        if (isOn)
        {
            sinusoidalToggle.isOn = false;
        }

        ApplyToggleStates();
    }

    // Este método aplica el estado de los toggles a los scripts de onda (activando o desactivando)
    void ApplyToggleStates()
    {
        if (sinusoidalWave != null)
            sinusoidalWave.enabled = sinusoidalToggle.isOn;
        if (gerstnerWave != null)
            gerstnerWave.enabled = gerstnerToggle.isOn;
    }
}
