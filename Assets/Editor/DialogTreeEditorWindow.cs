using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditorWindow : EditorWindow
{
    private static DialogTreeGraphView graphView;
    private static DialogTree asset;
    private static Vector2 mousePos;
    private static int dialogNodesCreated = 0;
    private static int booleanNodesCreated = 0;

    [MenuItem("Assets/Create/Dialog Tree")]
    public static void OpenWindow()
    {
        //TODO: Dock the window on creation
        EditorWindow window = GetWindow<DialogTreeEditorWindow>();
        window.titleContent = new GUIContent("New Dialog Tree");
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
        DialogNode node = new DialogNode(graphView, GetNextDialogNodeId());

        //Add node to graph view
        graphView.AddElement(node);

        //Initialize the DialogTree
        asset = ScriptableObject.CreateInstance<DialogTree>();
        asset.treeName = "New Dialog Tree";
        asset.startNode = node;


        Button dialogNodeAddButton = new Button(() => AddDialogNode());
        dialogNodeAddButton.text = "Create New Dialog Node";
        dialogNodeAddButton.tooltip = "Add Dialog Node";
        rootVisualElement.Add(dialogNodeAddButton);

        Button conditionNodeAddButton = new Button(() => AddBooleanNode());
        conditionNodeAddButton.text = "Create New Boolean Node";
        conditionNodeAddButton.tooltip = "Add Boolean Node";
        rootVisualElement.Add(conditionNodeAddButton);

        Button saveButton = new Button(() => CreateOrSaveAsset());
        saveButton.text = "Save";
        saveButton.tooltip = "Create/Save the DialogTree";
        rootVisualElement.Add(saveButton);
    }

    private static string GetNextDialogNodeId()
    {
        return "dialog-" + dialogNodesCreated++;
    }
    private static string GetNextBooleanNodeId()
    {
        return "bool-" + booleanNodesCreated++;
    }

    private static void AddDialogNode(bool isShortcut = false)
    {
        Node another;
        int numNodes = graphView.graphElements.ToList().Count;
        if (isShortcut)
        {
            another = new DialogNode("Node " + numNodes, graphView, GetNextDialogNodeId(), new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new DialogNode("Node " + numNodes, graphView, GetNextDialogNodeId());
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
        int numNodes = graphView.graphElements.ToList().Count;
        if (isShortcut)
        {
            another = new BooleanNode("Node " + numNodes, graphView, GetNextBooleanNodeId(), new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new BooleanNode("Node " + numNodes, graphView, GetNextBooleanNodeId());
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

    
    public static void CreateOrSaveAsset()
    {
        // Check if asset already exists
        string assetPath = AssetDatabase.GetAssetPath(asset);
        if (string.IsNullOrEmpty(assetPath))
        {
            // Asset doesn't exist, create new asset
            assetPath = EditorUtility.SaveFilePanelInProject("Save Dialog Tree Asset", "New Dialog Tree", "asset", "Please enter a file name to save the Dialog Tree asset to");
            if (string.IsNullOrEmpty(assetPath)) return; // User cancelled save dialog

            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            // Asset already exists, ask user if they want to overwrite it
            if (!EditorUtility.DisplayDialog("Overwrite Existing Asset?", "The asset already exists. Do you want to overwrite it?", "Yes", "No"))
                return;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
