using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 3f;

    private float lifeTimer;
    private bool isReturned;

    private void OnEnable()
    {
        lifeTimer = lifeTime;
        isReturned = false;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drone"))
        {
            Drone drone = other.GetComponent<Drone>();

            if (drone != null) drone.ReturnToPool();
            
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }

        isReturned = true;
        PoolManager.Instance.ReturnObject(PoolManager.PoolType.Projectile, gameObject);
    }
}
