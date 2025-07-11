// System.Collections, etc. van aquí

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Física y Movimiento")]
    public float fallMultiplier = 2f;
    public Transform body; // El modelo visual que rota

    [Header("Interacción con Entorno")]
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private bar oxygenBar; // Referencia a la barra de oxígeno

    // Propiedades públicas para que otros scripts lean el estado
    public bool IsGrounded { get; private set; }
    public bool IsWater { get; private set; }

    private Rigidbody rb;
    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;
    private Vector3 platformDelta;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (oxygenBar == null)
            oxygenBar = FindFirstObjectByType<bar>();
    }

    void FixedUpdate()
    {
        // Gravedad extra solo si está cayendo y no está en agua
        if (rb.linearVelocity.y < 0f && !IsWater)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // Movimiento solidario con plataformas móviles, solo si el delta es significativo
        if (IsGrounded && currentPlatform != null)
        {
            platformDelta = currentPlatform.position - lastPlatformPosition;

            if (platformDelta.sqrMagnitude > 0.000001f) // umbral mínimo para evitar micro saltos
            {
                rb.MovePosition(rb.position + platformDelta);
            }

            lastPlatformPosition = currentPlatform.position;
        }

        // Gravedad constante (en agua es mínima)
        rb.AddForce(Vector3.down * (IsWater ? 10f : 100f));
    }

    void Update()
    {
        // Rotación del modelo visual hacia la dirección del movimiento
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            body.forward = rb.linearVelocity.normalized;
        }
        else
            body.forward = transform.forward;
    }

    // Método público para ser llamado por el Input Handler
    public void PerformJump(float force, Vector3 direction)
    {
        if (IsGrounded || IsWater)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(direction * force, ForceMode.Impulse);

            IsGrounded = false;
            currentPlatform = null;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(groundTag))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    if (!IsGrounded || currentPlatform != collision.transform)
                    {
                        IsGrounded = true;
                        currentPlatform = collision.transform;
                        lastPlatformPosition = currentPlatform.position;
                    }
                    return;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform == currentPlatform)
        {
            IsGrounded = false;
            currentPlatform = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            IsWater = true;
            rb.linearDamping = 4f;
            rb.angularDamping = 2f;

            if (oxygenBar != null)
                oxygenBar.RefillOxygen(100);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            IsWater = false;
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
    }
}
