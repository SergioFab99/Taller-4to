using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instancia;
    private string escenaInicial;

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);
        escenaInicial = SceneManager.GetActiveScene().name;

        // Comienza la música si no estaba ya sonando
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
            audio.Play();

        // Suscribirse al cambio de escena
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento si este objeto se destruye
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene escenaAnterior, Scene escenaNueva)
    {
        // Si la escena nueva NO es la misma donde se creó la música, se destruye
        if (escenaNueva.name != escenaInicial)
        {
            Destroy(gameObject);
        }
    }
}
