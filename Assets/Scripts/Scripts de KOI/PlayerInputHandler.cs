using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovementController), typeof(PlayerFeedbackManager))]
public class PlayerInputHandler : MonoBehaviour
{
    #region Public Fields
    [Header("Parámetros de Salto")]
    public float minJumpForce = 10f;
    public float maxJumpForce = 130f;
    public float chargeTime = 2f;
    #endregion

    #region Serialized Fields
    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Animator animator;
    #endregion

    #region Private Fields
    private PlayerMovementController movementController;
    private PlayerFeedbackManager feedbackManager;

    private bool isCharging = false;
    private float holdTime = 0f;

    public bool isFuckingDead = false; // ❤️ Humor developer stays
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        movementController = GetComponent<PlayerMovementController>();
        feedbackManager = GetComponent<PlayerFeedbackManager>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!isFuckingDead)
        {
            HandleJumpInput();
            HandleNextSceneInput();
            UpdateAnimatorParameters();
        }
        else
        {
            animator.SetBool("rip", true);
        }
    }
    #endregion

    #region Jump Mechanics
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
            holdTime = Mathf.Clamp(holdTime, 0f, chargeTime);

            float chargePercentage = holdTime / chargeTime;
            feedbackManager.UpdateChargeFeedback(chargePercentage);
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
    #endregion

    #region Scene Management
    private void HandleNextSceneInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        int nextIndex = (currentIndex + 1) % totalScenes;

        SceneManager.LoadScene(nextIndex);
    }
    #endregion

    #region Animator
    private void UpdateAnimatorParameters()
    {
        animator.SetBool("holdMouse", isCharging);
        animator.SetBool("isTouchingGrass", movementController.IsGrounded);
        animator.SetBool("isInTheAir", !movementController.IsGrounded);
    }
    #endregion
}
