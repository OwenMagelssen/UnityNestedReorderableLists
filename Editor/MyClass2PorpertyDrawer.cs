using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(MyClass2))]
public class MyClass2PorpertyDrawer : PropertyDrawer
{
    float height;
    float yPos;
    Rect foldoutRect;
    Rect fieldRect;
    SerializedProperty myOtherInt;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.serializedObject.Update();
        
        height = 0;
        yPos = position.y;

        foldoutRect = new Rect(position);
        foldoutRect.height = EditorGUIUtility.singleLineHeight;
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, "Element");
        height += foldoutRect.height + EditorGUIUtility.standardVerticalSpacing;
        yPos += foldoutRect.height + EditorGUIUtility.standardVerticalSpacing;

        if (property.isExpanded)
        {
            myOtherInt = property.FindPropertyRelative("myOtherInt");
            fieldRect = new Rect(position);
            fieldRect.height = EditorGUI.GetPropertyHeight(myOtherInt);
            fieldRect.y = yPos;
            height += fieldRect.height + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(fieldRect, myOtherInt);
        }

        height += EditorGUIUtility.standardVerticalSpacing * 2;
        property.FindPropertyRelative("elementHeight").floatValue = height;
        
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }
}
