using Cinemachine;
using UnityEngine;

public class CameraSensitivity : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    void Start()
    {
        SettingsManager.OnSettingsUpdated += UpdateCamera;
        UpdateCamera();
    }

    void UpdateCamera()
    {
        GameSettings settings = SettingsManager.Instance.GetSettings();
        freeLookCamera.m_XAxis.m_MaxSpeed = settings.xSensitivity * 100f;
        freeLookCamera.m_YAxis.m_MaxSpeed = settings.ySensitivity * 100f;
    }

    void OnDestroy()
    {
        SettingsManager.OnSettingsUpdated -= UpdateCamera;
    }
}