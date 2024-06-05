using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private int _minLivingTimeAfterHit = 2;
    [SerializeField] private int _maxliveTimingAfterHit = 5;
    [SerializeField] private Material _standartMaterial;

    private bool _isHit;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;

    public event Action<Cube> Switched;

    private void Awake()
    {
        _isHit = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            Hit();
        }
    }

    private void SetRangomColor()
    {
        _meshRenderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private int GetRandomLivingTime()
    {
        return UnityEngine.Random.Range(_minLivingTimeAfterHit, _maxliveTimingAfterHit + 1);
    }

    private IEnumerator Switching()
    {
        int livingTime = GetRandomLivingTime();
        
        yield return new WaitForSeconds(livingTime);

        Switched?.Invoke(this);
    }

    public void Reset()
    {
        _isHit = false;
        _meshRenderer.material = _standartMaterial;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _rigidbody.rotation = Quaternion.Euler(Vector3.zero);
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    private void Hit()
    {
        if (_isHit == false)
        {
            SetRangomColor();
            StartCoroutine(Switching());
            _isHit = true;
        }
    }
}