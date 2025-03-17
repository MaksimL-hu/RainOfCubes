using UnityEngine;

public class BombSpawner : GenericSpawner<Bomb>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    private Cube _cube;

    private void OnEnable()
    {
        _cubeSpawner.CubeReleased += GetBomb;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeReleased -= GetBomb;
    }

    protected override Bomb InstantiateObject()
    {
        Bomb bomb = base.InstantiateObject();
        bomb.Exploded += ReleaseObject;

        return bomb;
    }

    protected override void OnActionGet(Bomb bomb)
    {
        base.OnActionGet(bomb);
        bomb.transform.position = _cube.transform.position;
        bomb.transform.rotation = _cube.transform.rotation;
        StartCoroutine(bomb.Living());
    }

    protected override void DestroyObject(Bomb bomb)
    {
        bomb.Exploded -= ReleaseObject;
        base.DestroyObject(bomb);
    }

    private void GetBomb(Cube cube)
    {
        _cube = cube;
        GetObject();
    }
}