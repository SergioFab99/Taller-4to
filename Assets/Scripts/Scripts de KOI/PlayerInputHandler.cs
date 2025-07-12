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
    [SerializeField] private Animator animator; // Referencia al Animator

    private PlayerMovementController movementController;
    private PlayerFeedbackManager feedbackManager;

    private bool isCharging = false;
    private float holdTime = 0f;
    public bool isFuckingDead = false;

    void Awake()
    {
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
        if(!isFuckingDead)
        {
            HandleJumpInput();
            HandleRestartInput();
            AnimationShit();
        }

        else if(isFuckingDead)
        {
            animator.SetBool("rip", true);
        }
    }

    private void HandleJumpInput()
    {
        if ((movementController.IsGrounded || movementController.IsWater) && Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            holdTime = 0f;
            feedbackManager.StartChargeFeedback();
        }

        if (isCharging && Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;

            float chargePercentage = Mathf.Clamp01(holdTime / chargeTime);
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);

            feedbackManager.UpdateChargeFeedback(chargePercentage);

            Debug.Log($"HoldTime: {holdTime:F2}, ChargeTime: {chargeTime:F2}, Percentage: {chargePercentage:F2}");
        }

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

    public void AnimationShit()
    {
        animator.SetBool("holdMouse", isCharging);
        animator.SetBool("isTouchingGrass", movementController.IsGrounded);
        animator.SetBool("isInTheAir", !movementController.IsGrounded);
    }
}
