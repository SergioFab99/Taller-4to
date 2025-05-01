using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Jump : MonoBehaviour
{
    public Slider powerBar;

    public float minJumpForce = 20f;
    public float maxJumpForce = 50f;
    public float chargeTime = 0.1f;
    public float fallMultiplier = 3f;
    public string groundTag = "Ground";
    public float raycastDistance = 1.2f;

    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isCharging = false;
    private bool isGrounded = false;
    private float holdTime = 0f;

    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;
    private Vector3 platformDelta;

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
        //eliminar despues
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 100);

        if (isGrounded && currentPlatform != null)
        {
            platformDelta = currentPlatform.position - lastPlatformPosition;
            rb.MovePosition(rb.position + platformDelta);
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(groundTag))
            return;

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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / chargeTime);

                Vector3 forward = cameraTransform.forward;
                forward.y = 0f;

                Vector3 jumpDirection = (forward + Vector3.up).normalized;

                rb.linearVelocity = Vector3.zero;
                rb.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);

                isCharging = false;
                isGrounded = false;
                currentPlatform = null;
            }
        }
    }
}
