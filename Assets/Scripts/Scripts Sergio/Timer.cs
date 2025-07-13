using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static float Timevalue;
    public TMP_Text timerText;
    public PlayerInputHandler playerInputHandler;

    private float timeElapsed = 0f;
    private bool isTiming = false;

    void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        timeElapsed = 0f;
        isTiming = true;
    }

    public void StopTimer()
    {
        isTiming = false;
    }

    public void ResetTimer()
    {
        timeElapsed = 0f;
        timerText.text = "0.00";
    }

    void Update()
    {
        if (isTiming)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = timeElapsed.ToString("F2");
            Timer.Timevalue = timeElapsed;
            if (playerInputHandler.isFuckingDead)
            {
                StopTimer();
            }
        }
    }
}