using UnityEngine;
using UnityEngine.UI;

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
