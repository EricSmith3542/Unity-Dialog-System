using UnityEngine;
using UnityEditor;

public class MyEditorScript
{
    private SerializedObject serializedObject;
    private SerializedProperty myProperty;

    public MyEditorScript()
    {
        // Initialize the serialized object
        serializedObject = new SerializedObject(Selection.activeGameObject.GetComponent<MyMonoBehaviourScript>());

        // Initialize the serialized property
        myProperty = serializedObject.FindProperty("myVariable");
    }

    public void OnGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Display the property field in the inspector
        EditorGUILayout.PropertyField(myProperty);

        // Apply any modifications to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}
