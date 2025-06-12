using UnityEngine;
using System.Collections.Generic;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;
    public float minY = -30f;
    public float maxY = 89f; // ← Aumentado el ángulo hacia arriba
    public float minDistance = 0.5f;
    public float maxDistance = 5f;
    public float zoomSpeed = 15f;
    public float obstructionCheckDistance = 10f;
    public LayerMask obstructionLayers;
    public float targetHeightOffset = 1.5f;
    public float transparencyThreshold = 0.7f;
    public float transparencyLerpSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 15f;
    private float currentDistance;
    private Renderer[] targetRenderers;
    private Dictionary<Material, float> originalAlphas = new Dictionary<Material, float>();

    void Start()
    {
        currentDistance = offset.magnitude;

        if (obstructionLayers.value == 0)
        {
            obstructionLayers = LayerMask.GetMask("Default");
        }

        if (target != null)
        {
            targetRenderers = target.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in targetRenderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        originalAlphas[mat] = mat.color.a;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Debug opcional para verificar el pitch
        // Debug.Log("Pitch: " + pitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        AdjustCameraDistance(rotation);

        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 desiredPosition = lookPoint + rotation * offset.normalized * currentDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * zoomSpeed);
        transform.LookAt(lookPoint);

        AdjustTargetTransparency();
    }

    void AdjustCameraDistance(Quaternion rotation)
    {
        float t = Mathf.InverseLerp(minY, maxY, pitch); // ← Corrección
        float desiredDistance = Mathf.Lerp(minDistance, maxDistance, t); // ← Relación coherente

        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 desiredCameraPos = lookPoint + rotation * offset.normalized * desiredDistance;

        Vector3 directionToCamera = (desiredCameraPos - lookPoint).normalized;
        Ray ray = new Ray(lookPoint, directionToCamera);
        RaycastHit hit;

        Debug.DrawRay(lookPoint, directionToCamera * obstructionCheckDistance, Color.red);

        if (Physics.Raycast(ray, out hit, obstructionCheckDistance, obstructionLayers))
        {
            float distanceToObstruction = hit.distance;
            desiredDistance = Mathf.Min(desiredDistance, distanceToObstruction * 0.9f);
        }

        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomSpeed);
    }

    void AdjustTargetTransparency()
    {
        if (targetRenderers == null) return;

        float alpha = currentDistance < transparencyThreshold ? 0f : 1f;

        foreach (Renderer renderer in targetRenderers)
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color color = mat.color;
                    float targetAlpha = alpha * originalAlphas[mat];
                    color.a = Mathf.Lerp(color.a, targetAlpha, Time.deltaTime * transparencyLerpSpeed);
                    mat.color = color;

                    if (color.a < 1f && mat.renderQueue != 3000)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.renderQueue = 3000;
                    }
                    else if (color.a >= 1f && mat.renderQueue != 2000)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.renderQueue = 2000;
                    }
                }
            }
        }
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
