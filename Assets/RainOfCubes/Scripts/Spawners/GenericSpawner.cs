using System;
using UnityEngine;
using UnityEngine.Pool;

public class GenericSpawner<Object> : MonoBehaviour where Object : SpawnObject
{
    [SerializeField] protected Object Prefab;
    [SerializeField] protected int PoolCapacity = 20;
    [SerializeField] protected int PoolMaxSize = 40;

    protected ObjectPool<Object> Pool;
    private int _countActiveObject;

    public event Action ObjectGotten;
    public event Action ObjectSpawned;
    public event Action<int> CountActiveObjectChanched;


    protected virtual void Awake()
    {
        Pool = new ObjectPool<Object>(
            createFunc: () => InstantiateObject(),
            actionOnGet: (@object) => OnActionGet(@object),
            actionOnRelease: (@object) => OnActionRelease(@object),
            actionOnDestroy: (@object) => DestroyObject(@object),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: PoolMaxSize);
    }

    protected virtual Object InstantiateObject()
    {
        Object @object = Instantiate(Prefab);

        ObjectSpawned?.Invoke();

        return @object;
    }

    protected virtual void OnActionGet(Object @object)
    {
        @object.gameObject.SetActive(true);
        @object.Reset();
        ObjectGotten?.Invoke();
        _countActiveObject++;
        CountActiveObjectChanched?.Invoke(_countActiveObject);
    }

    protected virtual void GetObject()
    {
        Pool.Get();
    }

    protected virtual void OnActionRelease(Object @object)
    {
        @object.gameObject.SetActive(false);
        _countActiveObject--;
        CountActiveObjectChanched?.Invoke(_countActiveObject);
    }

    protected virtual void ReleaseObject(Object @object)
    {
        Pool.Release(@object);
    }

    protected virtual void DestroyObject(Object @object)
    {
        Destroy(@object);
    }
}