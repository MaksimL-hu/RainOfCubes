using System;
using System.Collections;
using UnityEngine;

public class Bomb : SpawnObject
{
    [SerializeField] ColorChanger _colorChanger;
    [SerializeField] Exploder _exploder;

    private float _startAlpha = 1;
    private float _endAlpha = 0;

    public event Action<Bomb> Exploded;

    public IEnumerator Living()
    {
        int livingTime = GetRandomLivingTime();
        float time = 0;

        while (time < livingTime)
        {
            time += Time.deltaTime;

            _colorChanger.ChangeAlpha(_startAlpha, _endAlpha, time / livingTime);

            yield return null;
        }

        _colorChanger.ChangeAlpha(_startAlpha, _endAlpha, _endAlpha);
        _exploder.Explode();
        Exploded?.Invoke(this);
    }
}