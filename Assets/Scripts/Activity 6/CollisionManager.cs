using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance => instance;
    private static CollisionManager instance;

    private List<AABBCollider> colliders = new List<AABBCollider>();
    public IReadOnlyList<AABBCollider> Colliders => colliders;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(AABBCollider collider)
    {
        if (!colliders.Contains(collider))
            colliders.Add(collider);
    }

    public void Unregister(AABBCollider collider)
    {
        if (colliders.Contains(collider))
            colliders.Remove(collider);
    }

    public static bool Overlaps(AABBCollider a, AABBCollider b)
    {
        return a.Min.x < b.Max.x && a.Max.x > b.Min.x &&
               a.Min.y < b.Max.y && a.Max.y > b.Min.y;
    }
}