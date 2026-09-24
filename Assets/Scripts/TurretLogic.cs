using Mono.Cecil.Cil;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum TurretType
{
    Flame,
    Sniper,
    Shotgun
}

public class TurretLogic : MonoBehaviour
{
    public TurretType turretType;
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float range = 10f;
    [SerializeField] private float coneAngle = 90f;
    [SerializeField] private float burstInterval = 3.0f;
    [SerializeField] private float fireRate = 0.05f;
    [SerializeField] private int shotCount = 4;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileLifeTime = 3f;
    private float nextBurstTime = 0f;

    private LineRenderer lineRenderer;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned in TurretLogic.");
        }

        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {

        if (target == null) return;

        //rotate to face target
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
        var dot = Vector3.Dot(transform.forward, dir.normalized);

        switch(turretType)
        {
            case TurretType.Flame:
                FlameTurretLineRenderer();
                if (IsInCone(target, range, coneAngle))
                {
                    Debug.Log("Target is in cone");

                    if (Time.time >= nextBurstTime)
                    {
                        StartCoroutine(FlameShoot());
                        nextBurstTime = Time.time + burstInterval;
                    }
                }
                break;
            case TurretType.Sniper:
                SniperTurretLineRenderer();
                Vector3 forward = this.transform.forward;

                Vector3 toTarget = (target.position - this.transform.position).normalized;

                bool facingTarget = dot > 0f;

                if (facingTarget && dir.magnitude < range)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.deltaTime);
                    if (Time.time >= nextBurstTime)
                    {
                        StartCoroutine(SniperShoot());
                        nextBurstTime = Time.time + burstInterval;
                    }
                }
                break;
            case TurretType.Shotgun:
                ShotgunTurretLineRenderer();
                if (dir.magnitude < range)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.deltaTime);

                    if (Time.time >= nextBurstTime)
                    {
                        StartCoroutine(ShotgunShoot());
                        nextBurstTime = Time.time + burstInterval;
                    }
                }
                break;
        }      
    }

    IEnumerator FlameShoot()
    {
        float forwardRad = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float halfCone = coneAngle / 2f;

        float angleSpacing = coneAngle / shotCount;

        float offset = angleSpacing / 2f;

        float startAngle = (forwardRad * Mathf.Rad2Deg) - halfCone + offset;

        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = startAngle + (angleSpacing * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newProjectile.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newProjectile, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    IEnumerator SniperShoot()
    {
        Vector3 barrel = this.transform.forward;
        Vector3 toPlayer = ((Vector3)target.position
            - (Vector3)this.transform.position).normalized;
        float dot = Vector3.Dot(barrel, toPlayer);
        bool inSights = dot >= 0.98f;  // ~11° tolerance

        float radians = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float x = Mathf.Cos(radians);
        float z = Mathf.Sin(radians);

        Vector3 direction = new Vector3(x, 0, z);

        if (inSights)
        {
            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newProjectile.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newProjectile, projLifeTime);
        }   

        yield return new WaitForSeconds(fireRate);
    }

    IEnumerator ShotgunShoot()
    {
        float forwardRad = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float halfCone = coneAngle / 2f;

        float angleSpacing = coneAngle / shotCount;

        float offset = angleSpacing / 2f;

        float startAngle = (forwardRad * Mathf.Rad2Deg) - halfCone + offset;

        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = startAngle + (angleSpacing * i);
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(rad);
            float z = Mathf.Sin(rad);
            Vector3 direction = new Vector3(x, 0, z);

            GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(direction));

            ProjectileLogic rocketLogic = newProjectile.GetComponent<ProjectileLogic>();
            if (rocketLogic != null)
            {
                rocketLogic.direction = direction;
            }
            Destroy(newProjectile, 3f);

            yield return new WaitForSeconds(fireRate);
        }
    }

    bool IsInCone(Transform target, float range, float coneAngle)
    {
        Vector3 dir = target.position - this.transform.position;
        if (dir.magnitude > range) return false;
                
        float playerAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        float turretAngle = -this.transform.eulerAngles.y + 90f;

        float delta = Mathf.Abs(Mathf.DeltaAngle(turretAngle, playerAngle));

        return delta <= coneAngle / 2f; 
    }

    void FlameTurretLineRenderer()
    {
        // Draw detection cone matching IsInCone logic
        int segments = 20;
        lineRenderer.positionCount = segments + 2;

        Vector3 origin = transform.position;
        lineRenderer.SetPosition(0, origin);

        // Convert Unity rotation (eulerAngles.y) to polar angle matching IsInCone (-y + 90)
        float baseAngle = -transform.eulerAngles.y + 90f;
        float halfCone = coneAngle / 2f;
        float startAngle = baseAngle - halfCone;
        float step = coneAngle / segments;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = startAngle + (i * step);
            float rad = currentAngle * Mathf.Deg2Rad;

            // Polar math: Cos -> X, Sin -> Z
            Vector3 pointOffset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * range;
            lineRenderer.SetPosition(i + 1, origin + pointOffset);
        }

        lineRenderer.SetPosition(segments + 1, origin);

        // Color gradient to indicate flame range
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
    }

    void SniperTurretLineRenderer()
    {
        // Draw a single straight targeting ray along transform.forward up to range distance
        lineRenderer.positionCount = 2;

        Vector3 origin = transform.position;
        Vector3 targetPoint = origin + (transform.forward * range);

        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, targetPoint);

        // Color laser style
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void ShotgunTurretLineRenderer()
    {
        Vector3 origin = transform.position;

        float forwardRad = Mathf.Atan2(transform.forward.z, transform.forward.x);
        float halfCone = coneAngle / 2f;
        float angleSpacing = coneAngle / shotCount;
        float offset = angleSpacing / 2f;
        float startAngle = (forwardRad * Mathf.Rad2Deg) - halfCone + offset;

        int circleSegments = 40;

        // Total points: (shotCount * 3 for fan) + (circleSegments + 1 for full 360 loop)
        lineRenderer.positionCount = (shotCount * 3) + (circleSegments + 1);

        int pointIndex = 0;
        float lastPelletAngleDeg = 0f;

        // 1. Trace Pellet Lines
        for (int i = 0; i < shotCount; i++)
        {
            float currentAngle = startAngle + (i * angleSpacing);
            float rad = currentAngle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));
            Vector3 endPoint = origin + (direction * range);

            lineRenderer.SetPosition(pointIndex++, origin);
            lineRenderer.SetPosition(pointIndex++, endPoint);

            // On the final pellet line, stay at the endPoint instead of returning to origin
            if (i == shotCount - 1)
            {
                lineRenderer.SetPosition(pointIndex++, endPoint);
                lastPelletAngleDeg = currentAngle; // Save angle to seamlessly start circle here
            }
            else
            {
                lineRenderer.SetPosition(pointIndex++, origin);
            }
        }

        // 2. Trace Outer Circle seamless starting directly from the tip of the last pellet
        float step = 360f / circleSegments;
        for (int i = 0; i <= circleSegments; i++)
        {
            float currentCircleAngle = lastPelletAngleDeg + (i * step);
            float rad = currentCircleAngle * Mathf.Deg2Rad;

            Vector3 circlePoint = origin + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * range;
            lineRenderer.SetPosition(pointIndex++, circlePoint);
        }

        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.blue;
    }
}
