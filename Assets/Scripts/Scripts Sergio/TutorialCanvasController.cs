using UnityEngine;
using TMPro;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField] private float displayTime = 10f; // Tiempo total visible
    [SerializeField] private TextMeshProUGUI texto1;
    [SerializeField] private TextMeshProUGUI texto2;

    private void Start()
    {
        gameObject.SetActive(true); // Mostrar el canvas
        texto1.gameObject.SetActive(false);
        texto2.gameObject.SetActive(false);
        StartCoroutine(MostrarSecuencia());
    }

    private System.Collections.IEnumerator MostrarSecuencia()
    {
        // Mostrar primer texto
        texto1.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime / 2f);

        // Ocultar primer texto, mostrar segundo
        texto1.gameObject.SetActive(false);
        texto2.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime / 2f);

        // Ocultar canvas completo
        gameObject.SetActive(false);
    }
}
