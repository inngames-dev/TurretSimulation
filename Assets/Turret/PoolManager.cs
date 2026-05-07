using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public enum PoolType
    {
        Projectile,
        Drone
    }

    public static PoolManager Instance { get; private set; }

    [Header("Projectile Pool")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilePoolSize = 20;

    [Header("Drone Pool")]
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private int dronePoolSize = 10;

    private Queue<GameObject> projectilePool = new Queue<GameObject>();
    private Queue<GameObject> dronePool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CreatePool(PoolType.Projectile, projectilePrefab, projectilePoolSize);
        CreatePool(PoolType.Drone, dronePrefab, dronePoolSize);
    }

    private void CreatePool(PoolType type, GameObject prefab, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);

            GetPool(type).Enqueue(obj);
        }
    }

    public GameObject GetObject(PoolType type, Vector3 position, Quaternion rotation)
    {
        Queue<GameObject> pool = GetPool(type);
        GameObject prefab = GetPrefab(type);

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    public void ReturnObject(PoolType type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);

        GetPool(type).Enqueue(obj);
    }

    private Queue<GameObject> GetPool(PoolType type)
    {
        switch (type)
        {
            case PoolType.Projectile:
                return projectilePool;

            case PoolType.Drone:
                return dronePool;

            default:
                return null;
        }
    }

    private GameObject GetPrefab(PoolType type)
    {
        switch (type)
        {
            case PoolType.Projectile:
                return projectilePrefab;

            case PoolType.Drone:
                return dronePrefab;

            default:
                return null;
        }
    }
}
