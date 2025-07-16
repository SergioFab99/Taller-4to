using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Start()
    {
        Invoke("WaitToEnd", 35);
    }

    void Update()
    {
        
    }

    public void WaitToEnd()
    {
        SceneManager.LoadScene("Menú");
    }
}
