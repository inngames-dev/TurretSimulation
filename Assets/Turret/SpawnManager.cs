using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform turret;
    [SerializeField] private float spawnRadius = 15f;
    [SerializeField] private float spawnInterval = 2f;

    private float spawnTimer;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnDrone();
        }
    }

    private void SpawnDrone()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomY = Random.Range(0f, 2f);
        float radian = randomAngle * Mathf.Deg2Rad;

        Vector3 direction = new Vector3(Mathf.Cos(radian), 0f, Mathf.Sin(radian));
        Vector3 spawnPosition = turret.position + direction * spawnRadius;
        spawnPosition.y += randomY;

        Quaternion rotation = Quaternion.LookRotation(-direction);

        GameObject obj = PoolManager.Instance.GetObject(
            PoolManager.PoolType.Drone,
            spawnPosition,
            rotation
        );

        Drone drone = obj.GetComponent<Drone>();
        drone.Init(turret);
    }
}
