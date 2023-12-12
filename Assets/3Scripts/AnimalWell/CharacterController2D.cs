using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Handle movement
        Move();

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("attempted jump");
            Jump();
        }
        else if (!isGrounded)
        {
            Debug.Log("not grounded");
        }
    }

    void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(horizontalInput, 0f, 0f); // Only allow movement along the X-axis
        Vector3 moveDirection = Camera.main.transform.TransformDirection(movement);
        moveDirection.y = 0f;

        rb.AddForce(new Vector3(moveDirection.normalized.x * moveSpeed, 0f, 0f));

        // Face either right or left based on movement direction
        if (movement.x > 0)
        {
            transform.forward = Vector3.right; // Face right
        }
        else if (movement.x < 0)
        {
            transform.forward = Vector3.left; // Face left
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
