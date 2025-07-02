using UnityEngine;
using TMPro;

public class ShowTime : MonoBehaviour
{
    // Referencia al TextMeshPro donde se mostrará el tiempo
    public TMP_Text timeText;
    
    // Referencia al TextMeshPro donde se mostrará el nombre de usuario
    public TMP_Text userText;

    // Se ejecuta al iniciar la escena
    void Start()
    {
        // Comprobar si la referencia de tiempo y nombre de usuario están asignadas
        if (timeText != null && userText != null)
        {
            // Mostrar el tiempo anterior guardado en Timevalue con 2 decimales
            timeText.text = "Time: " + Timer.Timevalue.ToString("F2");

            // Mostrar el nombre de usuario guardado en RegisterUser1 (es estático)
            userText.text = "Username: " + RegisterUser1.username;
        }
        else
        {
            // Mostrar un error si las referencias no están asignadas
            if (timeText == null)
                Debug.LogError("No se ha asignado el TMP_Text de tiempo en el inspector.");
            
            if (userText == null)
                Debug.LogError("No se ha asignado el TMP_Text de usuario en el inspector.");
        }
    }
}
