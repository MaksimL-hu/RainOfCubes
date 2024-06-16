using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [Tooltip("Платформа над которой будут спавниться объекты")]
    [SerializeField] private Transform _mainPlatform;
    [SerializeField] private Cube _prefab;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private float _repeatRate = 1.0f;
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 40;

    private ObjectPool<Cube> _pool;
    private Vector3 _position;

    private void Awake()
    {
        _position = new Vector3(
            _mainPlatform.transform.position.x, 
            _mainPlatform.transform.position.y + _offset.y, 
            _mainPlatform.transform.position.z);

        _pool = new ObjectPool<Cube>(
            createFunc: () => InstantiateCube(),
            actionOnGet: (cube) => OnActionGet(cube),
            actionOnRelease: (cube) => OnActionRelease(cube),
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
        cube.Switched += ReleaseCube;

        return cube;
    }

    private void OnActionGet(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.Reset();
        cube.transform.position = GetRandomSpawnPoint();
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnActionRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void ReleaseCube(Cube cube)
    {
        _pool.Release(cube);
    }

    private void DestroyCube(Cube cube)
    {
        cube.Switched -= ReleaseCube;
        Destroy(cube);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            Random.Range(_position.x - _offset.x, _position.x + _offset.x),
            _position.y,
            Random.Range(_position.z - _offset.z, _position.z + _offset.z)
            );

        return spawnPoint;
    }
}