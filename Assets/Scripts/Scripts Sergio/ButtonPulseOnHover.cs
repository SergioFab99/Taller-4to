using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonPulseOnHover: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Pulso")]
    public float pulseSpeed = 1f;
    public float pulseAmount = 1f;

    [Header("Sonido (opcional)")]
    public AudioClip hoverSound;
    private AudioSource audioSource;

    RectTransform rect;
    Vector3 originalScale;
    bool isHovering = false;
    float timer = 0f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (isHovering)
        {
            timer += Time.deltaTime * pulseSpeed;
            float scale = 0.6f + Mathf.Sin(timer) * pulseAmount;
            rect.localScale = originalScale * scale;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        timer = 0f;
        if (hoverSound != null) audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        rect.localScale = originalScale;
    }
}
