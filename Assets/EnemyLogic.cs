using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public ScoreHandler scoreHandler;

    [SerializeField]
    private float proximityRadius = 5f;

    void Start()
    {
        scoreHandler = GameObject.Find("ScoreHandler").GetComponent<ScoreHandler>();
    }

    void Update()
    {
        //placeholder code
        ProjectileLogic[] activeRockets = FindObjectsOfType<ProjectileLogic>();

        // 2. Loop through them and check distance directly
        foreach (ProjectileLogic rocket in activeRockets)
        {
            float distance = Vector3.Distance(rocket.transform.position, transform.position);

            // Uncomment this line to see the actual distance in the console!
            // Debug.Log($"Distance to rocket: {distance}");

            // 3. Proximity check
            if (distance <= proximityRadius)
            {
                if (scoreHandler != null)
                {
                    scoreHandler.AddScore(100);
                }

                Destroy(rocket.gameObject);
                Destroy(gameObject);
                break;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
