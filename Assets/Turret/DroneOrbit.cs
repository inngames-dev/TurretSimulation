using UnityEngine;

public class DroneOrbit : MonoBehaviour
{
    public float rotateSpeed;

    [Header ("Drone Control")]
    public Transform drone;
    public float heightAmount;
    public float heightSpeed;

    private Vector3 startLocalPosition;

    void Start()
    {
        startLocalPosition = drone.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);

        Vector3 pos = startLocalPosition;

        pos.y += Mathf.Sin(Time.time * heightSpeed) * heightAmount;

        drone.localPosition = pos;
    }
}
