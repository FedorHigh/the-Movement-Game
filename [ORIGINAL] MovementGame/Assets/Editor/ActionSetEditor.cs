using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))] // указываем, для какого класса
public class PlayerEditor : Editor
{
    // SerializedObject и SerializedProperty нужны для корректной работы Undo и Prefab-системы
    private SerializedProperty playerName;
    private SerializedProperty health;
    private SerializedProperty speed;
    private SerializedProperty isActive;
    private SerializedProperty action;

    private void OnEnable()
    {
        // Привязываем поля
        playerName = serializedObject.FindProperty("playerName");
        health = serializedObject.FindProperty("health");
        speed = serializedObject.FindProperty("speed");
        isActive = serializedObject.FindProperty("isActive");
        action = serializedObject.FindProperty("action");
    }

    public override void OnInspectorGUI()
    {
        // Обновляем данные из объекта
        serializedObject.Update();

        GUILayout.Label("Player Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.PropertyField(playerName);
        EditorGUILayout.PropertyField(health);
        EditorGUILayout.PropertyField(speed);
        EditorGUILayout.PropertyField(isActive);
        EditorGUILayout.PropertyField(action, includeChildren: true);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        if (GUILayout.Button("Reset Stats"))
        {
            ((Player)target).ResetStats();
        }
        if (GUILayout.Button("Print Stats"))
        {
            ((Player)target).PrintStats();
        }

        // Применяем изменения
        serializedObject.ApplyModifiedProperties();
    }
}
