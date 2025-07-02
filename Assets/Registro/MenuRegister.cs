using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuRegister : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego; // Cambia esto por el nombre real de tu escena

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
