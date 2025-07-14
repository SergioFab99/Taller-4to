using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform Koi;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Verificar si Koi está asignado
        //if (Koi == null)
       // {
        //    Debug.LogError("El objeto Koi no está asignado.");
        //    return; // Detener ejecución si Koi no está asignado
       // }
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar si Koi está asignado antes de proceder
        if (Koi == null)
        {
            return; // Si Koi no está asignado, no hacemos nada
        }

        // Imprimir distancia entre el objeto y Koi
        Debug.Log(Vector3.Distance(Koi.position, transform.position));

        // Verificamos si el MeshRenderer está presente en el objeto Koi
        MeshRenderer koiRenderer = Koi.GetComponent<MeshRenderer>();
        
        if (koiRenderer != null)
        {
            if (Vector3.Distance(Koi.position, transform.position) < 1.7f)
            {
                koiRenderer.enabled = false; // Desactivamos el MeshRenderer cuando Koi esté cerca
            }
            else
            {
                koiRenderer.enabled = true; // Activamos el MeshRenderer cuando Koi esté lejos
            }
        }
        else
        {
            Debug.LogError("No se encontró el MeshRenderer en el objeto 'Koi'.");
        }
    }
}
