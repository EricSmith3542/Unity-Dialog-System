using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogGraphEditor))]
public class DialogGraphEditor : Editor
{
    private SerializedProperty myProperty;

    private void OnEnable()
    {
        myProperty = serializedObject.FindProperty("myVariable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display the property field in the inspector
        EditorGUILayout.PropertyField(myProperty);

        serializedObject.ApplyModifiedProperties();
    }
}