using System.Collections;
using UnityEngine;

public class ControladorAve : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidadVuelo = 2f;
    public float rangoVueloHorizontal = 15f;
    public float rangoVueloVertical = 5f;  // Rango vertical para simular el vuelo en círculos o dentro de un cuadrado
    public float tiempoVueloCircular = 5f; // Tiempo para completar un ciclo de vuelo circular

    [Header("Configuración de Caza")]
    public float tiempoEntreCaza = 5f;  // Tiempo entre cada caza
    public float radioDeteccion = 3f;  // Radio de detección para el jugador

    [Header("Referencias")]
    public Transform[] plataformasObjetivo;  // Las plataformas hacia donde el ave debe descender

    private Vector3 puntoVueloObjetivo;
    private Vector3 centroVuelo;  // El centro de vuelo (donde se mueve el ave)
    private float alturaVuelo;  // Altura original de vuelo
    private bool enDescenso = false;  // Indica si el ave está descendiendo
    private int indicePlataformaActual = 0;  // Indice de la plataforma actual hacia la cual el ave desciende

    void Start()
    {
        // Inicializamos la altura y el centro de vuelo
        alturaVuelo = transform.position.y;
        centroVuelo = new Vector3(transform.position.x, alturaVuelo, transform.position.z);
        
        // Iniciar rutina de caza
        StartCoroutine(RutinaCaza());
    }

    void Update()
    {
        if (!enDescenso)
        {
            VolarEnPatronCircular();
        }
    }

    void VolarEnPatronCircular()
    {
        // Movimiento circular dentro del área delimitada
        float tiempo = Time.time / tiempoVueloCircular;  // Usamos el tiempo para hacer que el ave vuele de forma circular

        // Calcular las posiciones X, Z y Y utilizando funciones seno y coseno
        float x = centroVuelo.x + Mathf.Sin(tiempo * Mathf.PI * 2) * rangoVueloHorizontal;
        float z = centroVuelo.z + Mathf.Cos(tiempo * Mathf.PI * 2) * rangoVueloHorizontal;
        float y = alturaVuelo + Mathf.Sin(tiempo * Mathf.PI) * rangoVueloVertical; // Oscilación vertical

        puntoVueloObjetivo = new Vector3(x, y, z);

        // Mover hacia el nuevo punto objetivo
        transform.position = Vector3.MoveTowards(transform.position, puntoVueloObjetivo, velocidadVuelo * Time.deltaTime);
    }

    IEnumerator RutinaCaza()
    {
        while (true)
        {
            // Esperar un tiempo antes de intentar cazar
            yield return new WaitForSeconds(tiempoEntreCaza);

            // Asegurarse de que hay plataformas disponibles
            if (plataformasObjetivo == null || plataformasObjetivo.Length == 0)
            {
                Debug.LogWarning("No hay plataformas asignadas al ControladorAve.");
                continue;
            }

            // Iniciar el descenso hacia la plataforma
            yield return StartCoroutine(DescenderAPlataforma());
        }
    }

    IEnumerator DescenderAPlataforma()
    {
        enDescenso = true;

        // Obtener la plataforma actual a la cual el ave debe descender
        Transform plataformaActual = plataformasObjetivo[indicePlataformaActual];
        Vector3 objetivoDescenso = plataformaActual.position;

        // Descender hacia la plataforma
        while (Vector3.Distance(transform.position, objetivoDescenso) > 0.1f)
        {
            // Mover hacia la plataforma
            Vector3 movimientoDescenso = Vector3.MoveTowards(transform.position, objetivoDescenso, velocidadVuelo * Time.deltaTime);
            movimientoDescenso.y = Mathf.Clamp(movimientoDescenso.y, float.MinValue, objetivoDescenso.y); // No mover más allá del eje Y de la plataforma
            transform.position = movimientoDescenso;
            yield return null;
        }

        // Después de aterrizar en la plataforma, detectar al jugador
        DetectarJugadorEnPlataforma();

        // Si hay un jugador en la plataforma, proceder a llevarlo arriba
        if (enDescenso && plataformasObjetivo != null && plataformasObjetivo.Length > 0)
        {
            yield return StartCoroutine(RegresarAlturaVuelo());
        }

        // Cambiar a la siguiente plataforma después de un descenso
        indicePlataformaActual = (indicePlataformaActual + 1) % plataformasObjetivo.Length;

        enDescenso = false;
    }

    void DetectarJugadorEnPlataforma()
    {
        // Detecta al jugador dentro de un radio definido
        Collider[] objetosDetectados = Physics.OverlapSphere(transform.position, radioDeteccion);
        foreach (Collider obj in objetosDetectados)
        {
            if (obj.CompareTag("Player"))
            {
                Debug.Log("¡Jugador detectado en la plataforma!");
                // Llamar función para interactuar con el jugador, si es necesario
                return;
            }
        }
    }

    IEnumerator RegresarAlturaVuelo()
    {
        // Regresar el ave a su altura original
        Vector3 objetivoRegreso = new Vector3(transform.position.x, alturaVuelo, transform.position.z);

        while (Vector3.Distance(transform.position, objetivoRegreso) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivoRegreso, velocidadVuelo * Time.deltaTime);
            yield return null;
        }

        // Después de regresar a la altura de vuelo, generar un nuevo centro aleatorio para seguir el patrón circular
        GenerarNuevoCentroVuelo();
    }

    void GenerarNuevoCentroVuelo()
    {
        // Generar un nuevo centro de vuelo aleatorio dentro del área de vuelo
        float x = Random.Range(centroVuelo.x - rangoVueloHorizontal, centroVuelo.x + rangoVueloHorizontal);
        float z = Random.Range(centroVuelo.z - rangoVueloHorizontal, centroVuelo.z + rangoVueloHorizontal);
        centroVuelo = new Vector3(x, alturaVuelo, z);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el área de vuelo y el radio de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centroVuelo, new Vector3(rangoVueloHorizontal * 2, 0.1f, rangoVueloHorizontal * 2));

        // Dibujar las plataformas
        if (plataformasObjetivo != null)
        {
            Gizmos.color = Color.cyan;
            foreach (Transform plataforma in plataformasObjetivo)
            {
                Gizmos.DrawWireSphere(plataforma.position, 0.5f);
            }
        }
    }
}
