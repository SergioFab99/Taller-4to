using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowTime : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text userText;
    public Button sendScoreButton;

    void Start()
    {
        if (timeText != null && userText != null)
        {
            timeText.text = "Time: " + Timer.Timevalue.ToString("F2");
            userText.text = "Username: " + RegisterUser1.username;
        }
        else
        {
            if (timeText == null)
                Debug.LogError("No se ha asignado el TMP_Text de tiempo en el inspector.");

            if (userText == null)
                Debug.LogError("No se ha asignado el TMP_Text de usuario en el inspector.");
        }

        if (sendScoreButton != null)
        {
            sendScoreButton.onClick.AddListener(CargarRanking);
        }
    }

    public void CargarRanking()
    {
        SceneManager.LoadScene("RankingUser");
    }
}
