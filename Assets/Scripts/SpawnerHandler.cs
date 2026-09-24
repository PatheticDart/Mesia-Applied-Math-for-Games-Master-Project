using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;

    [Header("Spawn Limits")]
    public int maxEnemies = 5;
    public int maxPowerUps = 3;

    [Header("Spawn Timers (Seconds)")]
    public float enemySpawnRate = 2f;
    public float powerUpSpawnRate = 8f;

    [Header("Spawn Area")]
    public Vector3 spawnAreaSize = new Vector3(20f, 0f, 20f);

    private float nextEnemySpawnTime;
    private float nextPowerUpSpawnTime;

    // Internal tracking lists
    private List<GameObject> activeEnemies = new List<GameObject>();
    private List<GameObject> activePowerUps = new List<GameObject>();

    void Update()
    {
        // Enemy Timer Check
        if (Time.time >= nextEnemySpawnTime)
        {
            SpawnEnemy();
            nextEnemySpawnTime = Time.time + enemySpawnRate;
        }

        // Power-Up Timer Check
        if (Time.time >= nextPowerUpSpawnTime)
        {
            SpawnPowerUp();
            nextPowerUpSpawnTime = Time.time + powerUpSpawnRate;
        }
    }

    private void SpawnEnemy()
    {
        // Clean the list to remove enemies that have been destroyed
        activeEnemies.RemoveAll(item => item == null);

        if (activeEnemies.Count < maxEnemies)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, GetRandomPosition(), Quaternion.identity);
            activeEnemies.Add(newEnemy);
        }
    }

    private void SpawnPowerUp()
    {
        // Clean the list to remove power-ups that have been collected
        activePowerUps.RemoveAll(item => item == null);

        if (activePowerUps.Count < maxPowerUps)
        {
            GameObject newPowerUp = Instantiate(powerUpPrefab, GetRandomPosition(), Quaternion.identity);
            activePowerUps.Add(newPowerUp);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Calculate random coordinates within the defined area
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
        float randomZ = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        // Add the spawner's own position so you can move the center point around
        return transform.position + new Vector3(randomX, randomY, randomZ);
    }

    void OnDrawGizmos()
    {
        // Draws a helpful green box in the Unity Editor to visualize the spawn area
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position, spawnAreaSize);
    }
}