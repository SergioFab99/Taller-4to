using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Jump : MonoBehaviour
{
    public Slider powerBar;

    public float minJumpForce = 10f;
    public float maxJumpForce = 130f;
    public float chargeTime = 2f;
    public float fallMultiplier = 2f;
    public string groundTag = "Ground";
    public float raycastDistance = 1.2f;

    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isCharging = false;
    private bool isGrounded = false;
    private bool isWater = false;
    private float holdTime = 0f;

    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;
    private Vector3 platformDelta;

    // Rotación para carga y retorno
    private bool isReturning = false;
    private float chargeRotationDuration = 1f;
    private float jumpRotationDuration = 1f;
    private float rotationTimer = 0f;
    private float returnRotationTimer = 0f;
    private float startAngle = 0f;
    private float chargeEndAngle = -50f;
    private float jumpEndAngle = 0f;
    private float fixedRotationX = 0f;
public float verySlowRotationDuration = 4f; // Puedes ajustar esto, entre 4 y 6 segundos queda bien


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        powerBar.gameObject.SetActive(false);
        powerBar.value = 0f;

        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (isGrounded || isWater)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                holdTime = 0f;
                rotationTimer = 0f;
                powerBar.gameObject.SetActive(true);
                Debug.Log("Started charging, beginning rotation to -50");
            }

            if (isCharging && Input.GetMouseButton(0))
            {
                holdTime += Time.deltaTime * 2;
                holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
                powerBar.value = holdTime / chargeTime;

                rotationTimer += Time.deltaTime;
                float t = Mathf.Clamp01(rotationTimer / chargeRotationDuration);
                float currentAngle = Mathf.Lerp(startAngle, chargeEndAngle, t);

                Vector3 currentRotation = transform.localEulerAngles;
                transform.localRotation = Quaternion.Euler(currentAngle, currentRotation.y, currentRotation.z);

                Debug.Log($"Charge rotation: X = {currentAngle}, Y = {currentRotation.y}, Z = {currentRotation.z}");
            }

            if (isCharging && Input.GetMouseButtonUp(0))
            {
                isCharging = false;
                isReturning = true;
                returnRotationTimer = 0f;
                PerformJump();
                Debug.Log("Released click, beginning slow return rotation to 0");
            }
        }

        if (isReturning)
        {
            returnRotationTimer += Time.deltaTime;
            float t = Mathf.Clamp01(returnRotationTimer / jumpRotationDuration);
            float currentAngle = Mathf.Lerp(chargeEndAngle, jumpEndAngle, t);

            Vector3 currentRotation = transform.localEulerAngles;
            transform.localRotation = Quaternion.Euler(currentAngle, currentRotation.y, currentRotation.z);

            Debug.Log($"Return rotation: X = {currentAngle}, Y = {currentRotation.y}, Z = {currentRotation.z}");

            if (t >= 1f)
            {
                isReturning = false;
                transform.localRotation = Quaternion.Euler(jumpEndAngle, currentRotation.y, currentRotation.z);
                Debug.Log("Return rotation complete");
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(2);
        }

        if (isWater)
        {
            bar bb = FindFirstObjectByType<bar>();
            if (bb != null)
            {
                bb.RefillOxygen(100);
            }
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

        if (rb.linearVelocity.y < 0)
        {
            float gravityMultiplier = isWater ? 0.01f : fallMultiplier;
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
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

                    isReturning = false;
                    rotationTimer = 0f;
                    returnRotationTimer = 0f;

                    Vector3 currentRotation = transform.localEulerAngles;
                    transform.localRotation = Quaternion.Euler(0f, currentRotation.y, 0f);

                    Debug.Log("Landed, rotation reset");
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
                forward = forward.normalized;

                Vector3 jumpDirection = (forward + Vector3.up).normalized;

                rb.linearVelocity = Vector3.zero;
                rb.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);

                isGrounded = false;
                currentPlatform = null;

                transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

                Debug.Log("Jump performed");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("water in");
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
