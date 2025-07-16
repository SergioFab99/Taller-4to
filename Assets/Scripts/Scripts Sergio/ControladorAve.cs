using System.Collections;
using UnityEngine;

public class ControladorAve : MonoBehaviour
{
    [Header("Configuración de Vuelo")]
    public float velocidadVuelo = 2f;
    public float rangoVueloHorizontal = 15f;
    public float rangoVueloVertical = 5f;
    public float tiempoVueloCircular = 5f;

    [Header("Configuración de Caza")]
    public float tiempoEntreCaza = 5f;
    public float radioDeteccion = 3f;

    [Header("Referencias")]
    public Transform[] plataformasObjetivo;

    private Vector3 puntoVueloObjetivo;
    private Vector3 centroVuelo;
    private float alturaVuelo;
    private bool enDescenso = false;
    private int indicePlataformaActual = 0;

    void Start()
    {
        alturaVuelo = transform.position.y;
        centroVuelo = new Vector3(transform.position.x, alturaVuelo, transform.position.z);
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
        float tiempo = Time.time / tiempoVueloCircular;
        float x = centroVuelo.x + Mathf.Sin(tiempo * Mathf.PI * 2) * rangoVueloHorizontal;
        float z = centroVuelo.z + Mathf.Cos(tiempo * Mathf.PI * 2) * rangoVueloHorizontal;
        float y = alturaVuelo + Mathf.Sin(tiempo * Mathf.PI) * rangoVueloVertical;
        puntoVueloObjetivo = new Vector3(x, y, z);
        transform.position = Vector3.MoveTowards(transform.position, puntoVueloObjetivo, velocidadVuelo * Time.deltaTime);
    }

    IEnumerator RutinaCaza()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreCaza);

            if (plataformasObjetivo == null || plataformasObjetivo.Length == 0)
            {
                Debug.LogWarning("No hay plataformas asignadas al ControladorAve.");
                continue;
            }

            yield return StartCoroutine(DescenderAPlataforma());
        }
    }

    IEnumerator DescenderAPlataforma()
    {
        enDescenso = true;

        Transform plataformaActual = plataformasObjetivo[indicePlataformaActual];
        Vector3 objetivoDescenso = plataformaActual.position;

        while (Vector3.Distance(transform.position, objetivoDescenso) > 0.1f)
        {
            Vector3 movimientoDescenso = Vector3.MoveTowards(transform.position, objetivoDescenso, velocidadVuelo * Time.deltaTime);
            movimientoDescenso.y = Mathf.Clamp(movimientoDescenso.y, float.MinValue, objetivoDescenso.y);
            transform.position = movimientoDescenso;
            yield return null;
        }

        DetectarJugadorEnPlataforma();

        if (enDescenso && plataformasObjetivo != null && plataformasObjetivo.Length > 0)
        {
            yield return StartCoroutine(RegresarAlturaVuelo());
        }

        indicePlataformaActual = (indicePlataformaActual + 1) % plataformasObjetivo.Length;

        enDescenso = false;
    }

    void DetectarJugadorEnPlataforma()
    {
        Collider[] objetosDetectados = Physics.OverlapSphere(transform.position, radioDeteccion);
        foreach (Collider obj in objetosDetectados)
        {
            if (obj.CompareTag("Player"))
            {
                Debug.Log("¡Jugador detectado en la plataforma!");
                return;
            }
        }
    }

    IEnumerator RegresarAlturaVuelo()
    {
        Vector3 objetivoRegreso = new Vector3(transform.position.x, alturaVuelo, transform.position.z);
        while (Vector3.Distance(transform.position, objetivoRegreso) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivoRegreso, velocidadVuelo * Time.deltaTime);
            yield return null;
        }
        GenerarNuevoCentroVuelo();
    }

    void GenerarNuevoCentroVuelo()
    {
        float x = Random.Range(transform.position.x - rangoVueloHorizontal, transform.position.x + rangoVueloHorizontal);
        float z = Random.Range(transform.position.z - rangoVueloHorizontal, transform.position.z + rangoVueloHorizontal);
        centroVuelo = new Vector3(x, alturaVuelo, z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioDeteccion);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centroVuelo, new Vector3(rangoVueloHorizontal * 2, 0.1f, rangoVueloHorizontal * 2));

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
