using UnityEngine;
using System.Collections.Generic;

public class DamageIndicator : MonoBehaviour
{
    // Массив всех рендереров в иерархии (SpriteRenderer, MeshRenderer и т. д.)
    private Renderer[] _renderers;
    private List<Color> _originalColors = new List<Color>();
    public float flashDuration = 0.5f;

    void Start()
    {
        // Получаем все рендереры (включая дочерние)
        _renderers = GetComponentsInChildren<Renderer>(true);

        // Сохраняем оригинальные цвета
        foreach (var renderer in _renderers)
        {
            if (renderer is SpriteRenderer spriteRenderer)
                _originalColors.Add(spriteRenderer.color);
            else if (renderer is MeshRenderer meshRenderer)
                _originalColors.Add(meshRenderer.material.color);
        }
    }

    public void FlashRed()
    {
        // Проходим по всем рендерерам и меняем цвет
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] is SpriteRenderer spriteRenderer)
                spriteRenderer.color = Color.red;
            else if (_renderers[i] is MeshRenderer meshRenderer)
                meshRenderer.material.color = Color.red;
        }

        // Возвращаем цвет через flashDuration секунд
        Invoke(nameof(ResetColors), flashDuration);
    }

    private void ResetColors()
    {
        // Восстанавливаем оригинальные цвета
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_renderers[i] is SpriteRenderer spriteRenderer)
                spriteRenderer.color = _originalColors[i];
            else if (_renderers[i] is MeshRenderer meshRenderer)
                meshRenderer.material.color = _originalColors[i];
        }
    }
}