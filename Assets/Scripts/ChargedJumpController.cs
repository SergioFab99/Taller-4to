using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargedJumpController : MonoBehaviour
{
    public Slider powerBar; // Drag and drop desde el Inspector

    [Header("Jump Settings")]
    public float minJumpForce = 20f;
    public float maxJumpForce = 50f;
    public float chargeTime = 0.1f;
    public float fallMultiplier = 3f; // Acelera caída
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

        powerBar.gameObject.SetActive(false); // Ocultar al inicio
        powerBar.value = 0f;
    }

    private void Update()
    {
        if (isGrounded && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
            powerBar.gameObject.SetActive(true); // Mostrar barra al empezar
        }

        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
            powerBar.value = holdTime / chargeTime; // Cargar visualmente
        }

        if (isCharging && Input.GetMouseButtonUp(0))
        {
            PerformJump();
        }
    }

    private void FixedUpdate()
    {
        // Aumenta la velocidad de caída si estás cayendo
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

            // Reiniciar la barra al tocar el suelo
            powerBar.value = 0f;
            powerBar.gameObject.SetActive(false);
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

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);

        isCharging = false;
        // La barra queda visible con el valor hasta que se toque suelo
    }
}
