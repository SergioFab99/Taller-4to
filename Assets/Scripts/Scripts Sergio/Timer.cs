using UnityEngine;
using TMPro;
using UnityEngine.UIElements;  // Necesario para TextMeshPro

public class Timer : MonoBehaviour
{
    public static float Timevalue;
    public TMP_Text timerText;

    // Variables del temporizador
    private float timeElapsed = 0f;
    private bool isTiming = false;

    // Se ejecuta cuando la escena comienza
    void Start()
    {
        // Iniciar el timer automáticamente al cargar la escena
        StartTimer();
    }

    // Iniciar el timer
    public void StartTimer()
    {
        // Reinicia el tiempo
        timeElapsed = 0f;
        // Inicia el conteo
        isTiming = true;   
    }

    // Detener el timer
    public void StopTimer()
    {
        isTiming = false;  // Detiene el conteo
    }

    // Reiniciar el timer
    public void ResetTimer()
    {
        // Resetea el tiempo
        timeElapsed = 0f;  
        // Muestra el tiempo reseteado
        timerText.text = "0.00"; 
    }

    // Update se llama una vez por frame
    void Update()
    {
        if (isTiming)
        {
            // Aumenta el tiempo por el tiempo transcurrido desde el último frame
            timeElapsed += Time.deltaTime;
            // Muestra el tiempo con 2 decimales
            timerText.text = timeElapsed.ToString("F2");
        }
        Timevalue = timeElapsed;  // Actualiza la variable estática Timevalue
    }
}
