using UnityEngine;

public enum ColliderTag
{
    Player,
    Ground,
    Obstacle,
    FallZone,
    FinishZone
}

public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float _width = 1f;
    [SerializeField] private float _length = 1f;
    [SerializeField] private ColliderTag _tag = ColliderTag.Ground;

    public ColliderTag Tag => _tag;
    public float Width => _width;
    public float Length => _length;

    public Vector2 Max { get; private set; }
    public Vector2 Min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Unregister(this);
        }
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        float halfW = _width * 0.5f;
        float halfL = _length * 0.5f;

        Max = new Vector2(pos.x + halfW, pos.y + halfL);
        Min = new Vector2(pos.x - halfW, pos.y - halfL);

        Debug.DrawLine(new Vector3(Min.x, Max.y), new Vector3(Max.x, Max.y), Color.magenta);
        Debug.DrawLine(new Vector3(Max.x, Max.y), new Vector3(Max.x, Min.y), Color.magenta);
        Debug.DrawLine(new Vector3(Max.x, Min.y), new Vector3(Min.x, Min.y), Color.magenta);
        Debug.DrawLine(new Vector3(Min.x, Min.y), new Vector3(Min.x, Max.y), Color.magenta);
    }
}