using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class suputa : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float rotationSpeed = 3f;

    private float currentYaw = 0f;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        currentYaw += mouseX;

        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 rotatedOffset = rotation * offset;

        transform.position = target.position + rotatedOffset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}