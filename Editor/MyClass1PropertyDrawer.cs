using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomPropertyDrawer(typeof(MyClass1))]
public class MyClass1PropertyDrawer : PropertyDrawer
{
    Rect intRect;
    Rect listRect;
    Rect subPropsRect;
    float indent = 8;
    float height;
    float yPos;
    SerializedProperty myInt;
    SerializedProperty myClass2List;
    ReorderableList list;
    MyClass1 myClassObject;
    System.Object propertyObject;

    private Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.serializedObject.Update();
        
        myClassObject = fieldInfo.GetValue(property.serializedObject.targetObject) as MyClass1;

        myInt = property.FindPropertyRelative("myInt");
        myClass2List = property.FindPropertyRelative("MyClass2List");

        height = 0;
        yPos = position.y;

        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        height += position.height + EditorGUIUtility.standardVerticalSpacing; ;
        yPos += position.height + EditorGUIUtility.standardVerticalSpacing; ;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, new GUIContent("Element"));

        if (property.isExpanded)
        {
            subPropsRect = new Rect(position);
            subPropsRect.x += indent;
            subPropsRect.width -= indent;

            intRect = new Rect(subPropsRect);
            intRect.y = yPos;
            intRect.height = EditorGUI.GetPropertyHeight(myInt);
            height += intRect.height + EditorGUIUtility.standardVerticalSpacing * 2;
            yPos += intRect.height + EditorGUIUtility.standardVerticalSpacing * 2;
            EditorGUI.PropertyField(intRect, myInt);

            list = GetNestedReorderableList(myClass2List);
            listRect = new Rect(subPropsRect);
            listRect.y = yPos;
            listRect.height = list.GetHeight();
            height += listRect.height + EditorGUIUtility.standardVerticalSpacing;
            list.DoList(listRect);
        }

        property.FindPropertyRelative("elementHeight").floatValue = height + EditorGUIUtility.standardVerticalSpacing;
        
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }

    private ReorderableList BuildReorderableList(SerializedProperty property)
    {
        ReorderableList list = new ReorderableList(property.serializedObject, property, true, true, true, true);
        list.elementHeightCallback = (int index) =>
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            return element.FindPropertyRelative("elementHeight").floatValue;
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.x += 10;
            rect.width -= 10;
            rect.y += 2;
            rect.height = list.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("elementHeight").floatValue;
            EditorGUI.PropertyField(rect, list.serializedProperty.GetArrayElementAtIndex(index), true);
        };
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "MyClass2 List");
        };
        return list;
    }

    private ReorderableList GetNestedReorderableList(SerializedProperty property)
    {
        string listKey = property.propertyPath;
        ReorderableList nestedReorderableList;

        if (innerListDict.ContainsKey(listKey))
        {
            nestedReorderableList = innerListDict[listKey];
        }
        else
        {
            nestedReorderableList = BuildReorderableList(property);
            innerListDict[listKey] = nestedReorderableList;
        }
        return nestedReorderableList;
    }
}
