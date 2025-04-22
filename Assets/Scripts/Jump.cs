using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jump : MonoBehaviour
{
    public Slider powerBar;

    public float minJumpForce = 20f;
    public float maxJumpForce = 50f;
    public float chargeTime = 0.1f;
    public float fallMultiplier = 3f;
    public string groundTag = "Ground";

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

        powerBar.gameObject.SetActive(false);
        powerBar.value = 0f;
    }

    private void Update()
    {
        if (isGrounded && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
            powerBar.gameObject.SetActive(true);
        }

        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
            powerBar.value = holdTime / chargeTime;
        }

        if (isCharging && Input.GetMouseButtonUp(0))
        {
            PerformJump();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;

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
    }
}
