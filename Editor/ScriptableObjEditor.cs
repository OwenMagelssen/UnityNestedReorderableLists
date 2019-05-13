using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(ScriptableObj))]
public class ScriptableObjEditor : Editor
{
    ReorderableList list;

    private void Awake()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("MyClass1List"), true, true, true, true);
        list.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            return element.FindPropertyRelative("elementHeight").floatValue;
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            float indent = 10;
            rect.x += indent;
            rect.width -= indent;
            rect.y += 2;
            EditorGUI.PropertyField(rect, serializedObject.FindProperty("MyClass1List").GetArrayElementAtIndex(index), true);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "MyClass1 List");
        };
        list.onRemoveCallback = (ReorderableList l) =>
        {
            ScriptableObj o = (ScriptableObj)l.serializedProperty.serializedObject.targetObject;
            o.MyClass1List.RemoveAt(l.index);
        };
        list.onAddCallback = (ReorderableList l) =>
        {
            ScriptableObj o = (ScriptableObj)l.serializedProperty.serializedObject.targetObject;
            o.MyClass1List.Add(new MyClass1());
        };
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
