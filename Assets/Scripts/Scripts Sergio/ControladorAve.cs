using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorAve : MonoBehaviour
{
    public float velocidadVuelo = 2f;
    public float velocidadDescenso = 5f;
    public float duracionCazaMinima = 5f;
    public float duracionCazaMaxima = 8f;
    public float rangoRadioPoligono = 5f;
    public float radioCaptura = 5f;
    public Transform jugador;
    public Transform[] plataformasObjetivo;
    public AudioClip sonidoDescenso; // Audio que se reproduce al descender
    public float volumenSonido = 1f;

    private bool enDescenso = false;
    private bool jugadorAtrapado = false;
    private float alturaInicial;
    private Vector3[] verticesPoligono;
    private int indiceVerticeActual = 0;
    private Vector3 centro;
    private int indicePlataformaActual = 0;
    private AudioSource audioSource;
    private CharacterController controllerJugador;

    void Start()
    {
        alturaInicial = transform.position.y;
        centro = new Vector3(transform.position.x, 0, transform.position.z);
        int lados = Random.Range(3, 10);
        verticesPoligono = CalcularVerticesPoligono(lados);
        controllerJugador = jugador.GetComponent<CharacterController>();
        
        // Configurar AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volumenSonido;
        
        StartCoroutine(RutinaCaza());
    }

    void Update()
    {
        if (!enDescenso)
            VolarPoligono();
    }

    void VolarPoligono()
    {
        Vector3 destino = new Vector3(verticesPoligono[indiceVerticeActual].x, alturaInicial, verticesPoligono[indiceVerticeActual].z);
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidadVuelo * Time.deltaTime);

        if (Vector3.Distance(transform.position, destino) < 0.2f)
        {
            indiceVerticeActual = (indiceVerticeActual + 1) % verticesPoligono.Length;
        }
    }

    Vector3[] CalcularVerticesPoligono(int lados)
    {
        Vector3[] vertices = new Vector3[lados];
        float anguloIncremento = 360f / lados;

        for (int i = 0; i < lados; i++)
        {
            float anguloGrados = anguloIncremento * i;
            float rad = anguloGrados * Mathf.Deg2Rad;
            float x = centro.x + Mathf.Cos(rad) * rangoRadioPoligono;
            float z = centro.z + Mathf.Sin(rad) * rangoRadioPoligono;
            vertices[i] = new Vector3(x, 0f, z);
        }
        return vertices;
    }

    IEnumerator RutinaCaza()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            
            // Reproducir sonido de descenso
            if (sonidoDescenso != null)
            {
                audioSource.PlayOneShot(sonidoDescenso);
            }
            
            enDescenso = true;
            jugadorAtrapado = false;

            Transform plataformaObjetivo = plataformasObjetivo[indicePlataformaActual];
            Vector3 objetivo = plataformaObjetivo.position;

            while (Vector3.Distance(transform.position, objetivo) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidadDescenso * Time.deltaTime);

                if (!jugadorAtrapado && Vector3.Distance(transform.position, jugador.position) < radioCaptura)
                {
                    jugadorAtrapado = true;

                    // Desactiva control del jugador
                    if (controllerJugador != null)
                        controllerJugador.enabled = false;

                    StartCoroutine(ReiniciarEscenaTrasRetraso(3f));
                }

                if (jugadorAtrapado)
                {
                    Vector3 nuevaPosJugador = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    jugador.position = nuevaPosJugador;
                }

                yield return null;
            }

            float duracionCaza = Random.Range(duracionCazaMinima, duracionCazaMaxima);
            yield return new WaitForSeconds(duracionCaza);

            Vector3 alturaObjetivo = new Vector3(transform.position.x, alturaInicial, transform.position.z);
            while (transform.position.y < alturaInicial - 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, alturaObjetivo, velocidadDescenso * Time.deltaTime);

                if (jugadorAtrapado)
                {
                    Vector3 nuevaPosJugador = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
                    jugador.position = nuevaPosJugador;
                }

                yield return null;
            }

            indicePlataformaActual = (indicePlataformaActual + 1) % plataformasObjetivo.Length;

            jugadorAtrapado = false;
            enDescenso = false;
        }
    }

    IEnumerator ReiniciarEscenaTrasRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioCaptura);
    }
}