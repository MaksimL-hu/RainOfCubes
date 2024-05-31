using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Tooltip("Платформа над которой будут спавниться объекты")]
    [SerializeField] private Transform _mainPlatform;
    [SerializeField] private Cube _prefab;

    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _offsetZ;

    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 40;

    private ObjectPool<Cube> _pool;
    private float _positionX;
    private float _positionY;
    private float _positionZ;

    private void Awake()
    {
        _positionX = _mainPlatform.transform.position.x;
        _positionY = _mainPlatform.transform.position.y + _offsetY;
        _positionZ = _mainPlatform.transform.position.z;

        _pool = new ObjectPool<Cube>(
            createFunc: () => InstantiateCube(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => ActionOnRelease(cube),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private Cube InstantiateCube()
    {
        Cube cube = Instantiate(_prefab);
        cube.OnSwitching += ReleaseCube;

        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.Reset();
        cube.transform.position = GetRandomSpawnPoint();
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private void DestroyCube(Cube cube)
    {
        cube.OnSwitching -= ReleaseCube;
        Destroy(cube);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(_positionX - _offsetX, _positionX + _offsetX),
            _positionY,
            Random.Range(_positionZ - _offsetZ, _positionZ + _offsetZ)
            );

        return spawnPoint;
    }
}