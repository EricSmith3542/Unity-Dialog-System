using UnityEngine;
using UnityEditor;

public class MyCustomEditorWindow : EditorWindow
{
    private MyEditorScript myEditorScript;

    [MenuItem("Window/My Custom Editor")]
    public static void ShowWindow()
    {
        GetWindow<MyCustomEditorWindow>("My Custom Editor");
    }

    private void OnEnable()
    {
        myEditorScript = new MyEditorScript();
    }

    private void OnGUI()
    {
        myEditorScript.OnGUI();
    }
}
