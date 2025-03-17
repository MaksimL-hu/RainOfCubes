using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpawnObject : MonoBehaviour
{
    [SerializeField] protected int MinLivingTimeAfterHit = 2;
    [SerializeField] protected int MaxLivingTimeAfterHit = 5;

    protected Rigidbody Rigidbody;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Reset() 
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        Rigidbody.rotation = Quaternion.Euler(Vector3.zero);
        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
    }

    protected virtual int GetRandomLivingTime()
    {
        return Random.Range(MinLivingTimeAfterHit, MaxLivingTimeAfterHit + 1);
    }
}