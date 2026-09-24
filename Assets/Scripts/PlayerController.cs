using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public SceneHandler sceneHandler = GameObject.Find("SceneHandler").GetComponent<SceneHandler>();
    public float speed = 5f;

    [SerializeField] private float minX = -15f;
    [SerializeField] private float maxX = 15f;
    [SerializeField] private float minZ = -15f;
    [SerializeField] private float maxZ = 15f;

    [SerializeField] private float proximityRadius = 5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // keeping commented code just in case restricted movement is needed in the future
        /*if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            Vector3 direction = new Vector3(horizontalInput, 0.0f, 0);
            transform.position += direction * speed * Time.deltaTime;
        }
        else if(Mathf.Abs(verticalInput) > 0.1f)
        {
            Vector3 direction = new Vector3(0, 0.0f, verticalInput);
            transform.position += direction * speed * Time.deltaTime;
        };*/

        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        transform.position += direction * speed * Time.deltaTime;

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);


        //placeholder code, was not able to learn alternative to FindObjectsOfType in time
        ProjectileLogic[] activeProjectiles = FindObjectsOfType<ProjectileLogic>();

        foreach (ProjectileLogic projectile in activeProjectiles)
        {
            float distance = Vector3.Distance(projectile.transform.position, transform.position);
            if (distance <= proximityRadius)
            {
                sceneHandler.RestartScene();
                break;
            }
        }
    }
}