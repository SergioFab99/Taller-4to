using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimation : MonoBehaviour
{
    public float charDelay = 0.1f;
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
        tmp.ForceMeshUpdate();
        TMP_TextInfo textInfo = tmp.textInfo;
        int totalChars = textInfo.characterCount;

        tmp.maxVisibleCharacters = 0;

        for (int i = 0; i < totalChars; i++)
        {
            if (textInfo.characterInfo[i].character == ' ')
            {
                tmp.maxVisibleCharacters = i + 1;
                continue;
            }

            tmp.maxVisibleCharacters = i + 1;

            tmp.ForceMeshUpdate();
            textInfo = tmp.textInfo;

            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertIndex = textInfo.characterInfo[i].vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[matIndex].colors32;

            if (i == 0)
            {
                for (int j = 0; j < 4; j++)
                    vertexColors[vertIndex + j].a = 255;
            }
            else
            {
                for (int j = 0; j < 4; j++)
                    vertexColors[vertIndex + j].a = 0;
            }

            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

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

            for (int j = 0; j < 4; j++)
                vertexColors[vertIndex + j].a = 255;
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield return new WaitForSeconds(charDelay);
        }
    }
}
