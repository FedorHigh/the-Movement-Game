using UnityEngine;

public class PersistentCanvas : MonoBehaviour
{
    private static PersistentCanvas _instance;

    void Awake()
    {
        // ���� ��� ���� ����� Canvas � ���������� ��������
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // ����� ������� ��� ��������������
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}