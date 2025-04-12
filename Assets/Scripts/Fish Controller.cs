using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public float swimSpeed = 5f;
    public float turnSpeed = 50f;
    public float wobbleAmount = 10f;
    public float wobbleSpeed = 5f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private float yaw;
    private float pitch;
    private float wobbleTimer = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Movimiento ondulante (tipo Magikarp)
        wobbleTimer += Time.deltaTime * wobbleSpeed;
        float wobble = Mathf.Sin(wobbleTimer) * wobbleAmount;
        transform.Rotate(Vector3.up, wobble * Time.deltaTime);

        // Movimiento hacia adelante constante
        transform.Translate(Vector3.forward * swimSpeed * Time.deltaTime);

        // Control con flechas o WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, -vertical * turnSpeed * Time.deltaTime);

        // Movimiento de cámara con mouse
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -45f, 45f);

        cameraTransform.position = transform.position - transform.forward * 5f + Vector3.up * 2f;
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
