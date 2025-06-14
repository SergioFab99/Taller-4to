using UnityEngine;
using System.Collections.Generic;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float sensitivityX = 3f;
    public float sensitivityY = 2f;
    public float maxDistance = 5f;
    public float defaultDistance = 3f; // Distancia por defecto cómoda
    public float minDistance = 0f; // Distancia mínima para acercarse
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
        currentDistance = defaultDistance; // Usar distancia por defecto en lugar del offset.magnitude

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

        // Control de rotación horizontal
        yaw += Input.GetAxis("Mouse X") * sensitivityX;

        // Control de rotación vertical y ajuste de distancia
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Ajuste de la distancia basado en la orientación vertical (pitch)
        AdjustCameraDistance(rotation);

        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 desiredPosition = lookPoint + rotation * offset.normalized * currentDistance;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * zoomSpeed);
        transform.LookAt(lookPoint);

        AdjustTargetTransparency();
    }

    void AdjustCameraDistance(Quaternion rotation)
    {
        float desiredDistance;

        // Invertir la lógica: si el pitch es mayor a 30 grados, acercarse al objetivo
        if (pitch > 30f) // Cuando apunta hacia arriba
        {
            // Acercarse al objetivo mientras se mantiene la orientación
            desiredDistance = Mathf.Min(maxDistance, currentDistance + (pitch - 30f) * 0.1f); // Acercarse con el aumento del pitch
        }
        else
        {
            // Para ángulos normales, alejarse
            desiredDistance = Mathf.Lerp(minDistance, defaultDistance, Mathf.InverseLerp(-60f, 30f, pitch));
        }

        // Verificar obstrucciones
        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 desiredCameraPos = lookPoint + rotation * offset.normalized * desiredDistance;

        Vector3 directionToCamera = (desiredCameraPos - lookPoint).normalized;
        Ray ray = new Ray(lookPoint, directionToCamera);
        RaycastHit hit;

        Debug.DrawRay(lookPoint, directionToCamera * obstructionCheckDistance, Color.red);

        if (Physics.Raycast(ray, out hit, obstructionCheckDistance, obstructionLayers))
        {
            if (!IsPartOfTarget(hit.collider.gameObject))
            {
                float distanceToObstruction = hit.distance;
                desiredDistance = Mathf.Min(desiredDistance, distanceToObstruction * 0.9f);
            }
        }

        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomSpeed);
    }

    bool IsPartOfTarget(GameObject obj)
    {
        if (target == null) return false;

        // Verificar si el objeto es el target o un hijo del target
        Transform checkTransform = obj.transform;
        while (checkTransform != null)
        {
            if (checkTransform == target)
                return true;
            checkTransform = checkTransform.parent;
        }
        return false;
    }

    void AdjustTargetTransparency()
    {
        if (targetRenderers == null) return;

        // Hacer transparente cuando la cámara está muy cerca
        float alpha = currentDistance < transparencyThreshold ? 0.3f : 1f; // Transparencia parcial en lugar de 0

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
