using System.Collections.Generic;
using UnityEngine;

public class HomingMissileLogic : MonoBehaviour
{
    public static List<HomingMissileLogic> ActiveMissiles { get; private set; } = new List<HomingMissileLogic>();

    [Header("Missile Settings")]
    [SerializeField] private float speed = 15f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float hitRadius = 1.5f;
    [SerializeField] private GameObject explosionPrefab;

    private Transform playerTransform;
    private float sqrHitRadius;

    private void OnEnable()
    {
        if (!ActiveMissiles.Contains(this))
        {
            ActiveMissiles.Add(this);
        }
    }

    private void OnDisable()
    {
        ActiveMissiles.Remove(this);
    }

    private void Start()
    {
        sqrHitRadius = hitRadius * hitRadius;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckHitPlayer();
    }

    private void CheckHitPlayer()
    {
        if (playerTransform == null) return;

        PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();

        float sqrDistance = (playerTransform.position - transform.position).sqrMagnitude;

        if (sqrDistance <= sqrHitRadius)
        {
            if (playerHealth != null)
            {
                playerHealth.Damage(1);
            }

            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}