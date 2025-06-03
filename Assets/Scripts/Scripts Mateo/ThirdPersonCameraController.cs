using UnityEngine;
using System.Collections.Generic;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform target; // Transform del objeto a seguir (por ejemplo, el jugador)
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset inicial de la cámara
    public float sensitivityX = 3f; // Sensibilidad del ratón en el eje X
    public float sensitivityY = 2f; // Sensibilidad del ratón en el eje Y
    public float minY = -30f; // Ángulo vertical mínimo
    public float maxY = 60f; // Ángulo vertical máximo
    public float minDistance = 0.5f; // Distancia mínima de la cámara al target
    public float maxDistance = 5f; // Distancia máxima de la cámara al target
    public float zoomSpeed = 15f; // Velocidad de ajuste del zoom
    public float obstructionCheckDistance = 10f; // Distancia para verificar obstrucciones
    public LayerMask obstructionLayers; // Capas que se consideran obstrucciones
    public float targetHeightOffset = 1.5f; // Offset vertical para el punto de enfoque
    public float transparencyThreshold = 0.7f; // Distancia a la que el target se vuelve transparente
    public float transparencyLerpSpeed = 5f; // Velocidad de transición de transparencia

    private float yaw = 0f;
    private float pitch = 15f;
    private float currentDistance; // Distancia actual de la cámara al target
    private Renderer[] targetRenderers; // Almacena los renderers del target
    private Dictionary<Material, float> originalAlphas = new Dictionary<Material, float>(); // Almacena alfas originales

    void Start()
    {
        // Inicializar la distancia actual con la magnitud del offset
        currentDistance = offset.magnitude;

        // Asegurarse de que el LayerMask incluya la capa "Default"
        if (obstructionLayers.value == 0)
        {
            obstructionLayers = LayerMask.GetMask("Default");
        }

        // Obtener todos los renderers del target
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

        // Actualizar yaw y pitch según el input del ratón
        yaw += Input.GetAxis("Mouse X") * sensitivityX;
        pitch -= Input.GetAxis("Mouse Y") * sensitivityY;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Calcular la rotación de la cámara
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Ajustar la distancia de la cámara según el ángulo vertical y obstrucciones
        AdjustCameraDistance(rotation);

        // Calcular la posición deseada de la cámara
        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 desiredPosition = lookPoint + rotation * offset.normalized * currentDistance;

        // Mover la cámara suavemente hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * zoomSpeed);

        // Hacer que la cámara mire al target con un offset vertical
        transform.LookAt(lookPoint);

        // Ajustar transparencia del target según la distancia
        AdjustTargetTransparency();
    }

    void AdjustCameraDistance(Quaternion rotation)
    {
        // Calcular el ángulo vertical relativo
        float verticalAngle = pitch;

        // Mapear el ángulo vertical para ajustar la distancia
        float t = Mathf.InverseLerp(0f, minY, verticalAngle);
        float desiredDistance = Mathf.Lerp(maxDistance, minDistance, t);

        // Calcular el punto de enfoque del target
        Vector3 lookPoint = target.position + Vector3.up * targetHeightOffset;

        // Calcular la posición deseada de la cámara
        Vector3 desiredCameraPos = lookPoint + rotation * offset.normalized * desiredDistance;

        // Lanzar el raycast desde el target hacia la posición deseada de la cámara
        Vector3 directionToCamera = (desiredCameraPos - lookPoint).normalized;
        Ray ray = new Ray(lookPoint, directionToCamera);
        RaycastHit hit;

        // Debug para visualizar el raycast
        Debug.DrawRay(lookPoint, directionToCamera * obstructionCheckDistance, Color.red);

        if (Physics.Raycast(ray, out hit, obstructionCheckDistance, obstructionLayers))
        {
            // Si hay una obstrucción, ajustar la distancia
            float distanceToObstruction = hit.distance;
            desiredDistance = Mathf.Min(desiredDistance, distanceToObstruction * 0.9f);
        }

        // Actualizar la distancia actual suavemente
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

                    // Cambiar el modo de renderizado si es necesario
                    if (color.a < 1f && mat.renderQueue != 3000)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        mat.SetInt("_ZWrite", 0);
                        mat.renderQueue = 3000; // Transparent queue
                    }
                    else if (color.a >= 1f && mat.renderQueue != 2000)
                    {
                        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        mat.SetInt("_ZWrite", 1);
                        mat.renderQueue = 2000; // Opaque queue
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