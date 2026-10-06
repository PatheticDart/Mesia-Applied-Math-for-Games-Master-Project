using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera mainCamera;

    [Header("Spawning Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDistanceFromCamera = 25f;

    [Header("Difficulty Scaling Settings")]
    [SerializeField] private float difficultyInterval = 10f;
    [SerializeField] private int baseMissilesPerSpawn = 1;

    private float spawnTimer;
    private float survivalTimer;
    private int extraMissilesToSpawn = 0;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        survivalTimer += Time.deltaTime;
        extraMissilesToSpawn = Mathf.FloorToInt(survivalTimer / difficultyInterval);

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnMissileWave();
        }
    }

    private void SpawnMissileWave()
    {
        int totalToSpawn = baseMissilesPerSpawn + extraMissilesToSpawn;

        for (int i = 0; i < totalToSpawn; i++)
        {
            Vector3 spawnPosition = GetPositionOutsideCamera();
            Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GetPositionOutsideCamera()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector3 direction = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;

        Vector3 spawnPos = playerTransform.position + direction * spawnDistanceFromCamera;
        spawnPos.y = playerTransform.position.y;

        return spawnPos;
    }
}