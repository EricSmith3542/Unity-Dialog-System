using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class MyCustomEditorWindow : EditorWindow
{
    private GraphView graphView;

    [MenuItem("Window/My Graph Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow<MyCustomEditorWindow>();
        window.titleContent = new GUIContent("My Graph Editor");
        window.Show();
    }

    private void OnEnable()
    {
        graphView = new MyGraphView();
        graphView.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
        graphView.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
        graphView.name = "My Graph View";

        var node = new Node();
        node.title = "My Node";
        node.SetPosition(new Rect(300, 300, 600, 600));
        graphView.AddElement(node);

        //var scrollView = new ScrollView();
        //scrollView.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
        //scrollView.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
        //scrollView.Add(graphView);
        rootVisualElement.Add(graphView);
    }
}

public class MyGraphView : GraphView
{
    public MyGraphView()
    {
        // Add any initialization code here
    }
}