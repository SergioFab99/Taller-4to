using UnityEngine;

public class FlappyBird3D : MonoBehaviour
{
    public float jumpForce = 5f; // Fuerza del salto
    public float rotationSpeed = 100f; // Velocidad de rotación
    public float moveSpeed = 5f; // Velocidad de movimiento hacia adelante y atrás
    public LayerMask groundLayer; // Capa del suelo
    public float groundCheckDistance = 0.1f; // Distancia para verificar el suelo

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Saltar al hacer clic
        if (Input.GetMouseButtonDown(0) && IsGrounded()) // Verificar si está en el suelo
        {
            Jump();
        }

        // Rotar a la izquierda y derecha
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(-rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(rotationSpeed * Time.deltaTime);
        }

        // Mover hacia adelante y atrás
        Move();
    }

    void Jump()
    {
        // Aplicar fuerza hacia arriba
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    void Rotate(float rotationAmount)
    {
        // Rotar el objeto en su propio eje
        transform.Rotate(0, rotationAmount, 0);
    }

    void Move()
    {
        float moveDirection = 0f;

        // Detectar entrada de W y S
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection = 1f; // Mover hacia adelante
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDirection = -1f; // Mover hacia atrás
        }

        // Mover el objeto en la dirección deseada
        Vector3 movement = transform.forward * moveDirection * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    bool IsGrounded()
    {
        // Verificar si el objeto está en contacto con el suelo
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}