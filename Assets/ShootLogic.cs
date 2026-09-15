using System.Collections;
using UnityEngine;
using TMPro;

public class ShootLogic : MonoBehaviour
{
    public GameObject rocket;
    public TMP_Text rocketCounter;

    public int rocketCount = 4;

    [SerializeField]
    private float burstInterval = 3.0f;
    private float nextBurstTime = 0f;
    private float fireRate = 0.05f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextBurstTime)
        {
            StartCoroutine(SpawnRockets());
            nextBurstTime = Time.time + burstInterval;
        }

        if (rocketCount != 8)
        {
            rocketCounter.text = rocketCount.ToString();
        }
        else
        {
            rocketCounter.text = rocketCount.ToString() + " (MAX)";
        }
    }

    private IEnumerator SpawnRockets()
    {
        float angleSpacing = 360f / rocketCount;
        
        float offset = angleSpacing / 2f;
        
        for (int i = 0; i < rocketCount; i++)
        {
            float currentAngle = offset + (angleSpacing * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newRocket = Instantiate(rocket, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newRocket.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newRocket, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    public void Upgrade()
    {
        if (rocketCount < 8)
        {
            rocketCount++;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}