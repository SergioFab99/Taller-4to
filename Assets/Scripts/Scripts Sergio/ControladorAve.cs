using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorAve : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidadVuelo = 2f;
    public float velocidadDescenso = 5f;
    public float rangoVueloHorizontal = 15f;
    
    [Header("Configuración de Caza")]
    public float tiempoMinimoCaza = 5f;
    public float tiempoMaximoCaza = 8f;
    public float radioCaptura = 2f;
    
    [Header("Referencias")]
    public Transform jugador;
    
    [Header("Audio")]
    public AudioClip sonidoDescenso;
    public float volumenSonido = 1f;
    public float distanciaAudible = 15f;

    private bool enDescenso = false;
    private bool jugadorAtrapado = false;
    private Vector3 puntoVueloObjetivo;
    private Vector3 centroVuelo;
    private float alturaVuelo; // Altura original del ave
    private AudioSource audioSource;
    private CharacterController controllerJugador;
    private Coroutine rutinaCazaActual;

    void Start()
    {
        // Mantener la altura original del ave
        alturaVuelo = transform.position.y;
        
        // Establecer el centro de vuelo basado en la posición inicial
        centroVuelo = new Vector3(transform.position.x, alturaVuelo, transform.position.z);
        
        // Obtener referencias
        if (jugador != null)
            controllerJugador = jugador.GetComponent<CharacterController>();
        
        // Configurar AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volumenSonido;
        audioSource.spatialBlend = 1f; // Sonido 3D
        
        // Generar primer punto de vuelo aleatorio
        GenerarNuevoPuntoVueloRandom();
        
        // Iniciar rutina de caza
        rutinaCazaActual = StartCoroutine(RutinaCaza());
    }

    void Update()
    {
        if (!enDescenso)
        {
            VolarCompletamenteRandom();
        }
    }

    void VolarCompletamenteRandom()
    {
        // Mover hacia el punto objetivo
        transform.position = Vector3.MoveTowards(transform.position, puntoVueloObjetivo, velocidadVuelo * Time.deltaTime);

        // Si llegamos al punto objetivo, generar uno completamente aleatorio
        if (Vector3.Distance(transform.position, puntoVueloObjetivo) < 0.5f)
        {
            GenerarNuevoPuntoVueloRandom();
        }
    }

    void GenerarNuevoPuntoVueloRandom()
    {
        // Generar punto COMPLETAMENTE aleatorio en X y Z, mantener Y original
        float x = Random.Range(-rangoVueloHorizontal, rangoVueloHorizontal) + centroVuelo.x;
        float z = Random.Range(-rangoVueloHorizontal, rangoVueloHorizontal) + centroVuelo.z;
        
        puntoVueloObjetivo = new Vector3(x, alturaVuelo, z);
    }

    IEnumerator RutinaCaza()
    {
        while (true)
        {
            // Esperar tiempo aleatorio antes de cazar
            yield return new WaitForSeconds(Random.Range(tiempoMinimoCaza, tiempoMaximoCaza));
            
            // Verificar que el jugador existe
            if (jugador == null)
            {
                Debug.LogWarning("Jugador no asignado al ControladorAve");
                continue;
            }

            // Reproducir sonido solo si el jugador está cerca
            if (sonidoDescenso != null && Vector3.Distance(transform.position, jugador.position) <= distanciaAudible)
            {
                audioSource.PlayOneShot(sonidoDescenso);
            }

            // Iniciar descenso IMPARABLE hacia el jugador
            yield return StartCoroutine(DescenderHaciaJugadorImparable());
        }
    }

    IEnumerator DescenderHaciaJugadorImparable()
    {
        enDescenso = true;
        jugadorAtrapado = false;
        
        // EL AVE VA A LLEGAR AL JUGADOR SÍ O SÍ
        while (!jugadorAtrapado)
        {
            // Calcular posición objetivo (donde está el jugador AHORA)
            Vector3 objetivoDescenso = jugador.position;
            
            // Mover DIRECTO hacia el jugador sin importar nada
            transform.position = Vector3.MoveTowards(transform.position, objetivoDescenso, velocidadDescenso * Time.deltaTime);
            
            // Verificar si atrapamos al jugador
            float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
            
            if (distanciaAlJugador < radioCaptura)
            {
                jugadorAtrapado = true;
                
                // Desactivar control del jugador
                if (controllerJugador != null)
                    controllerJugador.enabled = false;
                
                // ATRAPAR AL JUGADOR Y LLEVARLO
                StartCoroutine(LlevarJugadorYDestruir());
            }
            
            yield return null;
        }
    }

    IEnumerator LlevarJugadorYDestruir()
    {
        // Llevar al jugador hacia arriba
        Vector3 destinoFinal = new Vector3(transform.position.x, alturaVuelo + 5f, transform.position.z);
        
        while (Vector3.Distance(transform.position, destinoFinal) > 0.1f)
        {
            // Mover el ave hacia arriba
            transform.position = Vector3.MoveTowards(transform.position, destinoFinal, velocidadDescenso * Time.deltaTime);
            
            // Mantener al jugador pegado al ave
            Vector3 nuevaPosJugador = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            jugador.position = nuevaPosJugador;
            
            yield return null;
        }
        
        // Esperar un poco y luego destruir
        yield return new WaitForSeconds(1f);
        
        // Reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar radio de captura
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioCaptura);
        
        // Dibujar área de vuelo random
        Gizmos.color = Color.blue;
        Vector3 centro = Application.isPlaying ? centroVuelo : new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Gizmos.DrawWireCube(centro, new Vector3(rangoVueloHorizontal * 2, 0.1f, rangoVueloHorizontal * 2));
        
        // Dibujar línea hacia punto objetivo de vuelo
        if (Application.isPlaying && !enDescenso)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, puntoVueloObjetivo);
        }
        
        // Si está en descenso, dibujar línea hacia el jugador
        if (Application.isPlaying && enDescenso && jugador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, jugador.position);
        }
    }

    // Método para pausar/reanudar la caza (útil para debugging)
    public void PausarCaza()
    {
        if (rutinaCazaActual != null)
        {
            StopCoroutine(rutinaCazaActual);
            rutinaCazaActual = null;
        }
    }

    public void ReanudarCaza()
    {
        if (rutinaCazaActual == null)
        {
            rutinaCazaActual = StartCoroutine(RutinaCaza());
        }
    }

    private void OnDestroy()
    {
        // Limpiar corrutinas al destruir el objeto
        if (rutinaCazaActual != null)
        {
            StopCoroutine(rutinaCazaActual);
        }
    }
}