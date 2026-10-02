using UnityEngine;

public class SpawnerPathLogic : MonoBehaviour
{
    public enum CurveMode { Quadratic, Cubic }

    [Header("Curve Mode")]
    [SerializeField] private CurveMode curveMode = CurveMode.Cubic;

    [Header("Curve Points")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform controlA;
    [Tooltip("Only required when Curve Mode is set to Cubic.")]
    [SerializeField] private Transform controlB;
    [SerializeField] private Transform target;

    [Header("Settings")]
    [SerializeField] private GameObject objectToInstantiate;
    [SerializeField] private float timeToReachTarget = 3f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    [ContextMenu("Spawn Object")]
    public void Spawn()
    {
        if (objectToInstantiate == null || target == null || controlA == null)
        {
            Debug.LogWarning("Missing required references on BezierSpawner!");
            return;
        }

        if (curveMode == CurveMode.Cubic && controlB == null)
        {
            Debug.LogWarning("Control B is required when using Cubic mode!");
            return;
        }

        Vector3 startPos = spawnPoint != null ? spawnPoint.position : transform.position;

        GameObject spawnedObj = Instantiate(objectToInstantiate, startPos, Quaternion.identity);

        if (!spawnedObj.TryGetComponent<MoverLogic>(out var mover))
        {
            mover = spawnedObj.AddComponent<MoverLogic>();
        }

        if (curveMode == CurveMode.Quadratic)
        {
            mover.InitializeQuadratic(startPos, controlA.position, target.position, timeToReachTarget);
        }
        else
        {
            mover.InitializeCubic(startPos, controlA.position, controlB.position, target.position, timeToReachTarget);
        }
    }
}