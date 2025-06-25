using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bar : MonoBehaviour
{
    public Image oxygenImage;
    public float maxOxygen = 100f;
    public float drainRate = 5f; 

    private float currentOxygen;

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateBar();
    }

    void Update()
    {
        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        UpdateBar();

        if(currentOxygen <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
        UpdateBar();
    }

    void UpdateBar()
    {
        oxygenImage.fillAmount = currentOxygen / maxOxygen;
    }
}
