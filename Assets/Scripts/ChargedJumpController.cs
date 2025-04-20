using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float minJumpForce = 20f;
    public float maxJumpForce = 50f;
    public float chargeTime = 0.8f;
    public float fallMultiplier = 3f; // Realmente afecta la caída
    public string groundTag = "Ground";

    [Header("Camera Reference")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isCharging = false;
    private bool isGrounded = true;
    private float holdTime = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isGrounded && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
        }

        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
        }

        if (isCharging && Input.GetMouseButtonUp(0))
        {
            PerformJump();
        }
    }

    private void FixedUpdate()
    {
        // Aumenta la velocidad de caída de forma real
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }

    private void PerformJump()
    {
        float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / chargeTime);

        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;

        Vector3 jumpDirection = (forward + Vector3.up).normalized;

        rb.linearVelocity = Vector3.zero; // Cancelamos la inercia anterior
        rb.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);

        isCharging = false;
    }
}
