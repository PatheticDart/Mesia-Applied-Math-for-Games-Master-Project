using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public Item lowObjectPrefab;
    public Item highObjectPrefab;
    public Item healthPickupPrefab;
    public PlayerLogic playerRef;

    [Header("Spawn Chances (Weights)")]
    public float lowObjectChance = 60f;
    public float highObjectChance = 30f;
    public float healthPickupChance = 10f;

    [Header("Spawn Settings")]
    public float speed = 100f;
    public float laneWidth = 2f;
    public float spawnZ = 500f;
    public float spawnRate = 1.2f;

    [Header("Difficulty Scaling")]
    public float speedIncreasePerSec = 5f;
    public float maxSpeed = 300f;
    public float spawnRateDecreasePerSec = 0.03f;
    public float minSpawnRate = 0.3f;

    private float spawnTimer = 1f;
    private List<Item> activeItems = new List<Item>();

    private void BeginSpawn()
    {
        Item chosenPrefab = GetRandomPrefab();
        if (chosenPrefab == null) return;

        var spawnedItem = Instantiate(chosenPrefab, transform);

        int randomLane = Random.Range(-1, 2);
        spawnedItem.itemPosition = new Vector3(randomLane * laneWidth, 0, spawnZ);
        spawnedItem.player = playerRef;

        activeItems.Add(spawnedItem);
    }

    private Item GetRandomPrefab()
    {
        float totalChance = lowObjectChance + highObjectChance + healthPickupChance;
        if (totalChance <= 0f) return lowObjectPrefab;

        float roll = Random.Range(0f, totalChance);

        if (roll < lowObjectChance)
        {
            return lowObjectPrefab;
        }
        else if (roll < lowObjectChance + highObjectChance)
        {
            return highObjectPrefab;
        }
        else
        {
            return healthPickupPrefab;
        }
    }

    private void Update()
    {
        speed = Mathf.Min(maxSpeed, speed + speedIncreasePerSec * Time.deltaTime);
        spawnRate = Mathf.Max(minSpawnRate, spawnRate - spawnRateDecreasePerSec * Time.deltaTime);

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            BeginSpawn();
            spawnTimer = spawnRate;
        }

        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            var item = activeItems[i];
            item.itemPosition.z -= speed * Time.deltaTime;

            if (item.itemPosition.z < -CameraComponent.focalLength * 1f)
            {
                activeItems.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        var sortedChildren = children.OrderByDescending(x => x.GetComponent<Item>().itemPosition.z).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            sortedChildren[i].SetSiblingIndex(i);
        }
    }
}