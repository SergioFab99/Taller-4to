using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject container;

    private List<Narration> narrations;
    private float delayBetweenNarrations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUp(List<Narration> narrations, float delayBetweenNarrations = 0f)
    {
        this.narrations = narrations;
        this.delayBetweenNarrations = delayBetweenNarrations;
        StopAllCoroutines();
        StartCoroutine(PlayNarration());
    }

    private IEnumerator PlayNarration()
    {
        foreach (var narration in narrations)
        {
            audioSource.clip = narration.audioClip;
            audioSource.volume = narration.volume;
            audioSource.Play();

            container.SetActive(true);
            messageText.text = narration.text;

            yield return new WaitForSeconds(narration.audioClip.length + delayBetweenNarrations);

            container.SetActive(false);
        }
    }
}

[System.Serializable]
public class Narration
{
    public AudioClip audioClip;
    public string text;
    public float volume = 1f;
}
