using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Transform Koi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(Koi.position, transform.position));
        if (Vector3.Distance(Koi.position, transform.position) < 1.7f)
        {
            Koi.GetComponent<MeshRenderer>().enabled = false;
            // Aquí puedes agregar la lógica que deseas ejecutar cuando Koi esté cerca
        }
        else
        {
            Koi.GetComponent<MeshRenderer>().enabled = true;
            // Aquí puedes agregar la lógica que deseas ejecutar cuando Koi esté lejos
        }
    }
}
