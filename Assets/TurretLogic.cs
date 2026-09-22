using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        if (target == null) return;

        //rotate to face target
        var dir = target.transform.position - transform.position;
        var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, angle, 0), rotationSpeed * Time.deltaTime);
        
        var dot = Vector3.Dot(transform.forward, dir.normalized);
    }

    bool IsInCone(Transform turret, Transform target, float range, float coneAngle)
    {
        Vector3 dir = target.position - turret.position;
        if (dir.magnitude > range) return false;

        float playerAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        float turretAngle = turret.eulerAngles.y;

        float delta = Mathf.Abs(Mathf.DeltaAngle(turretAngle, playerAngle));
        return delta <= coneAngle / 2f;
    }
}
