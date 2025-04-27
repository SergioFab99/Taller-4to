using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimation : MonoBehaviour
{
    [Tooltip("Tiempo entre que empieza el fade de un carácter y el siguiente")]
    public float charDelay = 0.1f;
    [Tooltip("Duración del fade-in de cada carácter")]
    public float fadeDuration = 0.3f;

    TextMeshProUGUI tmp;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        StartCoroutine(ShowTextWithFade());
    }

    IEnumerator ShowTextWithFade()
    {
        // Fuerza actualización para tener textInfo correcto
        tmp.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmp.textInfo;
        int totalChars = textInfo.characterCount;

        // Al inicio ocultamos todos los caracteres
        tmp.maxVisibleCharacters = 0;

        for (int i = 0; i < totalChars; i++)
        {
            // Comprobamos si es un espacio y no lo animamos
            if (textInfo.characterInfo[i].character == ' ')
            {
                tmp.maxVisibleCharacters = i + 1;
                continue; // Skip this character (space)
            }

            // Revela el carácter i
            tmp.maxVisibleCharacters = i + 1;

            // Necesitamos refrescar textInfo para acceder a los vértices recién visibles
            tmp.ForceMeshUpdate();
            textInfo = tmp.textInfo;

            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIndex = textInfo.characterInfo[i].vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[matIndex].colors32;

            // Inicialmente forzamos alpha = 0 para los 4 vértices de este carácter
            if (i == 0) // Esto garantiza que no haya parpadeo para la primera letra
            {
                for (int j = 0; j < 4; j++)
                    vertexColors[vertIndex + j].a = 255; // La primera letra ya está visible
            }
            else
            {
                for (int j = 0; j < 4; j++)
                    vertexColors[vertIndex + j].a = 0; // Los demás caracteres empiezan invisibles
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // Gradualmente subimos alpha de 0 → 255 para los caracteres posteriores
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                byte alpha = (byte)Mathf.Lerp(0, 255, elapsed / fadeDuration);
                for (int j = 0; j < 4; j++)
                    vertexColors[vertIndex + j].a = alpha;
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Aseguramos alpha = 255 al final
            for (int j = 0; j < 4; j++)
                vertexColors[vertIndex + j].a = 255;
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // Esperamos antes de pasar al siguiente carácter
            yield return new WaitForSeconds(charDelay);
        }
    }
}
