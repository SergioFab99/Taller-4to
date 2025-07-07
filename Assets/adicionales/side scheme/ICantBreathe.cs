using UnityEngine;

public class ICantBreathe : MonoBehaviour
{
    public float refillAmount = 100f;
    public AudioClip PopSound;
    private AudioSource audioSource;
    public float soundVolume = 0.5f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        bar bb = FindFirstObjectByType<bar>();
        if (bb != null)
        {
            audioSource.PlayOneShot(PopSound);
            bb.RefillOxygen(refillAmount);
            Destroy(gameObject);
        }
    }
}
