using UnityEngine;

public class Slowball : MonoBehaviour
{
    public float stunFactor = 0.9f; // Reduce speed by 50%
    public float stunDuration = 3f; // Duration of the slow effect

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.ApplyStun(stunFactor, stunDuration);
            }
        }

        // Destroy the slowball after collision
        Destroy(gameObject);
    }
}
