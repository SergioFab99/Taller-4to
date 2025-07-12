using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MiBombo2 : MonoBehaviour
{
    public GameObject deathScreen;
    public PlayerInputHandler theMove;

    public Transform startPos;
    public Transform idk;
    void Start()
    {
        deathScreen.SetActive(false);
        theMove.isFuckingDead = false;
        deathScreen.transform.position = startPos.position;
    }

    
    void Update()
    {
        
    }

    public void FuckingDie()
    {
        if (!theMove.isFuckingDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("plop");
            deathScreen.SetActive(true);
            Debug.Log("dead screen true");
            theMove.isFuckingDead = true;
            Debug.Log("no longer move");

            RectTransform deathScreenRect = deathScreen.GetComponent<RectTransform>();

            StartCoroutine(MoveDeathScreen(deathScreenRect, idk.GetComponent<RectTransform>().anchoredPosition));
        }
    }

    private IEnumerator MoveDeathScreen(RectTransform deathScreenRect, Vector2 targetPosition)
    {
        float duration = 1f; 
        Vector2 startPosition = deathScreenRect.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            deathScreenRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        deathScreenRect.anchoredPosition = targetPosition;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Coward()
    {
        SceneManager.LoadScene("Menú"); //who the fuck le puso tilde *mira a sergio*
    }
}
