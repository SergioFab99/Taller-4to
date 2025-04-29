// Assets/Scripts/ControladorAve.cs
using System.Collections;
using UnityEngine;

public class ControladorAve : MonoBehaviour
{
    public float velocidadVuelo = 2f;
    public float velocidadDescenso = 5f;
    public float duracionCazaMinima = 5f;
    public float duracionCazaMaxima = 8f;
    public float rangoRadioPoligono = 5f;
    public Transform jugador;
    public Transform[] plataformasObjetivo;

    private bool enDescenso = false;
    private bool jugadorAtrapado = false;
    private float alturaInicial;
    private Vector3[] verticesPoligono;
    private int indiceVerticeActual = 0;
    private Vector3 centro;

    void Start()
    {
        alturaInicial = transform.position.y;
        centro = new Vector3(transform.position.x, 0, transform.position.z);
        int lados = Random.Range(3, 10);
        verticesPoligono = CalcularVerticesPoligono(lados);
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
            yield return new WaitForSeconds(Random.Range(4f, 10f));
            enDescenso = true;
            jugadorAtrapado = false;

            Transform plataformaObjetivo = plataformasObjetivo[Random.Range(0, plataformasObjetivo.Length)];
            Vector3 objetivo = new Vector3(plataformaObjetivo.position.x, plataformaObjetivo.position.y, plataformaObjetivo.position.z);

            while (Vector3.Distance(transform.position, objetivo) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidadDescenso * Time.deltaTime);

                if (!jugadorAtrapado && Vector3.Distance(jugador.position, plataformaObjetivo.position) < 1.5f)
                {
                    jugadorAtrapado = true;
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

            enDescenso = false;
        }
    }
}
