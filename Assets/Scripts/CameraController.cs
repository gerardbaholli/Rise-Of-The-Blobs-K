using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private GameObject tower;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            float rotationAmount = 1 * rotationSpeed * Time.deltaTime;
            transform.RotateAround(tower.transform.position, Vector3.up, rotationAmount);
        }

        if (Input.GetKey(KeyCode.D))
        {
            float rotationAmount = -1 * rotationSpeed * Time.deltaTime;
            transform.RotateAround(tower.transform.position, Vector3.up, rotationAmount);
        }
    }

}
