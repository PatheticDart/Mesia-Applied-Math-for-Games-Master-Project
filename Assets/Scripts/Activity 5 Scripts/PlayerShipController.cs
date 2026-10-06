using UnityEngine;
using UnityEngine.Rendering;

public class PlayerShipController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float rotationSpeed = 20f;

    [SerializeField] private float rollSpeed = 180f;
    [SerializeField] private float maxRollAngle = 30f;

    private float currentRoll = 0f;

    public GameObject shipMesh;
    public Transform shipMeshTransform;

    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;
    public GameObject camera4;

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.C))
        {
            switch(camera1.activeSelf)
            {
                case true:
                    camera1.SetActive(false);
                    camera2.SetActive(true);
                    break;
                case false when camera2.activeSelf:
                    camera2.SetActive(false);
                    camera3.SetActive(true);
                    break;
                case false when camera3.activeSelf:
                    camera3.SetActive(false);
                    camera4.SetActive(true);
                    break;
                case false when camera4.activeSelf:
                    camera4.SetActive(false);
                    camera1.SetActive(true);
                    break;
            }
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * -(rotationSpeed), Vector3.up);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotationSpeed, Vector3.up);
        }

        //barrel roll stuff (that I shouldn't even be focusing on rn)
        if (Input.GetAxis("Horizontal") != 0f)
        {
            currentRoll += Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        }
        else
        {
            currentRoll = Mathf.Lerp(currentRoll, 0f, Time.deltaTime * 5f);
        }

        currentRoll = Mathf.Clamp(currentRoll, -maxRollAngle, maxRollAngle);
        shipMeshTransform.localRotation = Quaternion.Euler(0f, 0f, -currentRoll);
    }
}