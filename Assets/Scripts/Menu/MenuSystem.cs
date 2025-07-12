using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private string nombreEscenaJuego = "Nivel 1"; // Cambia esto por el nombre real de tu escena
    [SerializeField] private string nombreEscenaJuego1 = "Creditos";
    [SerializeField] private string nombreEscenaJuego2;
    [SerializeField] private string EscenaMenu;

    public void Jugar()
    {
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void Creditos()
    {
        SceneManager.LoadScene(nombreEscenaJuego1);
    }

    public void Ranking()
    {
        SceneManager.LoadScene(nombreEscenaJuego2);
    }

     public void Menu()
    {
        SceneManager.LoadScene(EscenaMenu);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
