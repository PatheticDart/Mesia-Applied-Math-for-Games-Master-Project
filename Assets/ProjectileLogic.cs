using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public Vector3 direction;

    [SerializeField]
    private float speed = 5f;

    void Start()
    {
        // Good practice to ensure it's normalized!
        direction.Normalize();
    }

    void Update()
    {
        // Explicitly set Space.World to prevent movement skewing
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}