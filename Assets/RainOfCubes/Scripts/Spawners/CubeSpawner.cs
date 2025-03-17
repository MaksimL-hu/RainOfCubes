using System;
using UnityEngine;

public class CubeSpawner : GenericSpawner<Cube>
{
    [Tooltip("Платформа над которой будут спавниться объекты")]
    [SerializeField] private Transform _mainPlatform;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private float _repeatRate = 1.0f;

    private Vector3 _position;

    public event Action<Cube> CubeReleased;

    protected override void Awake()
    {
        _position = new Vector3(
            _mainPlatform.transform.position.x, 
            _mainPlatform.transform.position.y + _offset.y, 
            _mainPlatform.transform.position.z);

        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetObject), 0.0f, _repeatRate);
    }

    protected override Cube InstantiateObject()
    {
        Cube cube = base.InstantiateObject();
        cube.Switched += ReleaseObject;

        return cube;
    }

    protected override void OnActionGet(Cube cube)
    {
        base.OnActionGet(cube);
        cube.transform.position = GetRandomSpawnPoint();
    }

    protected override void DestroyObject(Cube cube)
    {
        base.DestroyObject(cube);
        cube.Switched -= ReleaseObject;
    }

    protected override void OnActionRelease(Cube cube)
    {
        base.OnActionRelease(cube);
        CubeReleased?.Invoke(cube);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 spawnPoint = new Vector3(
            UnityEngine.Random.Range(_position.x - _offset.x, _position.x + _offset.x),
            _position.y,
            UnityEngine.Random.Range(_position.z - _offset.z, _position.z + _offset.z)
            );

        return spawnPoint;
    }
}