using UnityEngine;
using UnityEngine.UI;

public class bar : MonoBehaviour
{
    public Image oxygenImage;
    public float maxOxygen = 100f;
    public float drainRate = 5f;

    public MiBombo2 death;
    public bool drowning = true;

    private float currentOxygen;

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateBar();
    }

    void Update()
    {
        if(drowning)
        {
            currentOxygen -= drainRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
            UpdateBar();
            Debug.Log("aaaa help");
        }

        else if(!drowning)
        {
            currentOxygen += (drainRate * 5f) * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
            UpdateBar();
            Debug.Log("yey");
        }

        if(currentOxygen <= 0)
        {
            death.FuckingDie();
        }
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
        UpdateBar();
    }



    public void SetDrowningState(bool state)
    {
        drowning = state;
    }

    void UpdateBar()
    {
        oxygenImage.fillAmount = currentOxygen / maxOxygen;
    }
}
