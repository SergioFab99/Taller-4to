using UnityEngine;

public class LiberarMouse: MonoBehaviour
{
    void Start()
    {
        // Mostrar el cursor
        Cursor.visible = true;

        // Liberar el cursor del centro de la pantalla
        Cursor.lockState = CursorLockMode.None;
    }
}
