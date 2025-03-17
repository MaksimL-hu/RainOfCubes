using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Bomb : SpawnObject
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private Material _standartMaterial;

    private float _startAltha = 1;
    private float _endAltha = 0;
    private MeshRenderer _meshRenderer;

    public event Action<Bomb> Exploded;

    protected override void Awake()
    {
        base.Awake(); 
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public IEnumerator Living()
    {
        int livingTime = GetRandomLivingTime();
        float time = 0;

        while (time < livingTime)
        {
            time += Time.deltaTime;

            ChangeAlpha(time / livingTime);

            yield return null;
        }

        ChangeAlpha(_endAltha);

        Explode();
    }

    private void ChangeAlpha(float value)
    {
        Color color = _meshRenderer.material.color;
        color.a = Mathf.Lerp(_startAltha, _endAltha, value);
        _meshRenderer.material.color = color;
    }

    private void Explode()
    {
        foreach(Rigidbody explodableObject in GetExplodableObjects())
            explodableObject.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

        Exploded?.Invoke(this);
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);

        List<Rigidbody> explodableObjects = new List<Rigidbody>();

        foreach (Collider hit in hits)
            if(hit.attachedRigidbody != null)
                explodableObjects.Add(hit.attachedRigidbody);

        return explodableObjects;
    }
}