using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Jump : MonoBehaviour
{
    [Header("UI & Sonido")]
    public Slider powerBar;
    public AudioClip chargingClip;
    public AudioClip jumpClip;

    [Header("Salto")]
    public float minJumpForce = 10f;
    public float maxJumpForce = 130f;
    public float chargeTime = 2f;
    public float fallMultiplier = 2f;
    public float raycastDistance = 1.2f;
    public string groundTag = "Ground";

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform body;    // Nuevo: el visual que rota

    private AudioSource audioSource;
    private Rigidbody rb;

    private bool isCharging = false;
    private bool isGrounded = false;
    private bool isWater = false;
    private float holdTime = 0f;

    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;
    private Vector3 platformDelta;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        powerBar.gameObject.SetActive(false);
        powerBar.value = 0f;

        rb.freezeRotation = true;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Iniciar carga de salto
        if ((isGrounded || isWater) && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
            powerBar.gameObject.SetActive(true);

            if (chargingClip != null && !audioSource.isPlaying)
            {
                audioSource.clip = chargingClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // Cargar fuerza
        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime * 2f;
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
            powerBar.value = holdTime / chargeTime;
        }

        // Ejecutar salto
        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            PerformJump();

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.loop = false;
            }
            if (jumpClip != null)
                audioSource.PlayOneShot(jumpClip);
        }

        // Reiniciar nivel
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Reabastecer oxígeno en agua
        if (isWater)
        {
            bar bb = FindFirstObjectByType<bar>();
            if (bb != null)
                bb.RefillOxygen(100);
        }

        // Rotación del modelo según velocidad real
        if (rb.linearVelocity.magnitude > 0.1f)
            body.forward = rb.linearVelocity.normalized;
        else
            body.forward = transform.forward;
    }

    void FixedUpdate()
    {
        // Gravedad extra
        rb.AddForce(Vector3.down * 100f);

        // Movimiento de plataformas móviles
        if (isGrounded && currentPlatform != null)
        {
            platformDelta = currentPlatform.position - lastPlatformPosition;
            rb.MovePosition(rb.position + platformDelta);
            lastPlatformPosition = currentPlatform.position;
        }

        // Aceleración de caída
        if (rb.linearVelocity.y < 0f)
        {
            float gravityMultiplier = isWater ? 0.01f : fallMultiplier;
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1f) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Solo verificamos si contacto con superficie con algo de normal vertical
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                if (!isGrounded)
                {
                    isGrounded = true;
                    powerBar.value = 0f;
                    powerBar.gameObject.SetActive(false);
                    currentPlatform = collision.transform;
                    lastPlatformPosition = currentPlatform.position;
                }
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == currentPlatform)
        {
            isGrounded = false;
            currentPlatform = null;
        }
    }

    private void PerformJump()
    {
        // Saltar si está en suelo o agua (no comprobamos raycast ni tag)
        if (isGrounded || isWater)
        {
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / chargeTime);

            Vector3 camForward = cameraTransform.forward;

            if (camForward.y < 0.5f)
            {
                Vector3 horizontal = new Vector3(camForward.x, 0f, camForward.z).normalized;
                camForward = (horizontal + Vector3.up * 0.5f).normalized;
            }
            else
            {
                camForward.Normalize();
            }

            Vector3 jumpForce = camForward * jumpStrength;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(jumpForce, ForceMode.Impulse);

            isGrounded = false;
            currentPlatform = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isWater = true;
            rb.linearDamping = 4f;
            rb.angularDamping = 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isWater = false;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
    }
}
