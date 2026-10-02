using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public ScoreHandler scoreHandler;

    public static List<EnemyLogic> ActiveEnemies { get; private set; } = new List<EnemyLogic>();

    [SerializeField]
    private float proximityRadius = 5f;

    [SerializeField] private GameObject uiCoinPrefab;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private RectTransform bankUIRect;

    void Start()
    {
        scoreHandler = GameObject.Find("Resource Manager").GetComponent<ScoreHandler>();
        canvasTransform = GameObject.Find("Canvas").transform;
        bankUIRect = GameObject.Find("Coin Counter").GetComponent<RectTransform>();
    }

    void Update()
    {
        //placeholder code (I really need to figure out how to do this properly T-T)
        ProjectileLogic[] activeProjectiles = FindObjectsOfType<ProjectileLogic>();

        foreach (ProjectileLogic projectile in activeProjectiles)
        {
            float distance = Vector3.Distance(projectile.transform.position, transform.position);

            if (distance <= proximityRadius)
            {
                if (scoreHandler != null)
                {
                    scoreHandler.AddScore(100);
                }

                GameObject coinUIObj = Instantiate(uiCoinPrefab, canvasTransform);

                if (coinUIObj.TryGetComponent<CoinWorldToUI>(out var coinUI))
                {
                    coinUI.Initialize(transform.position, bankUIRect, Camera.main, value: 10, flyDuration: 1.2f);
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
