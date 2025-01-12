using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using System.Collections;
using TMPro; // For TextMeshPro

public class EnemyWaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign the enemy prefab
    public Transform enemySpawnPoint; // Where the enemies spawn
    public Transform cannonTarget; // The cannon to target

    public int totalWaves = 3; // Total number of waves
    public float waveInterval = 5f; // Time between waves
    public float enemySpeed = 2f; // Speed at which enemies move
    public float spawnRadius = 5f; // Randomization radius for spawn location

    public TextMeshProUGUI gameOverText; // Assign TextMeshProUGUI for Game Over text
    public GameObject retryButton; // Assign the Retry button in the Inspector

    private int currentWave = 0;
    private int enemiesReachedCannon = 0; // Counter for enemies reaching the cannon
    public int maxEnemiesToLose = 5; // Number of enemies that cause a loss

    void Start()
    {
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text initially
        retryButton.SetActive(false); // Hide the Retry button initially
        StartCoroutine(SpawnEnemyWaves());
    }

    IEnumerator SpawnEnemyWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            SpawnEnemies(currentWave);
            yield return new WaitForSeconds(waveInterval);
        }
    }

    void SpawnEnemies(int wave)
    {
        int enemyCount = wave * 3; // Example: Increase enemy count with each wave

        for (int i = 0; i < enemyCount; i++)
        {
            // Calculate a random spawn position within a radius around the spawn point
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0,
                Random.Range(-spawnRadius, spawnRadius)
            );
            Vector3 spawnPosition = enemySpawnPoint.position + randomOffset;

            // Instantiate the enemy at the randomized position
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(MoveEnemyToTarget(enemy));
        }
    }

    IEnumerator MoveEnemyToTarget(GameObject enemy)
    {
        while (enemy != null && cannonTarget != null)
        {
            // Move the enemy toward the cannon
            enemy.transform.position = Vector3.MoveTowards(
                enemy.transform.position,
                cannonTarget.position,
                enemySpeed * Time.deltaTime
            );

            // Check for collision with the cannon
            if (Vector3.Distance(enemy.transform.position, cannonTarget.position) < 0.1f)
            {
                enemiesReachedCannon++; // Increment the counter
                Debug.Log($"Enemy reached the cannon! Total: {enemiesReachedCannon}");

                if (enemiesReachedCannon >= maxEnemiesToLose)
                {
                    Debug.Log("You lose! Too many enemies reached the cannon.");
                    GameOver(); // Handle game over logic
                }

                Destroy(enemy); // Destroy the enemy
                break;
            }

            yield return null; // Wait for the next frame
        }
    }

    void GameOver()
    {
        // Stop spawning enemies
        StopAllCoroutines();

        // Destroy the cannon
        if (cannonTarget != null)
        {
            Destroy(cannonTarget.gameObject);
            Debug.Log("Cannon destroyed! Game Over.");
        }

        // Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        Debug.Log("All enemies destroyed! Game Over.");

        // Show the Game Over text
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "Game Over";

        // Show the Retry button
        retryButton.SetActive(true);
    }

    public void RetryGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
