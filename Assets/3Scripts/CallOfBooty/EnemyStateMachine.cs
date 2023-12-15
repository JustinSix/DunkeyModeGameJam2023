using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public GravityManager gravityManager;
    public Transform player;
    public float moveSpeed = 2.5f;
    public float detectionRange = 10f;
    public float verticalOffset = 1f; // Adjustable offset in the Inspector
    private enum EnemyState
    {
        Idle,
        Chase,
    }

    private EnemyState currentState;

    private void Start()
    {
        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;

            case EnemyState.Chase:
                UpdateChaseState();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a wire sphere in the Unity Editor to visualize the detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void UpdateIdleState()
    {
        // Check if the player is within the detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            // Transition to the Chase state
            currentState = EnemyState.Chase;
        }
    }

    private void UpdateChaseState()
    {
        Vector3 targetPosition;
        if (gravityManager.GetIfOnCeiling())
        {
            // Calculate the target position with the vertical offset
            targetPosition = new Vector3(player.position.x, player.position.y + -verticalOffset, player.position.z);
        }
        else
        {
            // Calculate the target position with the vertical offset
            targetPosition = new Vector3(player.position.x, player.position.y + verticalOffset, player.position.z);
        }


        // Look at the adjusted target position
        transform.LookAt(targetPosition);

        // Move forward in the direction of the adjusted target position
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Check if the player is no longer within the detection range
        if (Vector3.Distance(transform.position, player.position) > detectionRange * 2.5f)
        {
            // Transition back to the Idle state
            currentState = EnemyState.Idle;
        }
    }
}
