using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    private bool canFlip = true;
    private float flipGravityCD = 3f;
    private float cooldownTimer = 0;
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform playerTransform;
    [SerializeField] float forceMagnitude = 10f;
    private bool isOnCeiling = false;
    private void Start()
    {
        Physics.gravity = new Vector3(0, -100.81f, 0);
    }
    // Update is called once per frame
    void Update()
    {
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
    
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerRB.AddForce(Vector3.forward * forceMagnitude, ForceMode.Impulse);
        }
    }
    private void FlipGravity()
    {
        Physics.gravity *= -1;
        playerRB.velocity = Vector3.zero;
        StartCoroutine(RotatePlayerUpsideDownSmooth());
    }
    private void PushPlayerStart()
    {
        //Apply force based on the new gravity direction
        if (Physics.gravity.y < 0)
        {
            Debug.Log("added force down");
            playerRB.AddForce(Vector3.down * forceMagnitude, ForceMode.Impulse);
        }
        else
        {
            Debug.Log("added force up");
            playerRB.AddForce(Vector3.up * forceMagnitude, ForceMode.Impulse);
        }
    }
    private void AdjustPlayerPositionStart()
    {
        Vector3 newPosition = playerTransform.position;
        if (Physics.gravity.y > 0)
        {
            Debug.Log("added movement down");
            newPosition.y -= .5f;
        }
        else
        {
            Debug.Log("added movement up");
            newPosition.y += .5f;
        }
    }
    private void ResetPlayerRotation()
    {
        if (isOnCeiling)
        {
            playerTransform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            playerTransform.rotation = Quaternion.Euler(0, 0f, 0f);
        }
    }
    private IEnumerator RotatePlayerUpsideDownSmooth()
    {
        AdjustPlayerPositionStart();
        ResetPlayerRotation();
        float elapsedTime = 0f;
        float rotationTime = 1.0f; // Adjust the time it takes to rotate

        Quaternion startRotation = playerTransform.localRotation;

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
            playerTransform.localRotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        playerTransform.localRotation = targetRotation;

        PushPlayerStart();
        if(Physics.gravity.y > 0f)
        {
            isOnCeiling = true;
        }
        else
        {
            isOnCeiling = false;
        }
    }
    void StartCooldown()
    {
        canFlip = false;
        // Reset the cooldown timer to the specified duration
        cooldownTimer = flipGravityCD;
    }

    private void OnDestroy()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0); 
    }
    private void OnApplicationQuit()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }
    public bool GetIfOnCeiling()
    {
        return isOnCeiling;
    }
}
