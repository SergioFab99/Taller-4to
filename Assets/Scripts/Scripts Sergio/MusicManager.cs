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

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && !audio.isPlaying)
            audio.Play();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene escenaAnterior, Scene escenaNueva)
    {
        if (escenaNueva.name != escenaInicial)
        {
            Destroy(gameObject);
        }
    }
}
