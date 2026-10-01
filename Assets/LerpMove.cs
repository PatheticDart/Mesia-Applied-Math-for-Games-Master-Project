using System;
using UnityEngine;

public class LerpMove : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Transform control;
    [SerializeField] private Transform controlA;
    [SerializeField] private Transform controlB;
    [SerializeField] private float timeToReachTarget = 3f;

    private float totalTime; 

    private Vector3 initialPosition;

    [SerializeField] private float resolution = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        totalTime += Time.deltaTime;
        var lerpedTime = Mathf.Clamp01(totalTime / timeToReachTarget);

        //transform.position = initialPosition + (target.position - initialPosition) * lerpedTime;

        //slow to fast
        //transform.position = initialPosition + (target.position - initialPosition) * (lerpedTime * lerpedTime);

        //fast to slow
        //transform.position = initialPosition + (target.position - initialPosition) * (Mathf.Pow(lerpedTime, 0.5f));

        //bezier curve
        //transform.position = QuadraticFast(initialPosition, control.position, target.position, lerpedTime);
        transform.position = CubicFast(initialPosition, controlA.position, controlB.position, target.position, lerpedTime);
    }

    [ContextMenu("Reset Position")]
    void Reset()
    {
        transform.position = initialPosition;
        totalTime = 0;
    }

    public static Vector3 QuadraticFast(Vector3 p0, Vector3 p1, Vector3 p2, float t) 
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }

    public static Vector3 CubicFast(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return u * u * u * p0 + 3 * u * u * t * p1 + 3 * u * t * t * p2 + t * t * t * p3;
    }

    private void OnDrawGizmos()
    {
        var previousLine = initialPosition;
        for(int i=0; i <= 10; i++)
        {
            // var gap = i / 10f;
            var gap = i / resolution;
            var newPos = CubicFast(initialPosition, controlA.position, controlB.position, target.position, gap);
            Debug.DrawLine(previousLine, newPos, Color.red);
            previousLine = newPos;
        }
    }
}
