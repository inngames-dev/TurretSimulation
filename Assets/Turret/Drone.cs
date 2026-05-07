using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float arriveDistance = 1f;

    private Transform target;
    private bool isReturned;

    public void Init(Transform targetTransform)
    {
        target = targetTransform;
        isReturned = false;
    }

    private void OnEnable()
    {
        isReturned = false;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude <= arriveDistance)
        {
            ReturnToPool();
            return;
        }

        Vector3 moveDirection = direction.normalized;

        transform.position += moveDirection * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }

    public void ReturnToPool()
    {
        if (isReturned)
        {
            return;
        }

        isReturned = true;
        PoolManager.Instance.ReturnObject(PoolManager.PoolType.Drone, gameObject);
    }
}
