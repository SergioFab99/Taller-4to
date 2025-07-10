using UnityEngine;
using UnityEngine.SceneManagement;  // Necesario para cargar escenas
using UnityEngine.UI;              // Necesario para usar botones UI

public class SceneChanger: MonoBehaviour
{
    [SerializeField]
    private string nombreDeLaEscena = "NombreDeTuEscena"; // Cambia esto en el Inspector o por script

    [SerializeField]
    private Button boton;  // Arrastra aquí el botón desde el Inspector

    private void Start()
    {
        if (boton != null)
        {
            // Agregamos el listener para que ejecute la función cuando se haga clic
            boton.onClick.AddListener(CargarEscena);
        }
        else
        {
            Debug.LogWarning("No se asignó el botón en el inspector.");
        }
    }

    private void CargarEscena()
    {
        Debug.Log($"Cargando escena: {nombreDeLaEscena}");
        SceneManager.LoadScene(nombreDeLaEscena);
    }
}
