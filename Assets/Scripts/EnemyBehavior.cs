using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the enemy
    private int currentHealth; // Current health of the enemy

    public float speed = 2f; // Default movement speed
    private float originalSpeed; // To store the original speed
    private bool isSlowed = false; // Track if the enemy is currently slowed

    void Start()
    {
        currentHealth = maxHealth; // Initialize enemy health
        originalSpeed = speed; // Store the initial speed
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move enemy
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Debug.Log($"Hit by: {other.gameObject.name}");

            // Determine damage
            int damage = other.gameObject.name.Contains("Slow") ? 2 : 5;
            TakeDamage(damage);

            // Apply slow effect
            if (other.gameObject.name.Contains("Slow"))
            {
                ApplyStun(0.5f, 3f);
            }

            Destroy(other.gameObject); // Destroy the projectile
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health
        Debug.Log($"Enemy HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Enemy defeated!");
            Destroy(gameObject);
        }
    }

    public void ApplyStun(float slowFactor, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            speed *= slowFactor; // Reduce speed
            StartCoroutine(RemoveSlowAfterDuration(duration));
        }
    }

    private IEnumerator RemoveSlowAfterDuration(float duration)
    {
     
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
        isSlowed = false;
       
    }
}
