using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    [SerializeField] private string nombreEscena; // Asigna el nombre de la escena en el Inspector

    public void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}