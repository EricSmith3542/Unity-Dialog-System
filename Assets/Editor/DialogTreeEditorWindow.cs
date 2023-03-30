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
        Node node = new DialogTreeGraphView.DialogNode(graphView);

        //Add node to graph view
        graphView.AddElement(node);

        var nodeAddButton = new Button(() => AddNode());
        nodeAddButton.text = "Create New Node";
        nodeAddButton.tooltip = "Add Node";
        rootVisualElement.Add(nodeAddButton);
    }


    private static void AddNode(bool isShortcut = false)
    {
        Node another;
        if (isShortcut)
        {
            another = new DialogTreeGraphView.DialogNode("Node " + graphView.graphElements.ToList().Count, graphView, new Rect(mousePos, Vector2.zero));
        }
        else
        {
            another = new DialogTreeGraphView.DialogNode("Node " + graphView.graphElements.ToList().Count, graphView);
        }

        graphView.AddElement(another);
    }

    [Shortcut("DialogTreeEditor/Shift-N", KeyCode.N, ShortcutModifiers.Shift)]
    private static void AddNodeShortCut()
    {
        AddNode(true);
    }

    private void OnGUI()
    {
        mousePos = Event.current.mousePosition;
    }
}

public class DialogTreeGraphView : GraphView
{
    public DialogTreeGraphView()
    {
        // Add selection dragger to the view
        var selectionDragger = new NodeSelectionDragger(this);
        this.AddManipulator(selectionDragger);        
    }

    public List<Port> GetCompatiblePorts(Port startPort)
    {
        List<Port> compatiblePorts = new List<Port>();
        foreach (Port port in ports)
        {
            if(port != startPort && port.node != startPort.node && port.direction != startPort.direction && port.portType == startPort.portType)
            {
                compatiblePorts.Add(port);
            }
        }
        return compatiblePorts;
    }

    public class DialogNode : Node
    {
        string dialog = "";
        public string Dialog { get => dialog; set => dialog = value; }

        int numOutputs = 0;

        private DialogTreeGraphView graphView;

        public DialogNode(string title, DialogTreeGraphView gv, Rect pos)
        {
            graphView = gv;
            SetPosition(pos);
            BuildRequirementsPort();
            BuildPortAddSubtractButtons();
            AddOutputPort();
            BuildTextArea();
            SetNodeCapabilites();
            this.AddManipulator(new NodeDragger(graphView));
            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));

            //Replace the borring titleLabel with an EditableLabel
            titleContainer.Remove(this.Query<Label>("title-label").First());
            var editableLabel = new EditableLabel(title);
            titleContainer.Insert(0, editableLabel);

        }
        public DialogNode(string title, DialogTreeGraphView gv) : this(title, gv, new Rect(0, 0, 0, 0)) { }
        public DialogNode(DialogTreeGraphView gv) : this("Start", gv) { }
        



        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // Add a menu item that performs some action
            evt.menu.AppendAction("Do Something", action => DoSomething(), DropdownMenuAction.AlwaysEnabled);
        }

        private void DoSomething()
        {
            // Perform some action
        }

        private void BuildRequirementsPort()
        {
            Port input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            input.AddManipulator(new EdgeDragHelper(graphView));
            input.portName = "Requirements";
            input.portColor = Color.green;
            inputContainer.Add(input);
        }

        private void BuildPortAddSubtractButtons()
        {
            var addButton = new Button(AddOutputPort);
            addButton.text = "+";
            addButton.tooltip = "Add Response Option";
            outputContainer.Add(addButton);

            var subButton = new Button(RemoveOutputPort);
            subButton.text = "-";
            subButton.tooltip = "Remove Last Response Option";
            outputContainer.Add(subButton);
        }

        private void AddOutputPort()
        {
            Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            output.AddManipulator(new EdgeDragHelper(graphView));
            output.portName = "Option " + numOutputs++;
            output.portColor = Color.red;
            outputContainer.Add(output);
        }
        private void RemoveOutputPort()
        {
            outputContainer.RemoveAt(outputContainer.childCount - 1);
            numOutputs--;
        }

        private void SetNodeCapabilites()
        {
            capabilities |= Capabilities.Movable |
                                    Capabilities.Resizable |
                                    Capabilities.Deletable |
                                    Capabilities.Selectable |
                                    Capabilities.Copiable |
                                    Capabilities.Ascendable |
                                    Capabilities.Collapsible |
                                    Capabilities.Droppable |
                                    Capabilities.Renamable |
                                    Capabilities.Snappable |
                                    Capabilities.Groupable |
                                    Capabilities.Stackable;
        }

        private void BuildTextArea()
        {
            var foldout = new Foldout();
            foldout.text = "Dialogue Text";
            mainContainer.Add(foldout);

            var textField = new TextField();
            textField.multiline = true;
            textField.RegisterValueChangedCallback(evt =>
            {
                Dialog = evt.newValue;
            });

            //Nest text area in scrollview and foldout
            ScrollView scroll = new ScrollView();
            scroll.Add(textField);
            foldout.Add(scroll);
        }
    }
}
