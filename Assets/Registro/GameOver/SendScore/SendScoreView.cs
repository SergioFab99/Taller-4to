using UnityEngine;
using UnityEngine.UI;

public class SendScoreView : MonoBehaviour
{
    [SerializeField] private Button sendScoreButton;
    private SendScoreController controller;

    private void Awake()
    {
        controller = GetComponent<SendScoreController>();
    }

    private void Start()
    {
        sendScoreButton.onClick.AddListener(SendScore);
    }

    private void SendScore()
    {
        controller.Execute(OnCallback);
    }

    private void OnCallback()
    {
        sendScoreButton.interactable = false;
        GetComponent<GetRankingView>().Execute();
    }
}
