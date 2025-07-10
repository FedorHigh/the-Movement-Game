using UnityEngine;
using UnityEngine.UI;

public class CameraInvertUI : MonoBehaviour
{
    [SerializeField] private Toggle toggleX;
    [SerializeField] private Toggle toggleY;

    void Start()
    {
        // Загружаем сохранённые настройки
        toggleX.isOn = SettingsManager.Instance.IsCameraXInverted();
        toggleY.isOn = SettingsManager.Instance.IsCameraYInverted();

        // Подписываемся на изменения
        toggleX.onValueChanged.AddListener(OnToggleXChanged);
        toggleY.onValueChanged.AddListener(OnToggleYChanged);
    }

    private void OnToggleXChanged(bool isOn) => SettingsManager.Instance.SetInvertCameraX(isOn);
    private void OnToggleYChanged(bool isOn) => SettingsManager.Instance.SetInvertCameraY(isOn);

    void OnDestroy()
    {
        toggleX.onValueChanged.RemoveListener(OnToggleXChanged);
        toggleY.onValueChanged.RemoveListener(OnToggleYChanged);
    }
}