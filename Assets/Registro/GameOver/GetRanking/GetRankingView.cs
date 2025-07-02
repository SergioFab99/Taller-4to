using UnityEngine;
using TMPro;

public class GetRankingView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankingText;

    private GetRankingController controller;

    private void Awake()
    {
        controller = GetComponent<GetRankingController>();
    }

    public void Execute()
    {
        controller.Execute(OnCallback);
    }

    private void OnCallback(RankingModel model)
    {
        if(model.mensaje=="Éxito")
        {
            foreach(UserRankingModel user in model.data)
            {
                rankingText.text += $"{user.NOMBRE_USUARIO} - {user.NOMBRE_NIVEL} - {user.TIEMPO_SEGUNDOS}\n";
            }
        }
        else
        {
            Debug.Log("Error");
        }
    }
}
