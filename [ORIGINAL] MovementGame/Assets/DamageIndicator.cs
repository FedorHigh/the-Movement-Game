using UnityEngine;
using System.Collections.Generic;

public class DamageIndicator : MonoBehaviour
{
    // ������ ���� ���������� � �������� (SpriteRenderer, MeshRenderer � �. �.)
    private Renderer[] _renderers;
    private List<Color> _originalColors = new List<Color>();
    public float flashDuration = 0.5f;
    public ParticleSystem particles;

    void Start()
    {
        // �������� ��� ��������� (������� ��������)
        _renderers = GetComponentsInChildren<Renderer>(false);

        // ��������� ������������ �����
        foreach (var renderer in _renderers)
        {
            
                _originalColors.Add(renderer.material.color);
        }
    }

    public void FlashRed(float duration = -1)
    {
        if (duration < 0) duration = flashDuration;

        if (particles != null) particles.Play();
        // �������� �� ���� ���������� � ������ ����
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] is SpriteRenderer spriteRenderer)
                spriteRenderer.color = Color.red;
            else if (_renderers[i] is MeshRenderer meshRenderer)
                meshRenderer.material.color = Color.red;
        }

        // ���������� ���� ����� flashDuration ������
        Invoke(nameof(ResetColors), duration);
        Debug.Log("colors flashed successfully");
    }
    public void FlashBlue(float duration = -1)
    {
        if(duration < 0) duration = flashDuration;

        if (particles != null) particles.Play();
        // �������� �� ���� ���������� � ������ ����
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] is SpriteRenderer spriteRenderer)
                spriteRenderer.color = Color.blue;
            else if (_renderers[i] is MeshRenderer meshRenderer)
                meshRenderer.material.color = Color.blue;
        }

        // ���������� ���� ����� flashDuration ������
        Invoke(nameof(ResetColors), duration);
        Debug.Log("colors flashed successfully");
    }

    private void ResetColors()
    {
        // ��������������� ������������ �����
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (i >= _renderers.Length || i >= _originalColors.Count) break;
            if (_renderers[i] is SpriteRenderer spriteRenderer)
                spriteRenderer.color = _originalColors[i];
            else if (_renderers[i] is MeshRenderer meshRenderer)
                meshRenderer.material.color = _originalColors[i];
        }
        Debug.Log("colors restored successfully");
    }
}