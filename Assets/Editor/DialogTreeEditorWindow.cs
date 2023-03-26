using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditorWindow : EditorWindow
{
    private GraphView graphView;

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
        Node node = new DialogNode();

        //Add node to graph view
        graphView.AddElement(node);

        Node another = new DialogNode("TEst");
        graphView.AddElement(another);
    }

    private void OnGUI()
    {
        
    }
}

public class DialogNode : Node
{
    string dialog = "";
    public string Dialog { get => dialog; set => dialog = value; }

    int numOutputs = 0;

    private TextField m_TitleField;

    public DialogNode(string title)
    {
        this.title = title;
        SetPosition(new Rect(0, 0, 0, 0));
        BuildRequirementsPort();
        BuildPortAddSubtractButtons();
        AddOutputPort();
        BuildTextArea();
        SetNodeCapabilites();
        this.AddManipulator(new NodeDragger());
        this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
    }

    public DialogNode() : this("Start") { }

    
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
        input.portName = "Requirements";
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
        Port output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        output.portName = "Option " + numOutputs++;
        outputContainer.Add(output);
    }
    private void RemoveOutputPort()
    {
        outputContainer.RemoveAt(outputContainer.childCount-1);
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

    private class NodeDragger : MouseManipulator
    {
        private bool m_Active;
        private Vector2 m_Start;
        private float t_Start_left, t_Start_top;

        public NodeDragger()
        {
            this.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent e)
        {
            if (e.button == (int)MouseButton.LeftMouse)
            {
                m_Active = true;
                m_Start = e.mousePosition;
                t_Start_left = target.style.left.value.value;
                t_Start_top = target.style.top.value.value;
                target.CaptureMouse();
            }
        }

        private void OnMouseMove(MouseMoveEvent e)
        {
            if (m_Active)
            {
                target.style.left = t_Start_left + (e.mousePosition.x - m_Start.x);
                target.style.top = t_Start_top + (e.mousePosition.y - m_Start.y);
                e.StopPropagation();
            }
        }

        private void OnMouseUp(MouseUpEvent e)
        {
            if (e.button == (int)MouseButton.LeftMouse && m_Active)
            {
                m_Active = false;
                target.ReleaseMouse();
                e.StopPropagation();
            }
        }
    }
}

public class DialogTreeGraphView : GraphView
{
    public DialogTreeGraphView()
    {
        // Add any initialization code here
    }
}