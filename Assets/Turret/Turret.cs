using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header ("오브젝트 참조")]
    public Transform yawPivot;
    public Transform pitchPivot;
    public Transform muzzlePoint;

    [Header ("회전 값")]
    public float yawSpeed;
    public float pitchSpeed;
    public float minPitch;
    public float maxPitch;

    [Header ("발사 값")]
    public float fireAngle;
    public float fireInterval;

    [Header ("탐색 값")]
    public float searchInterval = 0.2f;

    private float nextFireTime;
    private float nextSearchTime;
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        SearchTarget();

        if (target == null) return;

        RotateYaw();
        RotatePitch();
        FireAimed();
    }

    void RotateYaw()
    {
        Vector3 dir = target.position - yawPivot.position;

        // 높이 제외
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        yawPivot.rotation = Quaternion.RotateTowards(yawPivot.rotation, targetRot, yawSpeed * Time.deltaTime);
    }

    void RotatePitch()
    {
        Vector3 dir = target.position - pitchPivot.position;

        Vector3 localDir = yawPivot.InverseTransformDirection(dir);

        float distance = new Vector2(localDir.x, localDir.z).magnitude;

        float angle = -Mathf.Atan2(localDir.y, distance) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, minPitch, maxPitch);

        Quaternion targetRot = Quaternion.Euler(angle, 0f, 0f);

        pitchPivot.localRotation = Quaternion.RotateTowards( pitchPivot.localRotation, targetRot, pitchSpeed * Time.deltaTime);
    }

    void SearchTarget()
    {
        if (Time.time < nextSearchTime) return;

        nextSearchTime = Time.time + searchInterval;

        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        for (int i = 0; i < drones.Length; i++)
        {
            Vector3 dir = drones[i].transform.position - yawPivot.position;
            float distance = dir.sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = drones[i].transform;
            }
        }

        target = closestTarget;
    }

    void FireAimed()
    {
        Vector3 dir = target.position - muzzlePoint.position;

        float angle = Vector3.Angle(muzzlePoint.forward, dir);

        bool isAimed = angle <= fireAngle;

        if (isAimed && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireInterval;
        }
    }

    void Fire()
    {
        PoolManager.Instance.GetObject(PoolManager.PoolType.Projectile, muzzlePoint.position, muzzlePoint.rotation);
    }
}
