// System.Collections, etc. van aquí
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerFeedbackManager : MonoBehaviour
{
    [Header("UI")]
    public Slider powerBar;

    [Header("Sonido")]
    public AudioClip chargingClip;
    public AudioClip jumpClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        if (powerBar != null)
        {
            powerBar.gameObject.SetActive(false);
            powerBar.value = 0f;
        }
    }

    public void StartChargeFeedback()
    {
        if (powerBar != null)
        {
            powerBar.gameObject.SetActive(true);
        }

        if (chargingClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = chargingClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void UpdateChargeFeedback(float chargePercentage)
    {
        if (powerBar != null)
        {
            powerBar.value = chargePercentage;
        }
    }

    public void ReleaseChargeFeedback()
    {
        if (powerBar != null)
        {
            powerBar.value = 0f;
            powerBar.gameObject.SetActive(false);
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }
}