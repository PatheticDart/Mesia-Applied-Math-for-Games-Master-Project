using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public ScoreHandler scoreHandler;

    public static List<EnemyLogic> ActiveEnemies { get; private set; } = new List<EnemyLogic>();

    [SerializeField]
    private float proximityRadius = 5f;

    void Start()
    {
        scoreHandler = GameObject.Find("Resource Manager").GetComponent<ScoreHandler>();
    }

    void Update()
    {
        //placeholder code (I really need to figure out how to do this properly T-T)
        ProjectileLogic[] activeProjectiles = FindObjectsOfType<ProjectileLogic>();

        // 2. Loop through them and check distance directly
        foreach (ProjectileLogic projectile in activeProjectiles)
        {
            float distance = Vector3.Distance(projectile.transform.position, transform.position);

            // Uncomment this line to see the actual distance in the console!
            // Debug.Log($"Distance to projectile: {distance}");

            // 3. Proximity check
            if (distance <= proximityRadius)
            {
                if (scoreHandler != null)
                {
                    scoreHandler.AddScore(100);
                }

                Destroy(projectile.gameObject);
                Destroy(gameObject);
                break;
            }
        }


    }

    private void OnEnable()
    {
        ActiveEnemies.Add(this);
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
