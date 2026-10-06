using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float rotationSpeed = 20f;

    public GameObject shipMesh;

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * -(rotationSpeed), Vector3.up);

            //barrel roll stuff (that I shouldn't even be focusing on rn)
            shipMesh.transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * 90f, Vector3.forward);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up);
        }
    }
}