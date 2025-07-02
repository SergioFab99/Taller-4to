using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetRankingController : MonoBehaviour
{
    private const string url = "http://localhost/koi/selectkoi2.php";
    public void Execute(Action<RankingModel> onCallback)
    {
        StartCoroutine(SendRequest(onCallback));
    }

    private IEnumerator SendRequest(Action<RankingModel> onCallback)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                RankingModel model = JsonUtility.FromJson<RankingModel>(www.downloadHandler.text);
                onCallback?.Invoke(model);
            }
            else
            {
                Debug.Log("Error");
            }
        }
    }
}
