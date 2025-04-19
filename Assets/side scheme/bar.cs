using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class bar : MonoBehaviour
{
    public Image oxygenImage;
    public GameObject cat;
    public float maxOxygen = 100f;
    public float drainRate = 5f; 

    private float currentOxygen;

    void Start()
    {
        currentOxygen = maxOxygen;
        UpdateBar();
        cat.SetActive(false);
    }

    void Update()
    {
        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
        UpdateBar();

        if(currentOxygen <= 0)
        {
            cat.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
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
