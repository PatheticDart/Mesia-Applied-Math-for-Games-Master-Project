using UnityEngine;

public class TrigoMover : MonoBehaviour
{
    [SerializeField]
    private float xlength, ylength, timeMultiplier;

    private float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime * timeMultiplier;

        transform.position = new Vector3(
            xlength * Mathf.Cos(elapsedTime * Mathf.Deg2Rad),
            ylength * Mathf.Sin(elapsedTime * Mathf.Deg2Rad),
            0
        ) * Time.deltaTime;
    }
}
