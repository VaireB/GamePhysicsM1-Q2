using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab;      // Assign the projectile prefab
    public GameObject stunProjectilePrefab; // Assign the slow projectile prefab
    public Transform spawnPoint;            // Where the projectile spawns

    public float initialVelocity = 10f; // Initial velocity of the projectile (m/s)
    public float moveSpeed = 5f;        // Speed at which the cannon moves left and right
    public float fireRate = 0.5f;       // Time interval between consecutive shots when holding the fire button

    public float stunFireCooldown = 2f; // Cooldown time for the slowball
    private float nextSlowFireTime = 0f; // Tracks when the slowball can be fired again

    private float nextFireTime = 0f;

    void Update()
    {
        HandleMovement();

        // Fire regular cannonball
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }

        // Fire slowball
        if (Input.GetMouseButtonDown(1) && Time.time >= nextSlowFireTime)
        {
            FireSlowball();
        }
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);
    }

    void FireProjectile()
    {
        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>(); // Get the Rigidbody from the prefab

        if (rb == null)
        {
            Debug.LogError("Projectile prefab is missing a Rigidbody component!");
            return;
        }

        // Calculate the launch velocity
        Vector3 launchVelocity = spawnPoint.forward * initialVelocity;
        rb.velocity = launchVelocity;
    }

    void FireSlowball()
{
    Debug.Log("Attempting to fire slowball...");

    if (stunProjectilePrefab == null)
    {
        Debug.LogError("Slow projectile prefab is not assigned!");
        return;
    }

    // Instantiate the slowball
    GameObject slowball = Instantiate(stunProjectilePrefab, spawnPoint.position, spawnPoint.rotation);
    Debug.Log($"Slowball instantiated at position: {spawnPoint.position}");

    Rigidbody rb = slowball.GetComponent<Rigidbody>();
    if (rb == null)
    {
        Debug.LogError("Slow projectile prefab is missing a Rigidbody component!");
        return;
    }

    // Calculate the launch velocity
    Vector3 launchVelocity = spawnPoint.forward * initialVelocity;
    rb.velocity = launchVelocity;

    Debug.Log($"Slowball launched with velocity: {launchVelocity}");

    // Set the next available time to fire a slowball
    nextSlowFireTime = Time.time + stunFireCooldown;
}


}
