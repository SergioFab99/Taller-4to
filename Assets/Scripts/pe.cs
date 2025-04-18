using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pe : MonoBehaviour
{
    public float minJumpForce = 15f;
    public float maxJumpForce = 40f;
    public float chargeTime = 1f; 
    public string groundTag = "Ground";
    public float rotationSpeed = 100f;
    public Transform cameraTransform;

    private Rigidbody rb;
    public bool isGrounded = true;
    public float holdTime = 0f;
    private bool isCharging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isGrounded) 
        {
            if (Input.GetMouseButtonDown(0))
            {
                isCharging = true;
                holdTime = 0f;
            }    
        }

        if (Input.GetMouseButton(0) && isCharging)
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
        }

        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / chargeTime);

            Vector3 camDir = cameraTransform.forward;
            camDir.y = Mathf.Clamp(camDir.y, 0.1f, 1f); 
            Vector3 jumpDirection = camDir.normalized;

            rb.AddForce(jumpDirection * jumpStrength, ForceMode.Impulse);

            isCharging = false;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 10);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            isGrounded = false;
        }
    }
}
