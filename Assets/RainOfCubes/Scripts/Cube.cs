using System;
using UnityEngine;

public class Cube : SpawnObject
{
    public event Action<Cube> Switched;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            Switched?.Invoke(this);
        }
    }
}