using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorChanger : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeAlpha(float startAlpha, float endAlpha, float value)
    {
        Color color = _meshRenderer.material.color;
        color.a = Mathf.Lerp(startAlpha, endAlpha, value);
        _meshRenderer.material.color = color;
    }
}