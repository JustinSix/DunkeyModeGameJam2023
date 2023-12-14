using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    public Camera playerCamera;

    public float range = 100f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab; // Assign your projectile prefab in the inspector
    public Transform muzzleTransform; // Assign the muzzle transform in the inspector
    public float defaultProjectileDistance = 20f;

    [SerializeField] private float fireRate = 0.1f;
    private float cooldownTimer = 0;
    private bool canShoot = true;
    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera not assigned. Please assign the player camera in the inspector.");
        }
    }

    void Update()
    {
        if (!canShoot)
        {
            cooldownTimer -= Time.deltaTime;

            // Optionally, you can use this value for UI or other feedback
            // float normalizedCooldown = Mathf.Clamp01(cooldownTimer / abilityCooldown);

            // Check if the cooldown has expired
            if (cooldownTimer <= 0f)
            {
                canShoot = true;
            }
        }
        if (Input.GetButton("Fire1") && canShoot)
        {
            canShoot = false;
            Shoot();
        }
    }

    void Shoot()
    {
        cooldownTimer = fireRate;
        Debug.Log("attempted shoot");
        RaycastHit hit;
        Vector3 targetPosition;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // Check if the object hit has a script that can take damage
            Damageable target = hit.transform.GetComponent<Damageable>();

            if (target != null)
            {
                // Apply damage to the target
                target.TakeDamage(10); // You can adjust the damage amount
            }

            // If hit, use the hit point as the target position
            targetPosition = hit.point;
        }
        else
        {
            // If no hit, use a point in the forward direction at a fixed distance
            targetPosition = playerCamera.transform.position + playerCamera.transform.forward * defaultProjectileDistance;
        }

        SpawnProjectile(muzzleTransform.position, targetPosition);
    }

    void SpawnProjectile(Vector3 spawnPosition, Vector3 targetPosition)
    {
        if (projectilePrefab != null)
        {
            // Calculate the direction from the muzzle to the hit point
            Vector3 direction = (targetPosition - spawnPosition).normalized;

            // Instantiate the projectile at the muzzle position
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Get the rigidbody component of the projectile
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply force to the projectile in the calculated direction
            projectileRigidbody.velocity = direction * projectileSpeed;

            // Rotate the projectile to face the shooting direction
            projectile.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            Debug.LogError("Projectile prefab not assigned. Please assign the projectile prefab in the inspector.");
        }
    }
}
