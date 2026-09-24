/*using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum TurretType
{
    Flame,
    Sniper,
    Shotgun
}

public class TurretShootLogic : MonoBehaviour
{
    public GameObject projectile;

    [Header("Turret Settings")]
    public TurretType turretType;
    public int shotCount = 4;
    public float burstInterval = 3.0f;
    public float nextBurstTime = 0f;
    public float fireRate = 0.05f;
    public float nextFireTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        switch (turretType)
        {
            case TurretType.Flame:
                StartCoroutine(FlameShoot());
                break;
            case TurretType.Sniper:
                StartCoroutine(SniperShoot());
                break;
            case TurretType.Shotgun:
                StartCoroutine(ShotgunShoot());
                break;
        }
    }


    private IEnumerator FlameShoot()
    {
        float angleSpacing = 90f / shotCount;

        float offset = angleSpacing / 2f;

        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = offset + (angleSpacing * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newRocket = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newRocket.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newRocket, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator SniperShoot()
    {
        float angleSpacing = 360f / shotCount;

        float offset = angleSpacing / 2f;

        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = (offset + (angleSpacing * i));
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newRocket = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newRocket.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newRocket, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    private IEnumerator ShotgunShoot()
    {
        float angleSpacing = 180f / shotCount;

        float offset = angleSpacing / 2f;

        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = offset + (angleSpacing * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newRocket = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newRocket.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newRocket, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }
}
*/