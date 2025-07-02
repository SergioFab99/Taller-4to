using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetScoreController : MonoBehaviour
{
    private const string url = "http://localhost/koi/selectkoi.php";
    public void Execute(Action<GetScoreModel> onCallback)
    {
        StartCoroutine(SendRequest(onCallback));
    }

    private IEnumerator SendRequest(Action<GetScoreModel> onCallback)
    {
        WWWForm form =new WWWForm();
        form.AddField("username", GameData.username);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                GetScoreModel model = JsonUtility.FromJson<GetScoreModel>(www.downloadHandler.text);
                onCallback?.Invoke(model);
            }
            else
            {
                Debug.Log("Error");
            }
        }
    }
}
