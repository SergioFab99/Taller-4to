using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    public float fallMultiplier = 2f;
    public Transform body;
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private bar oxygenBar;
    [SerializeField] public bool IsGrounded { get; private set; }
    public bool IsWater { get; private set; }

    Rigidbody rb;
    Transform currentPlatform;
    Vector3 lastPlatformPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (oxygenBar == null)
            oxygenBar = FindFirstObjectByType<bar>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0f && !IsWater)
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;

        if (IsGrounded && currentPlatform != null)
        {
            Vector3 platformDelta = currentPlatform.position - lastPlatformPosition;
            if (platformDelta.sqrMagnitude > 0.000001f)
                rb.MovePosition(rb.position + platformDelta);
            lastPlatformPosition = currentPlatform.position;
        }

        rb.AddForce(Vector3.down * (IsWater ? 10f : 100f));
    }

    void Update()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        if (forward.magnitude > 0.1f)
            body.forward = forward.normalized;
    }

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

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag(groundTag) || collision.transform.CompareTag("WATAAAAAAA"))
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
                        oxygenBar.SetDrowningState(!collision.transform.CompareTag("WATAAAAAAA"));
                    }
                    return;
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform == currentPlatform)
        {
            IsGrounded = false;
            currentPlatform = null;
            oxygenBar.SetDrowningState(true);
        }
    }
}
