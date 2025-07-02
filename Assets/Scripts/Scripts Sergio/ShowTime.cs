using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    // Referencia al TextMeshPro donde se mostrará el tiempo
    public TMP_Text timeText;

    // Se ejecuta al iniciar la escena
    void Start()
    {
        // Comprobar si la referencia de text está asignada
        if (timeText != null)
        {
            // Mostrar el tiempo anterior guardado en Timevalue con 2 decimales
            timeText.text = "Time: " + Timer.Timevalue.ToString("F2");
        }
        else
        {
            Debug.LogError("No se ha asignado el TMP_Text en el inspector.");
        }
    }
}
