using UnityEngine;
using TMPro;

public class GetScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI oldScoreText;
    [SerializeField] private TextMeshProUGUI newScoreText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    
    private GetScoreController controller;

    private void Awake()
    {
        newScoreText.text = $"New Score: {GameData.score}";
        playerNameText.text = $"Player Name: {GameData.username}";
        oldScoreText.text = $"Old Score: Loading...";
    }

    private void Start()
    {
        controller.Execute(OnCallback);
    }

    private void OnCallback(GetScoreModel model)
    {
        if(model.mensaje=="Éxito")
        {
            oldScoreText.text = $"Old Score: {model.data.TIEMPO_SEGUNDOS}";
        }
        else
        {
            Debug.Log("Error");
        }
    }
}
