using JetBrains.Annotations;
using UnityEngine;

public class MoverLogic : MonoBehaviour
{
    public int health = 1;
    public int damageToPlayer = 1;
    public enum CurveType { Quadratic, Cubic }

    private CurveType curveType;
    private Vector3 startPoint;
    private Vector3 controlA;
    private Vector3 controlB;
    private Vector3 targetPoint;

    private float duration;
    private float elapsedTime;
    private bool isMoving;

    public void Start()
    {
    }

    public void InitializeQuadratic(Vector3 p0, Vector3 p1, Vector3 p2, float timeToReachTarget)
    {
        curveType = CurveType.Quadratic;
        startPoint = p0;
        controlA = p1;
        targetPoint = p2;
        duration = timeToReachTarget;

        transform.position = startPoint;
        elapsedTime = 0f;
        isMoving = true;
    }
    public void InitializeCubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float timeToReachTarget)
    {
        curveType = CurveType.Cubic;
        startPoint = p0;
        controlA = p1;
        controlB = p2;
        targetPoint = p3;
        duration = timeToReachTarget;

        transform.position = startPoint;
        elapsedTime = 0f;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        if (curveType == CurveType.Quadratic)
        {
            transform.position = QuadraticFast(startPoint, controlA, targetPoint, t);
        }
        else
        {
            transform.position = CubicFast(startPoint, controlA, controlB, targetPoint, t);
        }

        if (t >= 1f)
        {
            isMoving = false;
            OnReachDestination();
        }
    }

    public static Vector3 QuadraticFast(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0
             + 2f * u * t * p1
             + t * t * p2;
    }

    public static Vector3 CubicFast(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return u * u * u * p0
             + 3f * u * u * t * p1
             + 3f * u * t * t * p2
             + t * t * t * p3;
    }

    private void OnReachDestination()
    {
        ResourceManager.Instance.SubtractHealth(damageToPlayer);
        Destroy(gameObject);
    }
}