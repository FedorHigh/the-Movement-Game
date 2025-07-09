using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public enum Axis { X, Y }
    public Axis axis;

    [SerializeField] private Slider slider;

    void Start()
    {
        SettingsManager.OnSettingsLoaded += LoadValue;
        SettingsManager.OnSettingsUpdated += LoadValue;
        slider.onValueChanged.AddListener(OnSliderChanged);

        LoadValue();
    }

    void LoadValue()
    {
        GameSettings settings = SettingsManager.Instance.GetSettings();
        slider.value = axis == Axis.X ? settings.xSensitivity : settings.ySensitivity;
    }

    void OnSliderChanged(float value)
    {
        if (axis == Axis.X)
            SettingsManager.Instance.SetX(value);
        else
            SettingsManager.Instance.SetY(value);
    }
}