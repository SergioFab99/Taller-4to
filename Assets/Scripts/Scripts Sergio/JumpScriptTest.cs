using UnityEngine;

public class JumpScriptTest : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform body;
    public float jumpForce = 10f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(rb.linearVelocity.magnitude > 0.1f)
        {
            body.forward=rb.linearVelocity.normalized;
        }
        else{
            
            body.forward= transform.forward;
        }
        
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce+transform.forward * jumpForce/2, ForceMode.Impulse);
    }
}
