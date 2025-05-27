using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DificultadManager : MonoBehaviour
{
    [Header("Inputs del jugador")]
    public float tiempoMaximo;            // Segundos para completar ronda
    public int maxSheeps;            // multiplicador maximo de las ovejas
    public float dificultadOvejas;       // Distancia media oveja-granja

    [Header("Dificultad Calculada")]
    [Range(0f, 1f)] public float dificultad; // 0 (fácil) a 1 (difícil) esto funciona con un selector
    public GameObject slider; 

    [Header("Ajustes aplicados")]
    [HideInInspector] public float velocidadOvejasBase = 1f;
    [HideInInspector] public float velocidadMaxima = 3f;
    [HideInInspector] public int lobosBase = 0;
    [HideInInspector] public int lobosMaximos = 5;


    public void CalcularDificultad()
    {
        // Fuzzificación
        float tiempoLento = Mathf.Clamp01((tiempoMaximo - 120f) / 180f);
        float tiempoRapido = Mathf.Clamp01((60f - tiempoMaximo) / 60f);
        float tiempoMedio = 1f - Mathf.Max(tiempoRapido, tiempoLento);

        float perdidasAltas = Mathf.Clamp01(maxSheeps / 5f);
        float perdidasBajas = 1f - perdidasAltas;

        float distanciaLejos = Mathf.Clamp01((dificultadOvejas - 10f) / 20f);

        // Reglas difusas (simplificadas)
        float dificultadAlta = Mathf.Max(
            Mathf.Min(tiempoLento, perdidasAltas),
            distanciaLejos
        );
        float dificultadBaja = Mathf.Min(tiempoRapido, perdidasBajas);
        float dificultadMedia = 1f - Mathf.Max(dificultadAlta, dificultadBaja);

        // Desfuzzificación
        dificultad = (
            dificultadBaja * 0.3f +
            dificultadMedia * 0.6f +
            dificultadAlta * 1f
        ) / (dificultadBaja + dificultadMedia + dificultadAlta);

    }

    public void ConectToSlider()
    {
        dificultad = slider.GetComponent<Scrollbar>().value;
        GameManager.instance._dificulty = dificultad;

        CalcularDificultad();
    }
}
