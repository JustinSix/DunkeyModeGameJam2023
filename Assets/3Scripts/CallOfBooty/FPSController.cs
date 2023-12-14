using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public float speed = 5.0f;
    public float lookSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.8f;

    private CharacterController characterController;
    private Vector3 velocity;

    private bool canFlip = true;
    private float flipGravityCD = 3f;
    private float cooldownTimer = 0;

    [SerializeField] private Transform playerTransform;
    void Start()
    {
        playerTransform.position += new Vector3(10, 10, 10);
        characterController = GetComponent<CharacterController>();

        // Lock cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMove();

        if (!canFlip)
        {
            cooldownTimer -= Time.deltaTime;

            // Optionally, you can use this value for UI or other feedback
            // float normalizedCooldown = Mathf.Clamp01(cooldownTimer / abilityCooldown);

            // Check if the cooldown has expired
            if (cooldownTimer <= 0f)
            {
                canFlip = true;
            }
        }
        // Update the cooldown timer
        if (canFlip)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("pressed f key to rotate");
                FlipGravity();
                StartCooldown();
            }

        }
        velocity.y += gravity * Time.deltaTime;
        Debug.Log("player velocity: " + velocity + "\nand grounded is: " + characterController.isGrounded);
    }

    private void HandleMove()
    {
        // Player Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical);
        movement = Quaternion.Euler(0, transform.eulerAngles.y, 0) * movement;

        if (characterController.isGrounded)
        {

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

        characterController.Move((movement * speed + velocity) * Time.deltaTime);
    }
    private void Jump()
    {
        if (characterController.isGrounded)
        {
            // Jump with the correct force based on gravity direction
            if (gravity > 0)
            {
                velocity.y = -Mathf.Sqrt(jumpForce * 2f * Mathf.Abs(gravity));
            }
            else
            {
                velocity.y = Mathf.Sqrt(jumpForce * 2f * Mathf.Abs(gravity));
            }
        }
        else
        {
            // If not grounded, just stop applying gravity for a smooth fall
            velocity.y = 0f;
        }
    }
    private void FlipGravity()
    {
        velocity.y = 0f;
        gravity = gravity * -1;
        jumpForce = jumpForce * -1;


        Vector3 newPosition = playerTransform.position;
        if (gravity > 0)
        {
            newPosition.y -= 1f;
        }
        else
        {
            newPosition.y += 1;
        }

        //check if inversed and positive value

        StartCoroutine(RotatePlayerUpsideDownSmooth());

    }
    private IEnumerator RotatePlayerUpsideDownSmooth()
    {
        float elapsedTime = 0f;
        float rotationTime = 1.0f; // Adjust the time it takes to rotate

        Quaternion startRotation = transform.localRotation;

        // Determine the target rotation based on the current rotation
        Quaternion targetRotation;
        if (Mathf.Approximately(startRotation.eulerAngles.x, 0f))
        {
            targetRotation = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        while (elapsedTime < rotationTime)
        {
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.localRotation = targetRotation;
    }

    void StartCooldown()
    {
        canFlip = false;
        // Reset the cooldown timer to the specified duration
        cooldownTimer = flipGravityCD;
    }
}