using UnityEngine;

public class FinishZoneLogic : MonoBehaviour
{
    public GameObject player;
    public GameObject endPanel;

    public float proximityRadius = 5f;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        var distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= proximityRadius)
        {
            Time.timeScale = 0f;
            endPanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            endPanel.SetActive(false);
        }
    }
}