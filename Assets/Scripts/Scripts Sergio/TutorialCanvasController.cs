using UnityEngine;
using TMPro;

public class TutorialCanvasController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto1;
    [SerializeField] private TextMeshProUGUI texto2;
    [SerializeField] private TextMeshProUGUI texto3;

    [SerializeField] private float tiempoTexto1 = 3f;
    [SerializeField] private float tiempoTexto2 = 4f;
    [SerializeField] private float tiempoTexto3 = 5f;

    private void Start()
    {
        gameObject.SetActive(true);
        texto1.gameObject.SetActive(false);
        texto2.gameObject.SetActive(false);
        texto3.gameObject.SetActive(false);

        StartCoroutine(MostrarSecuencia());
    }

    private System.Collections.IEnumerator MostrarSecuencia()
    {
        texto1.gameObject.SetActive(true);
        yield return new WaitForSeconds(tiempoTexto1);
        texto1.gameObject.SetActive(false);

        texto2.gameObject.SetActive(true);
        yield return new WaitForSeconds(tiempoTexto2);
        texto2.gameObject.SetActive(false);

        texto3.gameObject.SetActive(true);
        yield return new WaitForSeconds(tiempoTexto3);
        texto3.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
