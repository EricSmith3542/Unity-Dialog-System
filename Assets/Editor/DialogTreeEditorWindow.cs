using System;
using System.Collections.Generic;
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
    private static bool openingExisting = false;
    private bool initDone = false;

    [MenuItem("Assets/Create/Dialog Tree")]
    public static void OpenWindow()
    {
        //TODO: Dock the window on creation
        EditorWindow window = GetWindow<DialogTreeEditorWindow>();
        window.titleContent = new GUIContent("New Dialog Tree");
        window.Show();
    }

    public static void OpenWindow(DialogTree dialogTree)
    {
        asset = dialogTree;
        OpenWindow();
    }

    [MenuItem("Assets/Open DialogTree Editor")]
    private static void OpenDialogTreeEditor()
    {
        UnityEngine.Object[] selectedDialogTrees = Selection.GetFiltered(typeof(DialogTree), SelectionMode.Assets);
        if (selectedDialogTrees.Length > 0 && selectedDialogTrees[0] is DialogTree)
        {
            openingExisting = true;
            OpenWindow((DialogTree)selectedDialogTrees[0]);
        }
        else
        {
            EditorUtility.DisplayDialog("Alert", "In order to open the Dialog Tree Editor, please select a Dialog Tree ScriptableObject or go to Create > Dialog Tree", "OK");
        }
    }

    private void OnEnable()
    {
        //Initialize the DialogTree creating a new asset
        if (!initDone)
        {
            //Initialize the graph view
            graphView = new DialogTreeGraphView();
            graphView.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));
            graphView.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
            rootVisualElement.Add(graphView);

            //Reset node counts
            dialogNodesCreated = 0;
            booleanNodesCreated = 0;

            //openingExisting is true when the editor window is opened using the OpenDialogTreeEditor method
            if (!openingExisting)
            {
                OpenNewGraph();
            }
            else
            {
                OpenSavedGraph();
            }
            initDone = true;
        }

        AddUIButtons();
    }

    private static void OpenNewGraph()
    {
        //Create Starting Node
        DialogNode node = new DialogNode(graphView, "dialog-" + GetNextDialogNodeId());

        //Add node to graph view
        graphView.AddElement(node);


        asset = CreateInstance<DialogTree>();
        asset.treeName = "New Dialog Tree";
        asset.startNode = (DialogNodeData)node.AsData();
    }

    private static void OpenSavedGraph()
    {
        BuildNodesFromSaveData();
        BuildEdgesFromSaveData();
        openingExisting = false;
    }

    private static void BuildEdgesFromSaveData()
    {
        foreach (NodeData data in asset.nodes)
        {
            switch (data)
            {
                case DialogNodeData dialogData:
                    DialogNode dialogNode = graphView.Query<DialogNode>().Where(node => node.id == dialogData.id).First();
                    foreach (Port output in dialogNode.outputContainer.Query<Port>().ToList())
                    {
                        List<string> connectedNodeIds = dialogData.outputPortsConnectionsMap.Get(output.portName);
                        if (connectedNodeIds != null)
                        {
                            foreach (string nodeId in connectedNodeIds)
                            {
                                Port input = graphView.Query<Port>().Where(p => p.direction == Direction.Input && p.GetFirstAncestorOfType<DialogNode>() != null && p.GetFirstAncestorOfType<DialogNode>().id == nodeId);
                                Edge edge = input.ConnectTo(output);
                                graphView.Add(edge);
                            }
                        }
                    }
                    break;
                case BooleanNodeData boolData:
                    Port boolNodePort = graphView.Query<Port>().Where(p => p.direction == Direction.Output && p.GetFirstAncestorOfType<BooleanNode>() != null && p.GetFirstAncestorOfType<BooleanNode>().id == boolData.id).First();
                    foreach (string nodeId in boolData.connectedNodeIds)
                    {
                        Port input = graphView.Query<Port>().Where(p => p.direction == Direction.Input && p.GetFirstAncestorOfType<DialogNode>() != null && p.GetFirstAncestorOfType<DialogNode>().id == nodeId);
                        Edge edge = input.ConnectTo(boolNodePort);
                        graphView.Add(edge);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private static void BuildNodesFromSaveData()
    {
        foreach (NodeData data in asset.nodes)
        {
            //Create and add node to the graph
            DialogTreeNode node = data.AsNode(graphView);
            graphView.AddElement(node);

            //Increment approriate node count
            switch (node)
            {
                case DialogNode d:
                    dialogNodesCreated++;
                    break;
                case BooleanNode b:
                    booleanNodesCreated++;
                    break;
                default:
                    Debug.Log("This is a new node type, and the node counts are not being updated properly. Add a case for this node type.");
                    break;
            }
        }
    }

    private void AddUIButtons()
    {
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
        DialogNode another;
        if (isShortcut)
        {
            another = new DialogNode("Dialog " + dialogNodesCreated, graphView, GetNextDialogNodeId(), new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new DialogNode("Dialog " + dialogNodesCreated, graphView, GetNextDialogNodeId());
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
        BooleanNode another;
        if (isShortcut)
        {
            another = new BooleanNode("Condition " + booleanNodesCreated, graphView, GetNextBooleanNodeId(), new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new BooleanNode("Condition " + booleanNodesCreated, graphView, GetNextBooleanNodeId());
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
        //Set the list of nodes and edges on the asset
        List<NodeData> nodes = new List<NodeData>();
        List<Edge> test = graphView.Query<Edge>().ToList();
        foreach (DialogTreeNode element in graphView.Query<DialogTreeNode>().ToList())
        {
            NodeData data = element.AsData();
            if (data is DialogNodeData)
            {
                ((DialogNodeData)data).outputPortsConnectionsMap.StoreAsString();
            }
            else if(data is BooleanNodeData) 
            {
                ((BooleanNodeData)data).StoreIdsAsString();
            }
            nodes.Add(data);
        }
        asset.nodes = nodes;

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
            ScriptableObject oldAsset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            if(oldAsset != null)
            {
                EditorUtility.CopySerialized(asset, oldAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                EditorUtility.DisplayDialog("Alert", "The old version of this dialog tree could not be found, and therefore cannot be overwritten", "OK");
            }

            
        }
    }
}
