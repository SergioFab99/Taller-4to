using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuRegister : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego; // Cambia esto por el nombre real de tu escena

    public void Jugar()
    {
        // Validar que los campos de registro estén llenos
        if (string.IsNullOrWhiteSpace(RegisterUser1.username) ||
            string.IsNullOrWhiteSpace(RegisterUser1.lastname) ||
            string.IsNullOrWhiteSpace(RegisterUser1.email) ||
            string.IsNullOrWhiteSpace(RegisterUser1.password))
        {
            Debug.LogWarning("Por favor, completa todos los campos de registro antes de continuar.");
            return;
        }
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void CargarLogin()
    {
        // El botón de login no debe validar campos, solo cambiar de escena
        SceneManager.LoadScene("Login");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}