using UnityEngine;

public class PowerUpLogic : MonoBehaviour
{
    public GameObject player;
    public ScoreHandler scoreHandler;

    [SerializeField]
    private float proximityRadius = 5f;

    void Start()
    {
        player = GameObject.Find("Player");
        scoreHandler = GameObject.Find("ScoreHandler").GetComponent<ScoreHandler>();
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        var distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= proximityRadius)
        {
            Destroy(this.gameObject);
            scoreHandler.AddScore(100);
            player.GetComponent<ShootLogic>().Upgrade();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
