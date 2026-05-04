using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header ("오브젝트 참조")]
    public Transform drone;
    public Transform yawPivot;
    public Transform pitchPivot;
    public Transform muzzlePoint;
    public GameObject projectilePrefab;

    [Header ("회전 값")]
    public float yawSpeed;
    public float pitchSpeed;
    public float minPitch;
    public float maxPitch;

    [Header ("발사 값")]
    public float fireAngle;
    public float fireInterval;

    private float nextFireTime;

    // Update is called once per frame
    void Update()
    {
        if (drone == null) return;

        RotateYaw();
        RotatePitch();
        FireAimed();
    }

    void RotateYaw()
    {
        Vector3 dir = drone.position - yawPivot.position;

        // 높이 제외
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion droneRot = Quaternion.LookRotation(dir);

        yawPivot.rotation = Quaternion.RotateTowards(yawPivot.rotation, droneRot, yawSpeed * Time.deltaTime);
    }

    void RotatePitch()
    {
        Vector3 dir = drone.position - pitchPivot.position;

        Vector3 localDir = yawPivot.InverseTransformDirection(dir);

        float distance = new Vector2(localDir.x, localDir.z).magnitude;

        float angle = -Mathf.Atan2(localDir.y, distance) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, minPitch, maxPitch);

        Quaternion droneRot = Quaternion.Euler(angle, 0f, 0f);

        pitchPivot.localRotation = Quaternion.RotateTowards( pitchPivot.localRotation, droneRot, pitchSpeed * Time.deltaTime);
    }

    void FireAimed()
    {
        Vector3 dir = drone.position - muzzlePoint.position;

        float angel = Vector3.Angle(muzzlePoint.forward, dir);

        bool isAimed = angel <= fireAngle;

        if (isAimed && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
        }
    }

    void Fire()
    {
        Instantiate(projectilePrefab, muzzlePoint.position, muzzlePoint.rotation);
    }
}
