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
        freeLookCamera.m_XAxis.m_MaxSpeed = settings.xSensitivity * 2400f;
        freeLookCamera.m_YAxis.m_MaxSpeed = settings.ySensitivity * 80f;

        freeLookCamera.m_XAxis.m_InvertInput = settings.invertCameraX;
        freeLookCamera.m_YAxis.m_InvertInput = settings.invertCameraY;
    }

    void OnDestroy()
    {
        SettingsManager.OnSettingsUpdated -= UpdateCamera;
    }
}