using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas _instance;

    void Awake()
    {
        // Если уже есть такой Canvas — уничтожить дубликат
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Иначе сделать его неуничтожаемым
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}