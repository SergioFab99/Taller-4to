using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camcam : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;
    public float minY = -30f;
    public float maxY = 60f;

    private float yaw = 0f;
    private float pitch = 15f;

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 cameraPos = target.position + rotation * offset;

        transform.position = cameraPos;
        transform.LookAt(target.position + Vector3.up * 1.5f); 
    }

    public Vector3 GetFlatForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetLookDirection()
    {
        return transform.forward.normalized;
    }
}
