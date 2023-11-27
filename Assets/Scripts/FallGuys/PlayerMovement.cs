using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    //events 
    public event EventHandler OnPlayerJumped;
    [SerializeField] PlayerInput playerInput;
    //customizeable
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 9;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float groundDrag;

    [Header("Running")]
    [SerializeField] private bool canRun = true;
    public bool IsRunning { get; private set; }

    private bool isMoving = false;

    //Slopes 
    public float maxSlopeAngle = 45f; // maximum slope angle in degrees
    public float slopeDistance = 2f;
    //[SerializeField] private float slopeCheckRaycastOriginOffset = -0.1f;

    Rigidbody rb;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    #region Jumping Variables
    [SerializeField] private float jumpStrength = 2;


    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;
    #endregion
    [SerializeField] Transform playerCamera;
    #region player hit variables
    private bool canMove = true; //If player is not hitted
    private bool isStuned = false;
    private bool wasStuned = false; //If player was stunned before get stunned another time
    private float pushForce;
    private Vector3 pushDir;

    #endregion

    void Awake()
    {
        // Get the rigidbody on this.
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        //register jump event 
        playerInput.OnJumpAction += PlayerInput_OnJumpAction;
    }
    private void Update()
    {
        SpeedControl();
        if (groundCheck.isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && playerInput.IsRunning();
        //check slope
        if (IsSlopeTooSteep(maxSlopeAngle, slopeDistance))
        {
            //Debug.LogError("too steep of slope");

            isMoving = false;
        }
        else
        {
            PlayerMove();
        }
    }
    private void PlayerMove()
    {
        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : walkSpeed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        //player input determine movement vector
        Vector2 movementVector = playerInput.GetMovementVectorNormalized();

        //calculate exact movement direction vector
        Vector3 moveDirection = transform.rotation * new Vector3(movementVector.x, 0, movementVector.y);


        // Apply movement depending on if grounded or in air
        if (groundCheck.isGrounded)
        {
            rb.AddForce(moveDirection * targetMovingSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * targetMovingSpeed * 10 * airMultiplier, ForceMode.Force);
        }


        if (rb.velocity.sqrMagnitude > 1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        Debug.Log("Move function called with, movement vector Y:" + movementVector.x + "movement vector Y:" + movementVector.y);
    }
    //JUMP 
    private void PlayerInput_OnJumpAction(object sender, System.EventArgs e)
    {
        //Jump when the Jump button is pressed and we are on the ground.
        //Input.GetButtonDown("Jump") && 
        if ((!groundCheck || groundCheck.isGrounded))
        {
            //playerAnim.SetTrigger("jump");
            OnPlayerJumped?.Invoke(this, EventArgs.Empty);
            rb.AddForce(Vector3.up * 100 * jumpStrength, ForceMode.Impulse);
        }
    }

    public bool IsSlopeTooSteep(float maxAngle, float distance)
    {
        //Vector3 origin = transform.position + Vector3.up * slopeCheckRaycastOriginOffset; // offset to avoid hitting the ground

        //if (Physics.Raycast(origin, transform.forward, out RaycastHit hitInfo, distance))
        //{
        //    Debug.DrawRay(origin, transform.forward * distance, Color.red);
        //    float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
        //    return angle > maxAngle;
        //}
        return false;
    }

    public void HitPlayer(Vector3 velocityF, float time, float force)
    {
        float multiplyForceByHealth = 100f;
        force *= multiplyForceByHealth;
        velocityF *= force;

        rb.velocity = velocityF;

        pushForce = velocityF.magnitude;
        pushDir = Vector3.Normalize(velocityF);
        StartCoroutine(Decrease(velocityF.magnitude, time));
    }


    private IEnumerator Decrease(float value, float duration)
    {
        if (isStuned)
            wasStuned = true;
        isStuned = true;
        canMove = false;

        float delta = 0;
        delta = value / duration;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            yield return null;

            rb.AddForce(new Vector3(0, GetComponent<Rigidbody>().mass, 0)); //Add gravity
        }

        if (wasStuned)
        {
            wasStuned = false;
        }
        else
        {
            isStuned = false;
            canMove = true;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > runSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * runSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    public bool IsMoving()
    {
        return isMoving;
    }
}
