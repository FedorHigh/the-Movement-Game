using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            // Если экземпляр уже уничтожен (например, при выходе из игры), возвращаем null
            if (_instance == null && !applicationIsQuitting)
            {
                _instance = FindObjectOfType<SettingsManager>();

                // Если не нашли — создаём новый объект
                if (_instance == null)
                {
                    GameObject settingsManagerObject = new GameObject("SettingsManager");
                    _instance = settingsManagerObject.AddComponent<SettingsManager>();
                    DontDestroyOnLoad(settingsManagerObject);
                }
            }

            return _instance;
        }
    }

    private GameSettings currentSettings = new GameSettings();
    private string filePath;
    private static bool applicationIsQuitting = false;

    public static System.Action OnSettingsLoaded;
    public static System.Action OnSettingsUpdated;

    void Awake()
    {
        // Проверяем, не создали ли мы уже этот объект
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Убедимся, что этот объект будет один
        _instance = this;
        DontDestroyOnLoad(gameObject);

        filePath = Path.Combine(Application.persistentDataPath, "gamesettings.json");

        LoadSettings();
    }

    void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            currentSettings = JsonUtility.FromJson<GameSettings>(json);
        }
        else
        {
            SaveSettings(); // Сохраняем дефолтные настройки
        }

        OnSettingsLoaded?.Invoke();
    }

    public void SaveSettings()
    {
        if (currentSettings == null) return;

        string json = JsonUtility.ToJson(currentSettings, true);
        File.WriteAllText(filePath, json);
        OnSettingsUpdated?.Invoke();
    }

    public GameSettings GetSettings() => currentSettings;

    public void SetX(float value)
    {
        if (currentSettings == null) return;
        currentSettings.xSensitivity = value;
        SaveSettings();
    }

    public void SetY(float value)
    {
        if (currentSettings == null) return;
        currentSettings.ySensitivity = value;
        SaveSettings();
    }
    public void SetInvertCameraX(bool isInverted)
    {
        currentSettings.invertCameraX = isInverted;
        SaveSettings();
    }

    public void SetInvertCameraY(bool isInverted)
    {
        currentSettings.invertCameraY = isInverted;
        SaveSettings();
    }

    public bool IsCameraXInverted() => currentSettings.invertCameraX;
    public bool IsCameraYInverted() => currentSettings.invertCameraY;
}
