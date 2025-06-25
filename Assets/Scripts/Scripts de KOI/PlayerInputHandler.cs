using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovementController), typeof(PlayerFeedbackManager))]
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Parámetros de Salto")]
    public float minJumpForce = 10f;
    public float maxJumpForce = 130f;
    public float chargeTime = 2f;

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;

    // Referencias a otros componentes
    private PlayerMovementController movementController;
    private PlayerFeedbackManager feedbackManager;

    private bool isCharging = false;
    private float holdTime = 0f;

    void Awake()
    {
        // Obtenemos las referencias una sola vez
        movementController = GetComponent<PlayerMovementController>();
        feedbackManager = GetComponent<PlayerFeedbackManager>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleJumpInput();
        HandleRestartInput();
    }

    private void HandleJumpInput()
    {
        // Iniciar carga si estamos en el suelo o en el agua
        if ((movementController.IsGrounded || movementController.IsWater) && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
            feedbackManager.StartChargeFeedback();
        }

        // Mantener la carga
        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;
            
            // Calcular el porcentaje ANTES de hacer el clamp
            float chargePercentage = Mathf.Clamp01(holdTime / chargeTime);
            
            // Ahora sí aplicamos el clamp al holdTime
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);
            
            // Enviar el feedback con el porcentaje correcto
            feedbackManager.UpdateChargeFeedback(chargePercentage);
            
            // Debug para verificar los valores
            Debug.Log($"HoldTime: {holdTime:F2}, ChargeTime: {chargeTime:F2}, Percentage: {chargePercentage:F2}");
        }

        // Soltar y saltar
        if (isCharging && Input.GetMouseButtonUp(0))
        {
            isCharging = false;
            float jumpStrength = Mathf.Lerp(minJumpForce, maxJumpForce, holdTime / chargeTime);
            Vector3 jumpDirection = CalculateJumpDirection();

            movementController.PerformJump(jumpStrength, jumpDirection);
            feedbackManager.ReleaseChargeFeedback();
        }
    }

    private Vector3 CalculateJumpDirection()
    {
        Vector3 camForward = cameraTransform.forward;
        float minVerticalComponent = 0.5f;

        // Si la componente vertical es muy baja, la ajustamos para que no haya saltos planos
        if (camForward.y < minVerticalComponent)
        {
            camForward.y = minVerticalComponent;
        }

        return camForward.normalized;
    }

    private void HandleRestartInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}