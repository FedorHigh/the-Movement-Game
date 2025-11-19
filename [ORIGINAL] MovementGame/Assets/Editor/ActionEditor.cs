using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Action))] // указываем, для какого класса
public class ActionEditor : Editor
{
    // SerializedObject и SerializedProperty нужны для корректной работы Undo и Prefab-системы
    private SerializedProperty actname;

    private void OnEnable()
    {
        // Привязываем поля
        actname = serializedObject.FindProperty("displayName");
    }

    public override void OnInspectorGUI()
    {
        // Обновляем данные из объекта
        serializedObject.Update();


        EditorGUILayout.PropertyField(actname);


        GUILayout.Space(10);

        // Применяем изменения
        serializedObject.ApplyModifiedProperties();
    }
}
