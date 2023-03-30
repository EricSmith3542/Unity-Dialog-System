using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditorWindow : EditorWindow
{
    private static DialogTreeGraphView graphView;
    private static Vector2 mousePos;

    [MenuItem("Window/Dialog Tree Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow<DialogTreeEditorWindow>();
        window.titleContent = new GUIContent("Dialog Tree Editor");
        window.Show();
    }

    private void OnEnable()
    {
        //Initialize the graph view
        graphView = new DialogTreeGraphView();
        graphView.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
        graphView.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
        //Add graph view to window
        rootVisualElement.Add(graphView);

        //Create Starting Node
        Node node = new DialogNode(graphView);

        //Add node to graph view
        graphView.AddElement(node);

        Button dialogNodeAddButton = new Button(() => AddDialogNode());
        dialogNodeAddButton.text = "Create New Dialog Node";
        dialogNodeAddButton.tooltip = "Add Dialog Node";
        rootVisualElement.Add(dialogNodeAddButton);

        Button conditionNodeAddButton = new Button(() => AddBooleanNode());
        conditionNodeAddButton.text = "Create New Boolean Node";
        conditionNodeAddButton.tooltip = "Add Boolean Node";
        rootVisualElement.Add(conditionNodeAddButton);
    }


    private static void AddDialogNode(bool isShortcut = false)
    {
        Node another;
        if (isShortcut)
        {
            another = new DialogNode("Node " + graphView.graphElements.ToList().Count, graphView, new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new DialogNode("Node " + graphView.graphElements.ToList().Count, graphView);
        }

        graphView.AddElement(another);
    }

    [Shortcut("DialogTreeEditor/Shift-N", KeyCode.N, ShortcutModifiers.Shift)]
    private static void AddDialogNodeShortCut()
    {
        AddDialogNode(true);
    }

    private static void AddBooleanNode(bool isShortcut = false)
    {
        Node another;
        if (isShortcut)
        {
            another = new BooleanNode("Node " + graphView.graphElements.ToList().Count, graphView, new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new BooleanNode("Node " + graphView.graphElements.ToList().Count, graphView);
        }

        graphView.AddElement(another);
    }

    [Shortcut("DialogTreeEditor/Shift-B", KeyCode.B, ShortcutModifiers.Shift)]
    private static void AddBooleanNodeShortCut()
    {
        AddBooleanNode(true);
    }

    private void OnGUI()
    {
        mousePos = Event.current.mousePosition;
    }
}
