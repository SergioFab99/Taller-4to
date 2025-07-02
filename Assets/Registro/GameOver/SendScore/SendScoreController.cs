using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class SendScoreController : MonoBehaviour
{
    private const string url = "http://localhost/koi/insertkoi2.php";
    public void Execute(Action onCallback)
    {
        StartCoroutine(SendRequest(onCallback));
    }

    private IEnumerator SendRequest(Action onCallback)
    {
        WWWForm form =new WWWForm();
        form.AddField("tiempo", GameData.tiempo.ToString());
        form.AddField("usuario_id", GameData.usuario_id);
        form.AddField("nivel_id", GameData.nivel_id);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                onCallback?.Invoke();
            }
            else
            {
                Debug.Log("Error");
            }
        }
    }
}
